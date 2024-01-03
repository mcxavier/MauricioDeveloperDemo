using Core.Domains.Catalogs.Filters;
using Core.SeedWork;
using Dapper;
using System.Linq;

namespace Infra.Repositories.Sql
{
    public static class ProductSqls
    {
        public static string ProductsFilteredSql(OrderByTypes orderby, OrderDirectionTypes direction)
        {
            // @TODO Filter by categories
            //(@containsCategories IS NULL OR PC.CategoryId IN @categories) AND
            var query = $@"
                SELECT 
                    PV.Id,
                    PV.Name,
                    PV.ImageUrl,
                    PV.CostPrice,
                    PV.ListPrice,
                    PV.CompleteName,
                    PV.ProductId,
                    PV.StockKeepingUnit,
                    PV.BasePrice,
                    PV.IsActive,
                    ISNULL(E.Units, 0) AS Units 
                INTO #ProductVariations
                FROM product.Variations PV WITH (NOLOCK)
                    LEFT JOIN product.Stock E WITH (NOLOCK) on REPLACE(PV.StockKeepingUnit, '-', '') = E.StockKeepingUnit 
                        AND E.StoreCode = @storeCode
                WHERE PV.IsActive = 1
                ORDER BY Units DESC
                
                SELECT
                    P.Id,
                    COUNT(*) OVER() AS Count,
                    SUM(PV.Units) AS Units,
                    P.Name As VariationName,
                    PV.BasePrice AS BasePrice
                INTO #Ids
                FROM product.Products AS P WITH (NOLOCK)
                    INNER JOIN #ProductVariations AS PV WITH (NOLOCK) ON PV.ProductId = P.Id
                    LEFT JOIN product.Specifications AS S WITH (NOLOCK)  ON S.ProductVariationId = PV.Id AND S.TypeId IN (1, 2, 4)
                    LEFT JOIN product.ProductCategory AS PC WITH (NOLOCK) ON PC.ProductId = P.Id
                WHERE (@containsFilters IS NULL OR (S.TypeId IN (1, 2, 4) AND S.Value IN @filters)) AND
                      (@priceFrom IS NULL OR PV.BasePrice >= @priceFrom) AND 
                      (@priceTo IS NULL OR PV.BasePrice <= @priceTo) AND
                      (@onlyOnStock = 0 AND ISNULL(PV.Units, 0) >= 0) AND
                      
                      (@containsCategories IS NULL OR (PV.Units > 0 AND Units > 0)) AND
                      (@containsFilters IS NULL OR (PV.Units > 0 AND Units > 0)) AND
                      
                      (@term IS NULL OR ((LOWER(P.Name) LIKE LOWER(@term)) OR (LOWER(PV.StockKeepingUnit) LIKE LOWER(@term))))                      
                GROUP BY P.Id, P.Name, PV.BasePrice
                ORDER BY
                    CASE WHEN SUM(PV.Units) > 0 THEN 1 ELSE 0 END DESC, 
                    {MakeOrderBySql(orderby, direction)}
                
                OFFSET (@pageIndex - 1) * (@pageSize) ROWS
                FETCH NEXT @pageSize ROWS ONLY
                
                
                SELECT
                    P.Id,
                    P.BrandName,
                    P.Ncm,
                    P.Name,
                    P.CommonReference,
                    P.Description,
                    P.Reference,

                    PV.Id,
                    PV.Name AS VariationName,
                    PV.Name,
                    PV.CompleteName,
                    PV.ProductId,
                    PV.ImageUrl,
                    PV.BasePrice,
                    PV.CostPrice,
                    PV.ListPrice,
                    PV.StockKeepingUnit,
                    PV.Units,

                    I.Id,
                    I.Name,
                    I.ProductVariationId,
                    I.UrlImage,
                    I.IsPrincipal,

                    S.Id,
                    S.ProductVariationId,
                    S.Name,
                    S.Description,
                    S.Value,
                    S.IsFilter,
                    S.TypeId,
                    
                    T.Count
                   
                FROM product.Products AS P WITH (NOLOCK)
                    INNER JOIN #Ids AS T ON T.Id = P.Id
                    INNER JOIN #ProductVariations AS PV ON PV.ProductId = T.Id
                    INNER JOIN product.Images AS I (NOLOCK) ON I.ProductVariationId = PV.Id
                    INNER JOIN product.Specifications AS S (NOLOCK) ON s.ProductVariationId = PV.Id
                WHERE S.TypeId IN (1, 2, 4)
                ORDER BY {MakeOrderBySql(orderby, direction)}
                
                DROP TABLE #Ids
                DROP TABLE #ProductVariations
            ";

            return query;
        }

        public static string OptimizedProductsFilteredSql(OrderByTypes orderby, OrderDirectionTypes direction, ProductFilter filters)
        {
            var stockCondition = filters.OnlyOnStock ? $@" AND T.Units > 0 AND V.BasePrice is not null " : "";

            var specificationsConditions = filters.Filters?.Count > 0 ? $@"AND (S.TypeId IN (1, 2, 4) AND S.Value IN @filters) " : "";

            var priceRange = filters.PriceRange.To.HasValue ? $@" AND V.BasePrice > @priceFrom AND V.BasePrice < @priceTo " : "";

            var description = string.IsNullOrWhiteSpace(filters.Term) ? "" : $@" AND V.name like @term ";

            var categories = filters.CategoriesIds?.Count > 0 ? $@" AND C.CategoryId in @categoriesIds " : "";

            var order = $"ORDER BY {(orderby == OrderByTypes.Relevance ? "Estoque DESC, Name ASC" : orderby == OrderByTypes.Alphabetic ? $"Name {direction}" : $"Price {direction}, Name ASC")}";

            var query = $@"
                SELECT P.ProductId, Name, COUNT(1) AS Variations, MAX(BasePrice) as Price, Max(Units) as Estoque
                FROM (
	                SELECT 
                        V.ProductId AS ProductId, V.BasePrice, PD.Name, PD.ModifiedAt AS ModifiedAt, S.Id as VariationId, T.Units
                    FROM [product].[Products] AS PD WITH (NOLOCK)					
						 inner join [product].[Variations] as V WITH (NOLOCK) on V.ProductId = PD.Id
                         left join [product].[Stock] as T With (NOLOCK) on T.StockKeepingUnit = V.StockKeepingUnit AND T.StoreCode = @storeCode
                         left join [product].[Specifications] as S WITH (NOLOCK) on S.ProductVariationId = V.Id
                         left join [product].[ProductCategory] as C WITH (NOLOCK) on C.ProductId = PD.Id                                                  
		            WHERE V.IsActive = 1 AND PD.IsActive = 1
		            {stockCondition} {specificationsConditions} {priceRange} {description} {categories} 
                ) AS P
                GROUP BY P.ProductId, Name                
                {order}
            ";

            return query;
        }

        private static string MakeOrderBySql(OrderByTypes orderby, OrderDirectionTypes direction)
        {
            var by = (orderby) switch
            {
                OrderByTypes.Price => "BasePrice",
                OrderByTypes.Alphabetic => "VariationName",
                _ => "PV.Id"
            };

            var dir = (direction) switch
            {
                OrderDirectionTypes.Asc => "ASC",
                OrderDirectionTypes.Desc => "DESC",
                _ => "DESC"
            };

            return $"{by} {dir}";
        }

        public static string SearchProductsSql()
        {
            const string query = @"
                SELECT
                    P.Id,
                    P.Name,
                    P.BrandName,
                    P.Reference,
                    P.Description,
                    MIN(I.UrlImage) as ImageUrl,
                    MIN(V.BasePrice) as BasePrice,
                    COUNT(*) OVER() AS Count
                FROM product.Products AS P (NOLOCK)
                INNER JOIN product.Variations AS V (NOLOCK) ON P.Id = V.ProductId
                LEFT  JOIN product.Images AS I (NOLOCK) ON I.ProductVariationId = V.Id
                WHERE V.IsActive = 1 AND
                      (@term IS NULL OR LOWER(P.Name) LIKE LOWER(@term))
                GROUP BY P.Name, P.Description, P.Id, P.Reference, P.BrandName
                ORDER BY (SELECT null)
                OFFSET (@pageIndex - 1) * (@pageSize) ROWS
                FETCH NEXT @pageSize ROWS ONLY
            ";

            return query;
        }

        public static string OptimizedSearchProductsSql(bool hasNext = false)
        {
            if (hasNext)
            {
                const string query = @"
                    SELECT
                        P.Id,
                        P.Name,
                        P.BrandName,
                        P.Reference,
                        P.Description,
                        MIN(I.UrlImage) as ImageUrl,
                        MIN(V.BasePrice) as BasePrice
                    FROM product.Products AS P (NOLOCK)
                    INNER JOIN product.Variations AS V (NOLOCK) ON P.Id = V.ProductId
                    LEFT  JOIN product.Images AS I (NOLOCK) ON I.ProductVariationId = V.Id
                    LEFT  JOIN product.ProductCategory AS C (NOLOCK) ON P.Id = C.ProductId
                    WHERE V.IsActive = 1 AND
                          (@term IS NULL OR LOWER(P.Name) LIKE LOWER(@term)) AND
                          ((@categoryId IS NULL) OR (@categoryId IS NOT NULL AND C.CategoryId = @categoryId) OR (@categoriesIds IS NOT NULL AND C.CategoryId IN @categoriesIds))
                    GROUP BY P.Name, P.Description, P.Id, P.Reference, P.BrandName
                    ORDER BY (SELECT null)
                    OFFSET (@pageIndex - 1) * (@pageSize) ROWS
                    FETCH NEXT (@pageSize+1) ROWS ONLY
                ";

                return query;
            }
            else
            {
                const string query = @"
                    SELECT
                        P.Id,
                        P.Name,
                        P.BrandName,
                        P.Reference,
                        P.Description,
                        MIN(I.UrlImage) as ImageUrl,
                        MIN(V.BasePrice) as BasePrice
                    FROM product.Products AS P (NOLOCK)
                    INNER JOIN product.Variations AS V (NOLOCK) ON P.Id = V.ProductId
                    LEFT  JOIN product.Images AS I (NOLOCK) ON I.ProductVariationId = V.Id
                    LEFT  JOIN product.ProductCategory AS C (NOLOCK) ON P.Id = C.ProductId
                    WHERE V.IsActive = 1 AND
                          (@term IS NULL OR LOWER(P.Name) LIKE LOWER(@term)) AND
                          ((@categoryId IS NULL) OR (@categoryId IS NOT NULL AND C.CategoryId = @categoryId) OR (@categoriesIds IS NOT NULL AND C.CategoryId IN @categoriesIds))
                    GROUP BY P.Name, P.Description, P.Id, P.Reference, P.BrandName
                    ORDER BY (SELECT null)
                    OFFSET (@pageIndex - 1) * (@pageSize) ROWS
                    FETCH NEXT @pageSize ROWS ONLY
                ";

                return query;
            }

        }

        public static string OptimizedSearchCountProductsSql()
        {
            const string query = @"
                SELECT COUNT(*) AS Count FROM (
                    SELECT P.Name
                    FROM product.Products AS P (NOLOCK)
                    INNER JOIN product.Variations AS V (NOLOCK) ON P.Id = V.ProductId
                    LEFT  JOIN product.Images AS I (NOLOCK) ON I.ProductVariationId = V.Id
                    LEFT  JOIN product.ProductCategory AS C (NOLOCK) ON P.Id = C.ProductId
                    WHERE V.IsActive = 1 AND 
                        (@term IS NULL OR LOWER(P.Name) LIKE LOWER(@term)) AND
                        ((@categoryId IS NULL) OR (@categoryId IS NOT NULL AND C.CategoryId = @categoryId) OR (@categoriesIds IS NOT NULL AND C.CategoryId IN @categoriesIds))
                    GROUP BY P.Name, P.Description, P.Id, P.Reference, P.BrandName
                ) AS P
            ";

            return query;
        }
    }

}