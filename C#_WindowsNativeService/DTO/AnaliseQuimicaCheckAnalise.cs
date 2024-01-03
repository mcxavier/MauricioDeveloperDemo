using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CPS_AnaliseQuimica.DTO
{
    // ********************************************************************************************************* //
    //                                    B I O M E T R I C      I T                     (www.biometric-it.com)  //
    // --------------------------------------------------------------------------------------------------------- //
    //  @Autor: MAURICIO XAVIER                                                                                  //
    //  @Data:  19/05/2020                                                                                       //
    //  @Descricao:                                                                                              //
    // ********************************************************************************************************* //
    public class AnaliseQuimicaCheckAnalise
    {
        public long id { get; set; }
        public String codCentro { get; set; }
        public Nullable<long> corrida { get; set; }
        public String particao { get; set; }
        public Nullable<long> lote { get; set; }
        public String pex { get; set; }
        public Nullable<DateTime> dataLingotamento { get; set; }
        public Nullable<DateTime> dataApontamentoProducao { get; set; }
        public Nullable<DateTime> dataApontamentoQualidade { get; set; }
        public String acoLingotado { get; set; }
        public String acoProduzido { get; set; }
        public String acoLiberado { get; set; }
        public String desoxProduzido { get; set; }
        public String desoxLiberado { get; set; }
        public String materialProduzido { get; set; }
        public String materialLiberado { get; set; }
        public String clienteProduzido { get; set; }
        public String clienteLiberado { get; set; }
        public String segmentoProduzido { get; set; }
        public String segmentoLiberado { get; set; }
        public String bitola { get; set; }
        public Nullable<long> numeroRolos { get; set; }
        public Nullable<long> pesoCorrida { get; set; }
        public String descricaoCaracteristica { get; set; }
        public String apelidoCaracteristica { get; set; }
        public String valor { get; set; }
        public String responsavelEnsaio { get; set; }
        public Nullable<DateTime> dataFechamentoEnsaio { get; set; }
        public String responsavelAtualizacaoMes { get; set; }
        public Nullable<DateTime> dtHrAtualizacaoMes { get; set; }
        public String materialComponente { get; set; }
        public String turno { get; set; }
        public String turma { get; set; }
        public String codigoLaminador { get; set; }
        public String aplicacao { get; set; }
        public String processo { get; set; }
        public String acoLing { get; set; }
        public String teorP { get; set; }
        public String teorC { get;set; }
        public String consumoLigas { get; set; }




        public AnaliseQuimicaCheckAnalise(String codCentro, Nullable<long> corrida, String particao, Nullable<long> lote,
                  String pex, Nullable<DateTime> dataLingotamento, Nullable<DateTime> dataApontamentoProducao, 
                  Nullable<DateTime> dataApontamentoQualidade, String acoLingotado, String acoProduzido,
                  String acoLiberado, String desoxProduzido, String desoxLiberado, String materialProduzido,
                  String materialLiberado, String clienteProduzido, String clienteLiberado, String segmentoProduzido,
                  String segmentoLiberado, String bitola, Nullable<long> numeroRolos, Nullable<long> pesoCorrida,
                  String descricaoCaracteristica, String apelidoCaracteristica, String valor, String responsavelEnsaio,
                  Nullable<DateTime> dataFechamentoEnsaio, String responsavelAtualizacaoMes, 
                  Nullable<DateTime> dtHrAtualizacaoMes, String materialComponente, String turno, String turma,
                  String codigoLaminador, String aplicacao, String processo, String acoLing, String teorP,
                  String teorC, String consumoLigas)
        {
            this.id = 0;
            this.codCentro = codCentro;
            this.corrida = corrida;
            this.particao = particao;
            this.lote = lote;
            this.pex = pex;
            this.dataLingotamento = dataLingotamento;
            this.dataApontamentoProducao = dataApontamentoProducao;
            this.dataApontamentoQualidade = dataApontamentoQualidade;
            this.acoLingotado = acoLingotado;
            this.acoProduzido = acoProduzido;
            this.acoLiberado = acoLiberado;
            this.desoxProduzido = desoxProduzido;
            this.desoxLiberado = desoxLiberado;
            this.materialProduzido = materialProduzido;
            this.materialLiberado = materialLiberado;
            this.clienteProduzido = clienteProduzido;
            this.clienteLiberado = clienteLiberado;
            this.segmentoProduzido = segmentoProduzido;
            this.segmentoLiberado = segmentoLiberado;
            this.bitola = bitola;
            this.numeroRolos = numeroRolos;
            this.pesoCorrida = pesoCorrida;
            this.descricaoCaracteristica = descricaoCaracteristica;
            this.apelidoCaracteristica = apelidoCaracteristica;
            this.valor = valor;
            this.responsavelEnsaio = responsavelEnsaio;
            this.dataFechamentoEnsaio = dataFechamentoEnsaio;
            this.responsavelAtualizacaoMes = responsavelAtualizacaoMes;
            this.dtHrAtualizacaoMes = dtHrAtualizacaoMes;
            this.materialComponente = materialComponente;
            this.turno = turno;
            this.turma = turma;
            this.codigoLaminador = codigoLaminador;
            this.aplicacao = aplicacao;
            this.processo = processo;
            this.acoLing = acoLing;
            this.teorP = teorP;
            this.teorC = teorC;
            this.consumoLigas = consumoLigas;
        }



    }
}