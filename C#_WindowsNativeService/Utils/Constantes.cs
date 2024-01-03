using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPS_AnaliseQuimica.Utils
{
    // ********************************************************************************************************* //
    //                                    B I O M E T R I C      I T                     (www.biometric-it.com)  //
    // --------------------------------------------------------------------------------------------------------- //
    //  @Autor: MAURICIO XAVIER                                                                                  //
    //  @Data:  19/05/2020                                                                                       //
    //  @Descricao:                                                                                              //
    // ********************************************************************************************************* //
    public class Constantes
    {
        public static string EventLogSource = "MES-CPA Análise Química";
        public static string EventLogName { get; internal set; }

        //public static string Servidor = "http://localhost:9080/MES_PROCESSO_WEB"; // local Server
        //public static string Servidor = "http://bhz-tst-appmq04.belgo.com.br/MES_PROCESSO_WEB";  // QA Server
        public static string Servidor = "http://mes.monlevade.belgo.com.br/MES_PROCESSO_WEB";  // PRD Server


        public static int MaxResponseContentBufferSize = 147000000; //max = 2Gb = 147.483.648 bytes
        public static int LimteTimeOutMinutos = 10;   // (GET) limite do timeout, em minutos, nas interações com o BackEnd.
        public static int LimteTimeOutMinutosPost = 30;   // (POST) limite do timeout, em minutos, nas interações com o BackEnd.

        public static string RaizServico = "/cps/aquisicaoDados";
        public static int DiasRetroativosAquisicao = 3;

        // database SAA
        public static string DataSource = "10.7.100.226\\GAMAPROD";
        public static string UserID = "saarel";
        public static string Password = "";
        public static string InitialCatalog = "ACM";

        // database AFA
        public static string DataSourceAFA = "10.7.100.226\\GAMAPROD";
        public static string UserIDAFA = "afm";
        public static string PasswordAFA = "altoforno";
        public static string InitialCatalogAFA = "AFM";

        // database MES
        public static string MESDataSource = "csbm4123:1521/MESMDE";
        public static string MESUserID = "TRACE";
        public static string MESPassword = "SIBARP";



    }
}
