namespace Infra.ExternalServices.Reshop.Dtos
{
    public class ReshopCustomer
    {
        public string Resultado { get; set; }
        public string Mensagem { get; set; }
        public string ClienteCadastrado { get; set; }
        public string Id { get; set; }
        public string CustomerInfoId { get; set; }
        public string Documento { get; set; }
        public string NumeroFidelidade { get; set; }
        public string DataCriacao { get; set; }
        public string DataUltimaModificacao { get; set; }
        public string Nome { get; set; }
        public string Documento2 { get; set; }
        public string DataNascimento { get; set; }
        public string Sexo { get; set; }
        public string NextExpirationDate { get; set; }
        public string EstadoCivil { get; set; }
        public string Profissao { get; set; }
        public string SaldoAtual { get; set; }
        public string SaldoEmProcessamento { get; set; }
        public string SaldoMonetario { get; set; }
        public string Fidelizado { get; set; }
        public string IdPrograma { get; set; }
        public string NomeProgramaFidelidade { get; set; }
        public string OcultaMensagemFidelidade { get; set; }
        public string CategoriaFidelidade { get; set; }
        public string CategoriaCliente { get; set; }
        public string ValorMinimoCompra { get; set; }
        public string PontuacaoPorVenda { get; set; }
        public string DataAdesao { get; set; }
        public string ResgateDesconto { get; set; }
        public string ResgateProduto { get; set; }
        public string ResgateCreditoDesconto { get; set; }
        public string ValorPonto { get; set; }
        public string SaldoMinimoPontos { get; set; }
        public string PermiteCredito { get; set; }
        public string PermiteResgate { get; set; }
        public string SenhaNoCredito { get; set; }
        public string SenhaNoResgate { get; set; }
        public string ValorLimiteDesconto { get; set; }
        public string CadastroAlterado { get; set; }
        public string PossuiApp { get; set; }
        public string SmsToken { get; set; }
        public string SugereResgateFidelidade { get; set; }
        public string SaldoCredito { get; set; }
        public string PermiteResgateValeCredito { get; set; }
        public string Nsu { get; set; }
        public Telefones Telefones { get; set; }
        public Endereco Enderecos { get; set; }   
        public Rede Redes { get; set; }
        public CampanhaOpcional CampanhasOpcionais { get; set; }
        public CamposCadastro CamposCadastro { get; set; }
        public Finalidade Finalidades { get; set; }  
        public CampoCustomizado CamposCustomizados { get; set; }
    }

    public class Telefones
    {
        public string Id { get; set; }
        public string TipoTelefone { get; set; }
        public string Ddi { get; set; }
        public string Ddd { get; set; }
        public string Telefone { get; set; }
        public string Ramal { get; set; }
    }

    public class Endereco
    {
        public string Id { get; set; }
        public string Cep { get; set; }
        public string Logradouro { get; set; }
        public string Número { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Uf { get; set; }
        public string Pais { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }

    public class Rede
    {
        public string Id { get; set; }
        public string TipoRede { get; set; }
        public string Endereço { get; set; }
    }

    public class CampanhaOpcional
    {
        public string IdCampanha { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Vencimento { get; set; }
        public string Promocode { get; set; }
        public string Exclusiva { get; set; }
        public string Restrita { get; set; }
        public string ValorDesconto { get; set; }
        public string PercentualDesconto { get; set; }
        public string InibeResgate { get; set; }
        public string SolicitaTokenApp { get; set; }
        public ProdutoRegra ProdutosRegra { get; set; }
    }

    public class ProdutoRegra
    {
        public string CodigoProduto { get; set; }
        public string ListaProdutos { get; set; }
        public string Qtde { get; set; }
    }

    public class CamposCadastro
    {
        public string IdCampanha { get; set; }
        public string IdCampo { get; set; }
        public string Titulo { get; set; }
        public string TipoDado { get; set; }
        public string TamMinimo { get; set; }
        public string TamMaximo { get; set; }
    }

    public class Finalidade
    {
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public string Permitido { get; set; }
    }

    public class CampoCustomizado
    {
        public string Campo { get; set; }
        public string Produto { get; set; }
        public string Valor { get; set; }
    }
}
