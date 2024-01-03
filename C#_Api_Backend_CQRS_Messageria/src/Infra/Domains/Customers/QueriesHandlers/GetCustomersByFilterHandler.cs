using System;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Core.Dtos;
using Core.QuerysCommands.Queries.Customers.GetCustomersByFilter;
using Infra.EntitityConfigurations.Contexts;
using Infra.ExternalServices.Authentication;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using Utils.Extensions;

namespace Infra.QueryCommands.QueriesHandlers.Customers
{

    public class GetCustomersByFilterHandler : IRequestHandler<GetCustomersByFilterQuery, GetCustomersByFilterResponse>
    {

        private readonly SmartSalesIdentity _identity;
        private readonly CoreContext _context;
        private readonly IdentityContext _identityContext;

        public GetCustomersByFilterHandler(SmartSalesIdentity smartSalesIdentity,
                                CoreContext context,
                                IdentityContext identityContext)
        {
            this._identity = smartSalesIdentity;
            this._context = context;
            this._identityContext = identityContext;
        }

        public async Task<GetCustomersByFilterResponse> Handle(GetCustomersByFilterQuery request, CancellationToken cancellationToken)
        {
            var parameters = new DynamicParameters(new
            {
                CustomerName = !string.IsNullOrEmpty(request.CustomerName) ? $"%{request.CustomerName}%" : null,
                StoreCode = this._identity.CurrentStoreCode,
                CustomerDocument = !string.IsNullOrEmpty(request.CustomerDocument.UnformatCpfCnpjRg()) ? $"%{request.CustomerDocument.UnformatCpfCnpjRg()}%" : null,
                request.AverageTicketStart,
                request.AverageTicketEnd,
                request.BuyPeriodInitialDate,
                request.BuyPeriodFinalDate,
                request.NotBuyPeriodInitialDate,
                request.NotBuyPeriodFinalDate,
                request.OnlyValidPhone,
                request.Categories,
                request.PaymentTypeId,
                request.Seller,
                request.PageIndex,
                request.PageSize,
                request.AgeMin,
                request.AgeMax
            });

            //customer.Subsidiary AS Subsidiary,
            const string GetCustomersByFilterSql = @"

                DECLARE @birthDayMin datetime = dateadd(year, -@ageMax, GETDATE());
                DECLARE @birthDayMax datetime = dateadd(year, -@ageMin, GETDATE());

                ;WITH FilteredCustomers (Id, Name, CPF, Birth, Phone, Subsidiary, Seller, AverageTicket, LastBuy)
                AS (
                SELECT
                    customer.Id AS Id,
                    customer.Name AS Name,
                    customer.Cpf AS CPF,
                    customer.BirthDay AS Birth,
                    customer.Phone AS Phone,
                    customer.StoreCode AS Subsidiary,
                    theseller.Name AS Seller,
                    AVG(theorder.Value) AS AverageTicket,
                    MAX(theorder.OrderedAt) AS LastBuy
                FROM customer.Customers AS customer (NOLOCK)
                    LEFT JOIN [order].[Orders] AS theorder (NOLOCK) ON theorder.CustomerId = customer.Id
                    LEFT JOIN payment.Payments AS payment (NOLOCK) ON payment.Id = theorder.PaymentId
                    LEFT JOIN customer.Sellers AS theseller (NOLOCK) ON theseller.Id = theorder.SellerId
                WHERE (@storeCode IS NULL OR customer.StoreCode = @storeCode OR customer.StoreCode IS NULL) AND
                      (@seller IS NULL OR theseller.Id = @seller) AND
                      (@paymentTypeId IS NULL OR payment.PaymentTypeId = @paymentTypeId) AND
                      (@customerName IS NULL OR customer.Name LIKE @customerName) AND
                      (@customerDocument IS NULL OR (customer.Cpf LIKE @customerDocument) OR (customer.Rg LIKE @customerDocument)) AND
                      ((@onlyValidPhone = 0) OR (customer.Phone IS NOT NULL AND customer.Phone != '')) AND
                      (@buyPeriodInitialDate IS NULL OR theorder.OrderedAt BETWEEN @buyPeriodInitialDate AND @buyPeriodFinalDate) AND
                      (@notBuyPeriodInitialDate IS NULL OR theorder.OrderedAt NOT BETWEEN @notBuyPeriodInitialDate AND @notBuyPeriodFinalDate) AND
                      ((@ageMax = 90 AND @ageMin = 0) OR customer.BirthDay BETWEEN @birthDayMin AND @birthDayMax)
                GROUP BY 
                    customer.Id,
                    customer.Name,
                    customer.BirthDay,
                    customer.Phone,
                    customer.StoreCode,
                    customer.Cpf,
                    customer.Rg,
                    theseller.Name
                HAVING (@averageTicketStart = 0 OR AVG(theorder.Value) >= @averageTicketStart) AND
                       (@averageTicketEnd = 0   OR AVG(theorder.Value) <= @AverageTicketEnd)
                )

                SELECT COUNT(*) AS Count FROM FilteredCustomers;

                ;WITH FilteredCustomers (Id, Name, CPF, Birth, Phone, Subsidiary, Seller, AverageTicket, LastBuy)
                AS (
                SELECT
                    customer.Id AS Id,
                    customer.Name AS Name,
                    customer.Cpf AS CPF,
                    customer.BirthDay AS Birth,
                    customer.Phone AS Phone,
                    customer.StoreCode AS Subsidiary,
                    theseller.Name AS Seller,
                    AVG(theorder.Value) AS AverageTicket,
                    MAX(theorder.OrderedAt) AS LastBuy
                FROM customer.Customers AS customer (NOLOCK)
                    LEFT JOIN [order].[Orders] AS theorder (NOLOCK) ON theorder.CustomerId = customer.Id
                    LEFT JOIN payment.Payments AS payment (NOLOCK) ON payment.Id = theorder.PaymentId
                    LEFT JOIN customer.Sellers AS theseller (NOLOCK) ON theseller.Id = theorder.SellerId
                WHERE (@storeCode IS NULL OR customer.StoreCode = @storeCode OR customer.StoreCode IS NULL) AND
                      (@seller IS NULL OR theseller.Id = @seller) AND
                      (@paymentTypeId IS NULL OR payment.PaymentTypeId = @paymentTypeId) AND
                      (@customerName IS NULL OR customer.Name LIKE @customerName) AND
                      (@customerDocument IS NULL OR (customer.Cpf LIKE @customerDocument) OR (customer.Rg LIKE @customerDocument)) AND
                      ((@onlyValidPhone = 0) OR (customer.Phone IS NOT NULL AND customer.Phone != '')) AND
                      (@buyPeriodInitialDate IS NULL OR theorder.OrderedAt BETWEEN @buyPeriodInitialDate AND @buyPeriodFinalDate) AND
                      (@notBuyPeriodInitialDate IS NULL OR theorder.OrderedAt NOT BETWEEN @notBuyPeriodInitialDate AND @notBuyPeriodFinalDate) AND
                      ((@ageMax = 90 AND @ageMin = 0) OR customer.BirthDay BETWEEN @birthDayMin AND @birthDayMax)
                GROUP BY 
                    customer.Id,
                    customer.Name,
                    customer.BirthDay,
                    customer.Phone,
                    customer.StoreCode,
                    customer.Cpf,
                    customer.Rg,
                    theseller.Name
                HAVING (@averageTicketStart = 0 OR AVG(theorder.Value) >= @averageTicketStart) AND
                       (@averageTicketEnd = 0   OR AVG(theorder.Value) <= @AverageTicketEnd)
                )

                SELECT *
                FROM FilteredCustomers
                ORDER BY LastBuy DESC
                OFFSET (@pageIndex - 1) * (@pageSize) ROWS
                FETCH NEXT @pageSize ROWS ONLY;

            ";

            var totalCount = 0;
            List<CustomerResumeDto> customers = new List<CustomerResumeDto>();

            var conn = this._context.Database.GetDbConnection();

            using (var multi = await conn.QueryMultipleAsync(GetCustomersByFilterSql, parameters))
            {
                totalCount = multi.Read<int>().AsList()[0];
                customers = multi.Read<CustomerResumeDto>().AsList();

                foreach (CustomerResumeDto customer in customers)
                {
                    customer.CPF = StringExtensions.FormatCPFCNPJ(customer.CPF);
                    customer.IsMobilePhone = StringExtensions.IsMobilePhoneNumber(customer.Phone);
                    customer.Subsidiary = customer.Subsidiary == null ? null : this._identity.CurrentStoreName;
                }
            }

            var customersPaged = new StaticPagedList<CustomerResumeDto>(customers, request.PageIndex, request.PageSize, totalCount);

            return new GetCustomersByFilterResponse
            {
                Customers = customersPaged
            };
        }
    }
}