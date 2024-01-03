namespace Infra.ExternalServices.Stock
{
    public static class StockIntegrationSql
    {
        public const string GetStockFromUx = @"
            SELECT
                StockKeepingUnitId = PR.ID_SKU, 
                StockKeepingUnitCode = PR.COD_SKU,
                Units = SLD.QTDE_ESTOQUE 
            FROM 
                LX_STK.STK_SALDO_SKU SLD
                JOIN LX_PRD.PRD_SKU_PRODUTO PR ON PR.ID_SKU = SLD.ID_SKU
                JOIN LX_PRD.PRD_ARTIGO ART ON ART.ID_ARTIGO = PR.ID_ARTIGO
                JOIN LX_STK.STK_DEPOSITO DEP ON DEP.ID_STK_DEPOSITO = SLD.ID_STK_DEPOSITO
                JOIN LX_LJV.LJV_LOJA LJ ON LJ.ID_LOJA = DEP.ID_LOJA
                JOIN LX_TBC.TBC_FILIAL FI ON FI.ID_FILIAL_PFJ = LJ.ID_FILIAL_PFJ
            WHERE
                (@shopCode  IS NULL OR LJ.COD_LOJA = @shopCode) AND
                (@skusCodes IS NULL OR COD_SKU IN @skusCodes)
        ";
    }
}