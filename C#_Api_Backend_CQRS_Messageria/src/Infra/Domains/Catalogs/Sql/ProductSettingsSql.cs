namespace Infra.DomainInfra.Catalogs.Sql
{
    public static class SpecificationsSql
    {
        public const string GetSpecifications = @"
            SELECT TOP 30
                typeId,
                description,
                [Value] as [value]
            FROM
                product.Specifications S WITH (NOLOCK)
	            INNER JOIN product.Variations V WITH (NOLOCK) ON V.Id = S.ProductVariationId
            WHERE
                TypeId = @typeId
                AND V.IsActive = 1
            GROUP BY
                TypeId,
                Description,
                [Value]
            ORDER BY
                COUNT(*) DESC
        ";

        public static string GetCategories = @"
            SELECT 
	            C.Id [id],
	            C.[Name] [value]
            FROM
	            product.Categories C WITH (NOLOCK)
	            INNER JOIN product.ProductCategory PC  WITH (NOLOCK) on C.Id = PC.CategoryId 
	            INNER JOIN product.Products P WITH (NOLOCK) on P.Id = PC.ProductId
            WHERE
	            p.IsActive = 1
            GROUP BY
	            C.Id,
	            C.[Name]
            ORDER BY
                C.[Name] ASC";
    }
}
