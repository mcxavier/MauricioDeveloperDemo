using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
//using Oracle.ManagedDataAccess.Client;
//using Oracle.ManagedDataAccess.Types;
using CPS_AnaliseQuimica.Utils;
using CPS_AnaliseQuimica.DTO;
using CPS_Service_Lib.LibUtils;
using Constantes = CPS_AnaliseQuimica.Utils.Constantes;

namespace CPS_AnaliseQuimica.DataBase
{
    // ********************************************************************************************************* //
    //                                    B I O M E T R I C      I T                     (www.biometric-it.com)  //
    // --------------------------------------------------------------------------------------------------------- //
    //  @Autor: MAURICIO XAVIER                                                                                  //
    //  @Data:  19/05/2020                                                                                       //
    //  @Descricao:                                                                                              //
    // ********************************************************************************************************* //

    public class OracleTools
    {
        private int eventId { get; set; }
        public List<string> logEmail { get; set; }

        private EventLog CPSeventLog { get; set; }
        private OracleConnection con { get; set; }





        // ********************************************************************************************************* //
        //                                    B I O M E T R I C      I T                     (www.biometric-it.com)  //
        // --------------------------------------------------------------------------------------------------------- //
        //  @Autor: MAURICIO XAVIER                                                                                  //
        //  @Data:  19/05/2020                                                                                       //
        //  @Descricao:                                                                                              //
        // ********************************************************************************************************* //
        public OracleTools(EventLog _CPSeventLog, List<string> _logEmail)
        {
            this.CPSeventLog = _CPSeventLog;
            this.logEmail = _logEmail;
            this.eventId = 0;
        }



        // ********************************************************************************************************* //
        //                                    B I O M E T R I C      I T                     (www.biometric-it.com)  //
        // --------------------------------------------------------------------------------------------------------- //
        //  @Autor: MAURICIO XAVIER                                                                                  //
        //  @Data:  19/05/2020                                                                                       //
        //  @Descricao:                                                                                              //
        // ********************************************************************************************************* //
        public void CreateConnection()
        {
            string conString = "User Id=" + Constantes.MESUserID + "; Password=" + Constantes.MESPassword + "; Data Source=" + Constantes.MESDataSource + "; Pooling =false;";


            if (this.con == null)
            {
                con = new OracleConnection();
            }

            if ((con.State == ConnectionState.Closed))
            {
                con = new OracleConnection();
                con.ConnectionString = conString;
                con.Open();
            }

        }





        // ********************************************************************************************************* //
        //                                    B I O M E T R I C      I T                     (www.biometric-it.com)  //
        // --------------------------------------------------------------------------------------------------------- //
        //  @Autor: MAURICIO XAVIER                                                                                  //
        //  @Data:  19/05/2020                                                                                       //
        //  @Descricao:                                                                                              //
        // ********************************************************************************************************* //
        public void GetCheckAnalise()
        {
            this.CreateConnection();

            using (this.con)
            {

                using (OracleCommand command = new OracleCommand(SqlBase.SelectCheckAnalise, this.con))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        RestTools rest = new RestTools();

                        try
                        {
                            AnaliseQuimicaCheckAnaliseDTO checkanalise = new AnaliseQuimicaCheckAnaliseDTO();
                            checkanalise.diasRetroativos = Constantes.DiasRetroativosAquisicao;

                            while (reader.Read())
                            {
                                checkanalise.listaAnaliseCheckAnalise.Add(new AnaliseQuimicaCheckAnalise(
                                  CPS_Service_Lib.LibUtils.Constantes.CodCentro, // String codCentro, 
                                    ParseTypes.GetInt(reader, 0), // Nullable<long> corrida, 
                                    ParseTypes.GetString(reader, 1), // String particao, 
                                    ParseTypes.GetInt(reader, 2), // Nullable<long> lote,
                                    ParseTypes.GetString(reader, 3), // String pex, 
                                    ParseTypes.GetDateTime(reader, 4), // Nullable < DateTime > dataLingotamento, 
                                    ParseTypes.GetDateTime(reader, 5), // Nullable < DateTime > dataApontamentoProducao,
                                    ParseTypes.GetDateTime(reader, 6), // Nullable < DateTime > dataApontamentoQualidade, 
                                    ParseTypes.GetString(reader, 7), // String acoLingotado, 
                                    ParseTypes.GetString(reader, 8), // String acoProduzido,
                                    ParseTypes.GetString(reader, 9), // String acoLiberado, 
                                    ParseTypes.GetString(reader, 10), // String desoxProduzido, 
                                    ParseTypes.GetString(reader, 11), // String desoxLiberado, 
                                    ParseTypes.GetString(reader, 12), // String materialProduzido,
                                    ParseTypes.GetString(reader, 13), // String materialLiberado, 
                                    ParseTypes.GetString(reader, 14), // String clienteProduzido, 
                                    ParseTypes.GetString(reader, 15), // String clienteLiberado, 
                                    ParseTypes.GetString(reader, 16), // String segmentoProduzido,
                                    ParseTypes.GetString(reader, 17), // String segmentoLiberado, 
                                    ParseTypes.GetString(reader, 18), // String bitola, 
                                    ParseTypes.GetInt(reader, 19), // Nullable<long> numeroRolos, 
                                    ParseTypes.GetInt(reader, 20), // Nullable<long> pesoCorrida,
                                    ParseTypes.GetString(reader, 21), // String descricaoCaracteristica, 
                                    ParseTypes.GetString(reader, 22), // String apelidoCaracteristica, 
                                    ParseTypes.GetString(reader, 23), // String valor, 
                                    ParseTypes.GetString(reader, 24), // String responsavelEnsaio,
                                    ParseTypes.GetDateTime(reader, 25), // Nullable < DateTime > dataFechamentoEnsaio, 
                                    ParseTypes.GetString(reader, 26), // String responsavelAtualizacaoMes,
                                    ParseTypes.GetDateTime(reader, 27), // Nullable < DateTime > dtHrAtualizacaoMes, 
                                    ParseTypes.GetString(reader, 28), // String materialComponente, 
                                    ParseTypes.GetString(reader, 29), // String turno, 
                                    ParseTypes.GetString(reader, 30), // String turma,
                                    ParseTypes.GetString(reader, 31), // String codigoLaminador,
                                    ParseTypes.GetString(reader, 32), // String PROCESSO, 
                                    ParseTypes.GetString(reader, 33), // String APLICACAO, 
                                    ParseTypes.GetString(reader, 34), // String ACO_LING, 
                                    ParseTypes.GetString(reader, 35), // String TEOR_P, 
                                    ParseTypes.GetString(reader, 36), // String TEOR_C, 
                                    ParseTypes.GetString(reader, 37)  // String CONSUM_LIGAS   
                                    ));
                            }

                            string sRetorno = rest.AnaliseQuimicaCheckAnalise(checkanalise);

                            CPSeventLog.WriteEntry("CheckAnalise: " + sRetorno, EventLogEntryType.Information, eventId++);
                            logEmail.Add("CheckAnalise: " + sRetorno);
                            logEmail.Add("Registros: " + checkanalise.listaAnaliseCheckAnalise.Count.ToString());
                            logEmail.Add("Dias Retroativos: " + Utils.Constantes.DiasRetroativosAquisicao.ToString());

                        }
                        catch (Exception ex)
                        {
                            CPSeventLog.WriteEntry("checkanalise: " + ex.Message, EventLogEntryType.Error, eventId++);
                            logEmail.Add("checkanalise: " + ex.Message);
                        }
                    }


                }
            }

        }





    }
}
