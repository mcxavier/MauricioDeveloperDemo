using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Dtos;
using Infra.EntitityConfigurations.Contexts;
using Infra.ExternalServices.Authentication;
using MediatR;
using Core.QuerysCommands.Queries.Insights;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Core.Models.Core.Ordering;
using Core.Domains.Ordering.Models;

namespace Infra.QueryCommands.QueriesHandlers.Customers
{
    public class GetInsightsByFilterHandler : IRequestHandler<GetInsightsByFilterQuery, GetInsightsByFilterResponse>
    {

        private readonly SmartSalesIdentity _identity;
        private readonly CoreContext _context;
        private readonly IdentityContext _identityContext;
        private readonly string _specifier;
        private readonly CultureInfo _culture;
        private int? _countOrders;

        public GetInsightsByFilterHandler(SmartSalesIdentity smartSalesIdentity, CoreContext context, IdentityContext identityContext)
        {
            this._identity = smartSalesIdentity;
            this._context = context;
            this._identityContext = identityContext;
            this._specifier = "F";
            this._culture = CultureInfo.CreateSpecificCulture("pt-BR");
            this._countOrders = 0;
        }

        private IQueryable<Order> OrderQuery(GetInsightsByFilterQuery request)
        {
            return this._context.Orders.Where(x => x.StoreCode == this._identity.CurrentStoreCode
                                                && x.Status.Id != (int)OrderStatusEnum.PaymentFailed
                                                && x.Status.Id != (int)OrderStatusEnum.Cancelled
                                                && x.Status.Id != (int)OrderStatusEnum.IntegrationFailed
                                                && x.Status.Id != (int)OrderStatusEnum.AwaitingValidation
                                                && x.Status.Id != (int)OrderStatusEnum.Submitted
                                                && (!request.BeginsAtInitialDate.HasValue || x.CreatedAt.Value >= request.BeginsAtInitialDate)
                                                && (!request.BeginsAtFinalDate.HasValue || x.CreatedAt.Value <= request.BeginsAtFinalDate)
                                            );
        }

        private IQueryable<OrderItem> OrderItemsQuery(GetInsightsByFilterQuery request)
        {
            return this._context.OrderItems.Include("Order").Include("Product")
                .Include("ProductVariation")
                .Where(x => x.Order.StoreCode == this._identity.CurrentStoreCode
                        && x.Order.Status.Id != (int)OrderStatusEnum.PaymentFailed
                        && x.Order.Status.Id != (int)OrderStatusEnum.Cancelled
                        && x.Order.Status.Id != (int)OrderStatusEnum.IntegrationFailed
                        && x.Order.Status.Id != (int)OrderStatusEnum.AwaitingValidation
                        && x.Order.Status.Id != (int)OrderStatusEnum.Submitted
                        && (!request.BeginsAtInitialDate.HasValue || x.Order.CreatedAt.Value >= request.BeginsAtInitialDate)
                        && (!request.BeginsAtFinalDate.HasValue || x.Order.CreatedAt.Value <= request.BeginsAtFinalDate)
                    );
        }

        private InsightsResumeDto GetIncome(GetInsightsByFilterQuery request)
        {
            try
            {
                decimal totalIncome = OrderQuery(request).Sum(x => x.Value);
                return new InsightsResumeDto
                {
                    Type = "value",
                    Title = "Faturamento",
                    Value = "R$ " + totalIncome.ToString(this._specifier, this._culture)
                };
            }
            catch
            {
                return null;
            }
        }

        private InsightsResumeDto GetAverageTicket(GetInsightsByFilterQuery request)
        {
            try
            {
                decimal averageTicketValue = OrderQuery(request).Average(x => x.Value);
                this._countOrders = OrderQuery(request).Count();
                return new InsightsResumeDto
                {
                    Type = "value",
                    Title = "Ticket médio",
                    Value = "R$ " + averageTicketValue.ToString(this._specifier, this._culture),
                    TotalValue = this._countOrders.Value + " atendimentos"
                };
            }
            catch
            {
                return null;
            }
        }

        private InsightsResumeDto GeAttendance(GetInsightsByFilterQuery request)
        {
            try
            {
                this._countOrders = this._countOrders ?? OrderQuery(request).Count();
                int countItems = OrderItemsQuery(request).Sum(x => x.Units);
                decimal averageProductsPerOrder = (decimal)((float)countItems / (float)this._countOrders);
                return new InsightsResumeDto
                {
                    Type = "value",
                    Title = "Peças por atendimento",
                    Value = averageProductsPerOrder.ToString(this._specifier, this._culture),
                    TotalValue = this._countOrders.Value + " atendimentos"
                };
            }
            catch
            {
                return null;
            }
        }

        private InsightsResumeDto GetPayments(GetInsightsByFilterQuery request)
        {
            try
            {
                IList<InsightDataDto> data = OrderQuery(request)
                    .Where(order => order.Payment.PaymentType != null)
                    .Select(order => new InsightDataDto
                    {
                        Label = order.Payment.PaymentType.Name,
                        Value = ((decimal)order.Payment.Amount) / (decimal)100.0
                    })
                    .GroupBy(x => x.Label,
                        x => x.Value,
                        (label, values) => new InsightDataDto
                        {
                            Label = label,
                            Value = values.Sum(x => x)
                        })
                    .OrderByDescending(x => x.Value)
                    .ToList();

                if (data.Count() == 0) throw new Exception("No payment is available.");

                return new InsightsResumeDto { Type = "sector", Title = "Formas de pagamento", UnitLeft = "R$ ", Data = data };
            }
            catch
            {
                return null;
            }
        }

        private InsightsResumeDto GetTopSellers(GetInsightsByFilterQuery request)
        {
            try
            {
                IList<InsightDataDto> topSellers = OrderQuery(request)
                    .Include("Seller")
                    .Where(x => x.Seller != null)
                    .GroupBy(order => order.Seller.Name, (sellerName, orders) => new InsightDataDto
                    {
                        Label = sellerName,
                        Value = orders.Sum(order => order.Value)
                    })
                    .OrderByDescending(x => x.Value)
                    .Take(5) // Take Top 5 or less than 5
                    .ToList();

                if (topSellers.Count() == 0) throw new Exception("No sellers on top.");

                return new InsightsResumeDto { Type = "rank", Title = "Top " + topSellers.Count() + " vendedores", UnitLeft = "R$ ", Data = topSellers };
            }
            catch
            {
                return null;
            }
        }

        private InsightsResumeDto GetTopProducts(GetInsightsByFilterQuery request)
        {
            try
            {
                IList<InsightDataDto> topProducts = OrderItemsQuery(request)
                    .Select(orderItem => new InsightDataDto
                    {
                        Label = orderItem.ProductName,
                        Value = (orderItem.UnitPrice - (orderItem.UnitDiscount ?? 0)) * orderItem.Units
                    })
                    .GroupBy(pv => pv.Label,
                        pv => pv.Value,
                        (label, values) => new InsightDataDto
                        {
                            Label = label,
                            Value = values.Sum(x => x)
                        })
                    .OrderByDescending(x => x.Value)
                    .Take(5) // Take Top 5 or less than 5
                    .ToList();

                if (topProducts.Count() == 0) throw new Exception("No products on top.");

                return new InsightsResumeDto { Type = "rank", Title = "Top " + topProducts.Count() + " produtos", UnitLeft = "R$ ", Data = topProducts };
            }
            catch
            {
                return null;
            }
        }

        private InsightsResumeDto GetTopCustomers(GetInsightsByFilterQuery request)
        {
            try
            {
                IList<InsightDataDto> topCustomers = OrderQuery(request)
                    .Select(order => new InsightDataDto
                    {
                        Label = order.Customer.Name,
                        Value = order.Value
                    })
                    .GroupBy(x => x.Label,
                        x => x.Value,
                        (label, values) => new InsightDataDto
                        {
                            Label = label,
                            Value = values.Sum(x => x)
                        })
                    .OrderByDescending(x => x.Value)
                    .Take(5) // Take Top 5 or less than 5
                    .ToList();

                if (topCustomers.Count() == 0) throw new Exception("No customers on top.");

                return new InsightsResumeDto { Type = "rank", Title = "Top " + topCustomers.Count() + " clientes", UnitLeft = "R$ ", Data = topCustomers };
            }
            catch
            {
                return null;
            }
        }

        private string MonthFromNumber(int monthNumber)
        {
            switch (monthNumber)
            {
                case 1: return "Jan";
                case 2: return "Fev";
                case 3: return "Mar";
                case 4: return "Abr";
                case 5: return "Mai";
                case 6: return "Jun";
                case 7: return "Jul";
                case 8: return "Ago";
                case 9: return "Set";
                case 10: return "Out";
                case 11: return "Nov";
                case 12: return "Dez";
                default: return "?";
            }
        }

        private InsightsResumeDto GetConversionRate(GetInsightsByFilterQuery request)
        {
            // TODO: GetConversionRate implementation
            List<InsightDataDto> data = new List<InsightDataDto>();

            data.Add(new InsightDataDto { Label = "Jan", Value = (decimal)20 });
            data.Add(new InsightDataDto { Label = "Fev", Value = (decimal)10 });
            data.Add(new InsightDataDto { Label = "Mar", Value = (decimal)13 });
            data.Add(new InsightDataDto { Label = "Abr", Value = (decimal)19 });
            data.Add(new InsightDataDto { Label = "Mai", Value = (decimal)30 });
            data.Add(new InsightDataDto { Label = "Jun", Value = (decimal)27 });
            data.Add(new InsightDataDto { Label = "Jul", Value = (decimal)40 });
            data.Add(new InsightDataDto { Label = "Ago", Value = (decimal)13 });
            data.Add(new InsightDataDto { Label = "Set", Value = (decimal)33 });
            data.Add(new InsightDataDto { Label = "Out", Value = (decimal)26 });
            data.Add(new InsightDataDto { Label = "Nov", Value = (decimal)36 });
            data.Add(new InsightDataDto { Label = "Dez", Value = (decimal)18 });

            return new InsightsResumeDto { Type = "bar", Title = "Taxa de conversão", UnitRight = "Compras", Value = "72%", Data = data };
        }

        private InsightsResumeDto GetCartDropRate(GetInsightsByFilterQuery request)
        {
            // TODO: GetCartDropRate implementation
            List<InsightDataDto> data = new List<InsightDataDto>();

            data.Add(new InsightDataDto { Label = "Jan", Value = (decimal)20 });
            data.Add(new InsightDataDto { Label = "Fev", Value = (decimal)10 });
            data.Add(new InsightDataDto { Label = "Mar", Value = (decimal)13 });
            data.Add(new InsightDataDto { Label = "Abr", Value = (decimal)19 });
            data.Add(new InsightDataDto { Label = "Mai", Value = (decimal)30 });
            data.Add(new InsightDataDto { Label = "Jun", Value = (decimal)27 });
            data.Add(new InsightDataDto { Label = "Jul", Value = (decimal)40 });
            data.Add(new InsightDataDto { Label = "Ago", Value = (decimal)13 });
            data.Add(new InsightDataDto { Label = "Set", Value = (decimal)33 });
            data.Add(new InsightDataDto { Label = "Out", Value = (decimal)26 });
            data.Add(new InsightDataDto { Label = "Nov", Value = (decimal)36 });
            data.Add(new InsightDataDto { Label = "Dez", Value = (decimal)18 });

            return new InsightsResumeDto { Type = "bar", Title = "Taxa de abandono de carrinho", Value = "21%", UnitRight = "Abandonos/Carrinhos", Data = data };
        }

        private InsightsResumeDto GetWhatsAppSubmissions(GetInsightsByFilterQuery request)
        {
            int amountOfYears = 1;
            var dataSeries = new List<List<InsightDataDto>>();
            var dataSeriesCombined = new List<InsightDataDto>();

            DateTime todayLastYear = DateTime.Today.AddYears(-amountOfYears);
            var defaultMonths = new List<InsightValueMonthYearDto>();

            for (int i = 0; i < 12 * amountOfYears; ++i)
            {
                DateTime defaultMonth = DateTime.Today.AddMonths(-i);
                defaultMonths.Add(new InsightValueMonthYearDto { Value = 0, Month = defaultMonth.Month, Year = defaultMonth.Year });
            }

            List<InsightDataDto> data = this._context.CatalogCustomers
                .Where(x => x.SentAt.HasValue
                    && x.SentAt.Value >= todayLastYear
                    && (!request.BeginsAtInitialDate.HasValue || x.SentAt.Value >= request.BeginsAtInitialDate)
                    && (!request.BeginsAtFinalDate.HasValue || x.SentAt.Value <= request.BeginsAtFinalDate)
                    && x.Catalog.StoreCode == this._identity.CurrentStoreCode
                )
                .Select(catalogs => new InsightValueMonthYearDto
                {
                    Value = 1,
                    Month = catalogs.SentAt.Value.Month,
                    Year = catalogs.SentAt.Value.Year
                })
                .ToList()
                .Union(defaultMonths)
                .GroupBy(x => new { x.Month, x.Year },
                    x => x.Value,
                    (catalog, values) => new
                    {
                        Value = values.Sum(x => x),
                        Month = catalog.Month,
                        Year = catalog.Year
                    })
                .OrderBy(x => x.Year * 12 + x.Month)
                .Take(amountOfYears * 12)
                .ToList().Select(x => new InsightDataDto
                {
                    Label = MonthFromNumber(x.Month),
                    Value = x.Value,
                    UnitRight = x.Year.ToString()
                }).ToList();

            var totalSentCatalogCustomer = (int)data.Sum(x => x.Value);

            data.ForEach((element) =>
            {
                dataSeriesCombined.Add(element);
            });

            return new InsightsResumeDto
            {
                Type = "bar",
                Title = "Quantidade de envios de Whatsapp",
                Value = totalSentCatalogCustomer.ToString(),
                UnitRight = "Envios",
                Data = dataSeriesCombined,
                DataSeries = dataSeries
            };
        }

        private InsightsResumeDto GetCatalogOpenings(GetInsightsByFilterQuery request)
        {
            // TODO: GetCatalogOpenings implementation
            List<InsightDataDto> data = new List<InsightDataDto>();

            data.Add(new InsightDataDto { Label = "Jan", Value = (decimal)20 });
            data.Add(new InsightDataDto { Label = "Fev", Value = (decimal)10 });
            data.Add(new InsightDataDto { Label = "Mar", Value = (decimal)13 });
            data.Add(new InsightDataDto { Label = "Abr", Value = (decimal)19 });
            data.Add(new InsightDataDto { Label = "Mai", Value = (decimal)30 });
            data.Add(new InsightDataDto { Label = "Jun", Value = (decimal)27 });
            data.Add(new InsightDataDto { Label = "Jul", Value = (decimal)40 });
            data.Add(new InsightDataDto { Label = "Ago", Value = (decimal)13 });
            data.Add(new InsightDataDto { Label = "Set", Value = (decimal)33 });
            data.Add(new InsightDataDto { Label = "Out", Value = (decimal)26 });
            data.Add(new InsightDataDto { Label = "Nov", Value = (decimal)36 });
            data.Add(new InsightDataDto { Label = "Dez", Value = (decimal)18 });

            return new InsightsResumeDto { Type = "bar", Title = "Abertura de catálogos", Value = "3.720", UnitRight = "Aberturas de catálogos", Data = data };
        }

        private InsightsResumeDto GetCatalogCreations(GetInsightsByFilterQuery request)
        {
            int totalCreatedCatalogs = 0;
            int amountOfYears = 1;
            List<List<InsightDataDto>> dataSeries = new List<List<InsightDataDto>>();
            List<InsightDataDto> dataSeriesCombined = new List<InsightDataDto>();

            DateTime todayLastYear = DateTime.Today.AddYears(-amountOfYears);
            List<InsightValueMonthYearDto> defaultMonths = new List<InsightValueMonthYearDto>();
            for (int i = 0; i < 12 * amountOfYears; ++i)
            {
                DateTime defaultMonth = DateTime.Today.AddMonths(-i);
                defaultMonths.Add(new InsightValueMonthYearDto
                {
                    Value = 0,
                    Month = defaultMonth.Month,
                    Year = defaultMonth.Year
                });
            }

            List<InsightDataDto> data = this._context.Catalogs
                .Where(x => x.CreatedAt.HasValue
                    && x.CreatedAt.Value >= todayLastYear
                    && (!request.BeginsAtInitialDate.HasValue || x.CreatedAt.Value >= request.BeginsAtInitialDate)
                    && (!request.BeginsAtFinalDate.HasValue || x.CreatedAt.Value <= request.BeginsAtFinalDate)
                    && x.StoreCode == this._identity.CurrentStoreCode
                )
                .Select(catalogs => new InsightValueMonthYearDto
                {
                    Value = 1,
                    Month = catalogs.CreatedAt.Value.Month,
                    Year = catalogs.CreatedAt.Value.Year
                })
                .ToList()
                .Union(defaultMonths)
                .GroupBy(x => new { x.Month, x.Year },
                    x => x.Value,
                    (catalog, values) => new
                    {
                        Value = values.Sum(x => x),
                        Month = catalog.Month,
                        Year = catalog.Year
                    })
                .OrderBy(x => x.Year * 12 + x.Month)
                .Take(amountOfYears * 12)
                .ToList().Select(x => new InsightDataDto
                {
                    Label = MonthFromNumber(x.Month),
                    Value = x.Value,
                    UnitRight = x.Year.ToString()
                }).ToList();
            totalCreatedCatalogs = (int)data.Sum(x => x.Value);
            data.ForEach((element) =>
            {
                dataSeriesCombined.Add(element);
            });
            return new InsightsResumeDto
            {
                Type = "bar",
                Title = "Número de catálogos criados",
                Value = totalCreatedCatalogs.ToString(),
                UnitRight = "Catálogos novos",
                Data = dataSeriesCombined,
                DataSeries = dataSeries
            };
        }

        public async Task<GetInsightsByFilterResponse> Handle(GetInsightsByFilterQuery request, CancellationToken cancellationToken)
        {
            List<InsightsResumeDto> insights = new List<InsightsResumeDto>();

            /*** Value Insights ***/
            // Getting Income Insight
            insights.Add(GetIncome(request));

            // Getting Average Ticket Insight
            insights.Add(GetAverageTicket(request));

            // Getting Attendance Insight
            insights.Add(GeAttendance(request));

            /*** Bar Plot Insights ***/
            // Getting Conversion Rate Insight
            //insights.Add(GetConversionRate(request));

            // Getting Cart Drop Rate Insight
            //insights.Add(GetCartDropRate(request));

            // Getting Catalog Openings Insight
            //insights.Add(GetCatalogOpenings(request));

            // Getting WhatsApp Submissions Insight
            insights.Add(GetWhatsAppSubmissions(request));

            // Getting Catalog Creations Insight
            insights.Add(GetCatalogCreations(request));

            /*** Sector Plot Insights ***/
            // Getting Payments Insight
            insights.Add(GetPayments(request));

            /*** Rank Plot Insights ***/
            // Getting Top 5 Sellers Insight
            insights.Add(GetTopSellers(request));

            // Getting Top 5 Products Insight
            insights.Add(GetTopProducts(request));

            // Getting Top 5 Customers Insight
            insights.Add(GetTopCustomers(request));

            return new GetInsightsByFilterResponse()
            {
                Insights = insights
            };
        }
    }
}