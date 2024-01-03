namespace Infra.ExternalServices.Price
{
    public static class PriceIntegrationSql
    {
        public const string GetPricesFromUx = @"
            SELECT 
                produto.COD_SKU AS StockKeepingUnitCode,
                preco.PRECO AS ListPrice,
                preco.PRECO_ORIGINAL AS BasePrice,
                MAX(preco.DATA_INI_VIGENCIA) AS Date
            FROM lx_prd.prd_sku_preco AS preco
                JOIN LX_LJV.LJV_LOJA AS loja ON loja.ID_LOJA = preco.ID_LOJA
                JOIN LX_PRD.PRD_SKU_PRODUTO AS produto ON produto.ID_SKU = preco.ID_SKU
            WHERE produto.COD_SKU IN @skusCodes AND 
                  preco.ID_TAB_PRECO = 1 AND
                  loja.COD_LOJA = @shopCode 
            GROUP BY produto.COD_SKU, preco.PRECO, preco.PRECO_ORIGINAL
            ORDER BY Date
        ";
    }
}