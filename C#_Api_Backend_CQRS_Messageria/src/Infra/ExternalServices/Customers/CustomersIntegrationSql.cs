namespace Infra.ExternalServices.Customers
{

    public static class CustomersIntegrationSql
    {
        public const string GetCustomersFromUx = @"
            SELECT
                [customer].PF_NOME AS [FirstName],
                [customer].PF_SOBRENOME AS [LastName],
                [customer].ENDERECO_ELETRONICO AS [Email],
                [customer].CNPJ_CPF as [Document],
                CONVERT(DATE, [customer].DATA_NASCIMENTO) AS [Birthday],
                [customer].DDD_CELULAR + [customer].CELULAR AS [ContactNumber],
                [customer].DDD + [customer].TELEFONE AS [Telephone],
                [customer].RG_IE AS [Rg],
                [store].COD_LOJA AS [StoreCode]
            FROM LX_CRM.CRM_PFJ AS [customer]
            LEFT JOIN lx_ljv.LJV_LOJA AS [store]
                   ON [customer].ID_LOJA = [store].ID_LOJA
            WHERE [store].COD_LOJA = @storeCode
        ";
        
        public const string GetOrderHistoryFromUx = @"
            SELECT
               [store].COD_LOJA AS 'StoreCode',
               [sku].COD_SKU as 'StockKeepingUnit',
               [item].PRECO_BRUTO_ITEM as 'GrossValue',
               [item].DESCONTO_ITEM AS 'Discount',
               [item].VALOR_LIQUIDO_PAGO as 'NetValue',
               [item].QTDE_ITEM as 'Units',
               CONVERT(DATE, [atendimento].DATA_ATENDIMENTO) as 'OrderDate',
               [cli].CNPJ_CPF as 'customerDocument',
               [vendedor].NOME_VENDEDOR as 'SellerName'
            FROM lx_ljv.ljv_atendimento AS [atendimento]
               LEFT JOIN lx_ljv.LJV_LOJA AS [store]
                   ON [store].ID_LOJA = [atendimento].ID_LOJA
               LEFT JOIN LX_CRM.CRM_PFJ AS [cli]
                   ON cli.ID_CRM_PFJ = atendimento.ID_CRM_PFJ
               LEFT JOIN lx_ljv.LJV_ATENDIMENTO_ITEM AS [item]
                   ON atendimento.ID_ATENDIMENTO = item.ID_ATENDIMENTO
               LEFT  JOIN LX_PRD.PRD_SKU_PRODUTO AS [sku]
                   ON item.ID_SKU = SKU.ID_SKU 
               LEFT JOIN lx_ljv.LJV_ATENDIMENTO_VENDEDOR AS [atendeVendedor]
                   ON item.ID_ATENDIMENTO = atendeVendedor.ID_ATENDIMENTO
               LEFT JOIN lx_ljv.LJV_VENDEDOR AS [vendedor]
                   ON vendedor.ID_VENDEDOR = atendeVendedor.ID_VENDEDOR
            WHERE [atendimento].ID_CRM_PFJ IS NOT NULL AND 
                  [store].COD_LOJA = @storeCode 
            GROUP BY 
	              [store].COD_LOJA,
	              [sku].COD_SKU,
	              [item].PRECO_BRUTO_ITEM,
	              [item].DESCONTO_ITEM,
	              [item].VALOR_LIQUIDO_PAGO,
	              [item].QTDE_ITEM,
	              [atendimento].DATA_ATENDIMENTO,
	              [cli].CNPJ_CPF,
	              [vendedor].NOME_VENDEDOR
        ";
    }

}