using System;
using System.Text;
using System.Data.SqlClient;
using System.Diagnostics;
using CPS_AnaliseQuimica.DTO;
using CPS_AnaliseQuimica.Utils;
using System.Collections.Generic;
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
    public class SqlServerTools
    {
        private int eventId { get; set; }
        public List<string> logEmail { get; set; }
        private SqlConnectionStringBuilder builder { get; set; }
        private SqlConnection connection { get; set; }
        private SqlConnection connectionAFA { get; set; }
        private EventLog CPSeventLog { get; set; }


        // ********************************************************************************************************* //
        //                                    B I O M E T R I C      I T                     (www.biometric-it.com)  //
        // --------------------------------------------------------------------------------------------------------- //
        //  @Autor: MAURICIO XAVIER                                                                                  //
        //  @Data:  19/05/2020                                                                                       //
        //  @Descricao:                                                                                              //
        // ********************************************************************************************************* //
        public SqlServerTools(EventLog _CPSeventLog, List<string> _logEmail)  
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
        ~SqlServerTools()
        {
            try
            {
                this.connection.Close();
            }
            catch(Exception ex) {
            }

            this.connection = null;
        }


        // ********************************************************************************************************* //
        //                                    B I O M E T R I C      I T                     (www.biometric-it.com)  //
        // --------------------------------------------------------------------------------------------------------- //
        //  @Autor: MAURICIO XAVIER                                                                                  //
        //  @Data:  19/05/2020                                                                                       //
        //  @Descricao:                                                                                              //
        // ********************************************************************************************************* //
        public void GetGusa()
        {
            this.CreateConnection();

            using (this.connection)
            {

                using (SqlCommand command = new SqlCommand(SqlBase.SelectGusa, connection))
                {
                    command.CommandTimeout = 120;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        RestTools rest = new RestTools();

                        try
                        {
                            AnaliseQuimicaGusaDTO gusa = new AnaliseQuimicaGusaDTO();
                            gusa.diasRetroativos = Constantes.DiasRetroativosAquisicao;
                            gusa.codCentro = CPS_Service_Lib.LibUtils.Constantes.CodCentro;

                            while (reader.Read())
                            {
                                gusa.listaAnaliseGusa.Add(new AnaliseQuimicaGusa(
                                    ParseTypes.GetInt(reader, 1), // CORRIDA, 
                                    ParseTypes.GetString(reader, 2), // ACO, 
                                    ParseTypes.GetDecimal(reader, 3), // S, 
                                    ParseTypes.GetDecimal(reader, 4), // C, 
                                    ParseTypes.GetDecimal(reader, 5), // MN, 
                                    ParseTypes.GetDecimal(reader, 6), // P, 
                                    ParseTypes.GetDecimal(reader, 7), // SI, 
                                    ParseTypes.GetDecimal(reader, 8), // TI, 
                                    ParseTypes.GetDecimal(reader, 10), // CR, 
                                    ParseTypes.GetDecimal(reader, 11), // NI,
                                    ParseTypes.GetDecimal(reader, 12), // CU, 
                                    ParseTypes.GetString(reader, 13), // TURMA, 
                                    ParseTypes.GetInt(reader, 14), // TURNO
                                    ParseTypes.GetDateTime(reader, 0), // DATA
                                    CPS_Service_Lib.LibUtils.Constantes.CodCentro,// CODCENTRO
                                    ParseTypes.GetString(reader, 15), // String PROCESSO, 
                                    ParseTypes.GetString(reader, 16), // String APLICACAO, 
                                    ParseTypes.GetString(reader, 17), // String ACO_LING, 
                                    ParseTypes.GetString(reader, 18), // String TEOR_P, 
                                    ParseTypes.GetString(reader, 19), // String TEOR_C, 
                                    ParseTypes.GetString(reader, 20)  // String CONSUM_LIGAS   
                                    ));
                            }

                            string sRetorno = rest.AnaliseQuimicaGusa(gusa);

                            CPSeventLog.WriteEntry("Gusa: " + sRetorno, EventLogEntryType.Information, eventId++);
                            logEmail.Add("Gusa: " + sRetorno);
                            logEmail.Add("Registros: " + gusa.listaAnaliseGusa.Count.ToString());
                            logEmail.Add("Dias Retroativos: " + Utils.Constantes.DiasRetroativosAquisicao.ToString());

                        }
                        catch (Exception ex)
                        {
                            CPSeventLog.WriteEntry("Gusa: " + ex.Message, EventLogEntryType.Error, eventId++);
                            logEmail.Add("Gusa: " + ex.Message);
                        }
                    }
                }
            }

        }





        // ********************************************************************************************************* //
        //                                    B I O M E T R I C      I T                     (www.biometric-it.com)  //
        // --------------------------------------------------------------------------------------------------------- //
        //  @Autor: MAURICIO XAVIER                                                                                  //
        //  @Data:  19/05/2020                                                                                       //
        //  @Descricao:                                                                                              //
        // ********************************************************************************************************* //
        public void GetGusaAFA()
        {
            this.CreateConnectionAFA();

            using (this.connectionAFA)
            {

                using (SqlCommand command = new SqlCommand(SqlBase.SelectGusaAFA, connectionAFA))
                {
                    command.CommandTimeout = 120;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        RestTools rest = new RestTools();
                        AnaliseQuimicaGusaAFA reg;

                        try
                        {
                            AnaliseQuimicaGusaAFADTO gusaAFA = new AnaliseQuimicaGusaAFADTO();
                            gusaAFA.diasRetroativos = Constantes.DiasRetroativosAquisicao;
                            gusaAFA.codCentro = CPS_Service_Lib.LibUtils.Constantes.CodCentro;

                            while (reader.Read())
                            {

                                gusaAFA.listaAnaliseGusaAFA.Add(new AnaliseQuimicaGusaAFA(
                                ParseTypes.GetInt(reader, 1), // CORRIDA, 
                                0, // TORPEDO, 
                                ParseTypes.GetFloatDecimal(reader, 3), // S, 
                                ParseTypes.GetFloatDecimal(reader, 4), // C, 
                                ParseTypes.GetFloatDecimal(reader, 5), // MN, 
                                ParseTypes.GetFloatDecimal(reader, 6), // P, 
                                ParseTypes.GetFloatDecimal(reader, 7), // SI, 
                                ParseTypes.GetFloatDecimal(reader, 8), // TI, 
                                ParseTypes.GetFloatDecimal(reader, 10), // CR, 
                                ParseTypes.GetFloatDecimal(reader, 11), // NI,
                                ParseTypes.GetString(reader, 13), // TURMA, 
                                " ", // MES
                                ParseTypes.GetInt(reader, 14), // TURNO
                                ParseTypes.GetDateTime(reader, 0), // DATA
                                CPS_Service_Lib.LibUtils.Constantes.CodCentro // CODCENTRO
                                ));

                            }

                            string sRetorno = rest.AnaliseQuimicaGusaAFA(gusaAFA);

                            CPSeventLog.WriteEntry("GusaAFA: " + sRetorno, EventLogEntryType.Information, eventId++);
                            logEmail.Add("GusaAFA: " + sRetorno);
                            logEmail.Add("Registros: " + gusaAFA.listaAnaliseGusaAFA.Count.ToString());
                            logEmail.Add("Dias Retroativos: " + Utils.Constantes.DiasRetroativosAquisicao.ToString());

                        }
                        catch (Exception ex)
                        {
                            CPSeventLog.WriteEntry("GusaAFA: " + ex.Message, EventLogEntryType.Error, eventId++);
                            logEmail.Add("GusaAFA: " + ex.Message);
                        }
                    }
                }
            }

        }








        // ********************************************************************************************************* //
        //                                    B I O M E T R I C      I T                     (www.biometric-it.com)  //
        // --------------------------------------------------------------------------------------------------------- //
        //  @Autor: MAURICIO XAVIER                                                                                  //
        //  @Data:  19/05/2020                                                                                       //
        //  @Descricao:                                                                                              //
        // ********************************************************************************************************* //
        public void GetAcoConvertedor()
        {
            this.CreateConnection();

            using (this.connection)
            {

                using (SqlCommand command = new SqlCommand(SqlBase.SelectAcoCV, connection))
                {
                    command.CommandTimeout = 120;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        RestTools rest = new RestTools();

                        try
                        {
                            AnaliseQuimicaAcoCVDTO acoCV = new AnaliseQuimicaAcoCVDTO();
                            acoCV.diasRetroativos = Constantes.DiasRetroativosAquisicao;
                            acoCV.codCentro = CPS_Service_Lib.LibUtils.Constantes.CodCentro;

                            while (reader.Read())
                            {
                                acoCV.listaAnaliseAcoCV.Add(new AnaliseQuimicaAcoCV(
                                    ParseTypes.GetInt(reader, 1), // CORRIDA, 
                                    ParseTypes.GetString(reader, 2), // ACO, 
                                    ParseTypes.GetDecimal(reader, 4), // S, 
                                    ParseTypes.GetDecimal(reader, 5), // C, 
                                    ParseTypes.GetDecimal(reader, 6), // MN, 
                                    ParseTypes.GetDecimal(reader, 7), // P, 
                                    ParseTypes.GetDecimal(reader, 8), // SI, 
                                    ParseTypes.GetDecimal(reader, 9), // AL
                                    ParseTypes.GetDecimal(reader, 10), // N
                                    ParseTypes.GetDecimal(reader, 11), // B
                                    ParseTypes.GetDecimal(reader, 12), // NB
                                    ParseTypes.GetDecimal(reader, 13), // TI, 
                                    ParseTypes.GetDecimal(reader, 14), // CU, 
                                    ParseTypes.GetDecimal(reader, 15), // CR,
                                    ParseTypes.GetDecimal(reader, 16), // MO,
                                    ParseTypes.GetDecimal(reader, 17),  // NI
                                    ParseTypes.GetDecimal(reader, 18),  // V
                                    ParseTypes.GetDecimal(reader, 19),  // CA
                                    ParseTypes.GetDecimal(reader, 20),  // SN
                                    ParseTypes.GetDecimal(reader, 21),  // CX
                                    ParseTypes.GetString(reader, 23), // TURMA, 
                                    ParseTypes.GetInt(reader, 24), // TURNO
                                    ParseTypes.GetString(reader, 3),  // AMOSTRA 
                                    ParseTypes.GetDateTime(reader, 0), // DATA
                                    CPS_Service_Lib.LibUtils.Constantes.CodCentro, // CODCENTRO
                                    ParseTypes.GetString(reader, 25), // String PROCESSO, 
                                    ParseTypes.GetString(reader, 26), // String APLICACAO, 
                                    ParseTypes.GetString(reader, 27), // String ACO_LING, 
                                    ParseTypes.GetString(reader, 28), // String TEOR_P, 
                                    ParseTypes.GetString(reader, 29), // String TEOR_C, 
                                    ParseTypes.GetString(reader, 30),  // String CONSUM_LIGAS   
                                    (ParseTypes.GetDecimalDefault(reader, 7) + ParseTypes.GetDecimalDefault(reader, 4)) //Nullable<decimal> ps
                                    ));
                            }

                           
                            string sRetorno = rest.AnaliseQuimicaAcoCV(acoCV);

                            CPSeventLog.WriteEntry("Aco CV: " + sRetorno.ToString(), EventLogEntryType.Information, eventId++);
                            logEmail.Add("Aco CV: " + sRetorno);
                            logEmail.Add("Registros: " + acoCV.listaAnaliseAcoCV.Count.ToString());
                            logEmail.Add("Dias Retroativos: " + Utils.Constantes.DiasRetroativosAquisicao.ToString());
                        }
                        catch (Exception ex)
                        {
                            CPSeventLog.WriteEntry("Aco CV: " + ex.Message, EventLogEntryType.Error, eventId++);
                            logEmail.Add("Aco CV: " + ex.Message);
                        }
                    }
                }
            }

        }






        // ********************************************************************************************************* //
        //                                    B I O M E T R I C      I T                     (www.biometric-it.com)  //
        // --------------------------------------------------------------------------------------------------------- //
        //  @Autor: MAURICIO XAVIER                                                                                  //
        //  @Data:  19/05/2020                                                                                       //
        //  @Descricao:                                                                                              //
        // ********************************************************************************************************* //
        public void GetAcoMLC()
        {
            this.CreateConnection();

            using (this.connection)
            {

                using (SqlCommand command = new SqlCommand(SqlBase.SelectAcoMLC, connection))
                {
                    command.CommandTimeout = 120;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        RestTools rest = new RestTools();

                        try
                        {
                            AnaliseQuimicaAcoMLCDTO acoMLC = new AnaliseQuimicaAcoMLCDTO();
                            acoMLC.diasRetroativos = Constantes.DiasRetroativosAquisicao;
                            acoMLC.codCentro = CPS_Service_Lib.LibUtils.Constantes.CodCentro;

                            while (reader.Read())
                            {
                                acoMLC.listaAnaliseAcoMLC.Add(new AnaliseQuimicaAcoMLC(
                                    ParseTypes.GetInt(reader, 1), // CORRIDA, 
                                    ParseTypes.GetString(reader, 2), // ACO, 
                                    ParseTypes.GetDecimal(reader, 4), // S, 
                                    ParseTypes.GetDecimal(reader, 5), // C, 
                                    ParseTypes.GetDecimal(reader, 6), // MN, 
                                    ParseTypes.GetDecimal(reader, 7), // P, 
                                    ParseTypes.GetDecimal(reader, 8), // SI, 
                                    ParseTypes.GetDecimal(reader, 9), // AL
                                    ParseTypes.GetDecimal(reader, 10), // N
                                    ParseTypes.GetDecimal(reader, 11), // B
                                    ParseTypes.GetDecimal(reader, 12), // NB
                                    ParseTypes.GetDecimal(reader, 13), // TI, 
                                    ParseTypes.GetDecimal(reader, 14), // CU, 
                                    ParseTypes.GetDecimal(reader, 15), // CR,
                                    ParseTypes.GetDecimal(reader, 16), // MO,
                                    ParseTypes.GetDecimal(reader, 17),  // NI
                                    ParseTypes.GetDecimal(reader, 18),  // V
                                    ParseTypes.GetDecimal(reader, 19),  // CA
                                    ParseTypes.GetDecimal(reader, 20),  // SN
                                    ParseTypes.GetDecimal(reader, 21),  // CX
                                    ParseTypes.GetString(reader, 23), // TURMA, 
                                    ParseTypes.GetInt(reader, 24), // TURNO
                                    ParseTypes.GetString(reader, 3),  // AMOSTRA 
                                    ParseTypes.GetDateTime(reader, 0), // DATA
                                    CPS_Service_Lib.LibUtils.Constantes.CodCentro, // CODCENTRO
                                    ParseTypes.GetString(reader, 25), // String PROCESSO, 
                                    ParseTypes.GetString(reader, 26), // String APLICACAO, 
                                    ParseTypes.GetString(reader, 27), // String ACO_LING, 
                                    ParseTypes.GetString(reader, 28), // String TEOR_P, 
                                    ParseTypes.GetString(reader, 29), // String TEOR_C, 
                                    ParseTypes.GetString(reader, 30)  // String CONSUM_LIGAS   
                                    ));
                            }


                            string sRetorno = rest.AnaliseQuimicaAcoMLC(acoMLC);

                            CPSeventLog.WriteEntry("Aco MLC: " + sRetorno.ToString(), EventLogEntryType.Information, eventId++);
                            logEmail.Add("Aco MLC: " + sRetorno);
                            logEmail.Add("Registros: " + acoMLC.listaAnaliseAcoMLC.Count.ToString());
                            logEmail.Add("Dias Retroativos: " + Utils.Constantes.DiasRetroativosAquisicao.ToString());
                        }
                        catch (Exception ex)
                        {
                            CPSeventLog.WriteEntry("Aco MLC: " + ex.Message, EventLogEntryType.Error, eventId++);
                            logEmail.Add("Aco MLC: " + ex.Message);
                        }
                    }
                }
            }

        }





        // ********************************************************************************************************* //
        //                                    B I O M E T R I C      I T                     (www.biometric-it.com)  //
        // --------------------------------------------------------------------------------------------------------- //
        //  @Autor: MAURICIO XAVIER                                                                                  //
        //  @Data:  19/05/2020                                                                                       //
        //  @Descricao:                                                                                              //
        // ********************************************************************************************************* //
        public void GetAcoPanela()
        {
            this.CreateConnection();

            using (this.connection)
            {

                using (SqlCommand command = new SqlCommand(SqlBase.SelectAcoPA, connection))
                {
                    command.CommandTimeout = 120;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        RestTools rest = new RestTools();

                        try
                        {
                            AnaliseQuimicaAcoPADTO acoPA = new AnaliseQuimicaAcoPADTO();
                            acoPA.diasRetroativos = Constantes.DiasRetroativosAquisicao;
                            acoPA.codCentro = CPS_Service_Lib.LibUtils.Constantes.CodCentro;

                            while (reader.Read())
                            {
                                acoPA.listaAnaliseAcoPA.Add(new AnaliseQuimicaAcoPA(
                                    ParseTypes.GetInt(reader, 1), // CORRIDA, 
                                    ParseTypes.GetString(reader, 2), // ACO, 
                                    ParseTypes.GetDecimal(reader, 4), // S, 
                                    ParseTypes.GetDecimal(reader, 5), // C, 
                                    ParseTypes.GetDecimal(reader, 6), // MN, 
                                    ParseTypes.GetDecimal(reader, 7), // P, 
                                    ParseTypes.GetDecimal(reader, 8), // SI, 
                                    ParseTypes.GetDecimal(reader, 9), // AL
                                    ParseTypes.GetDecimal(reader, 10), // N
                                    ParseTypes.GetDecimal(reader, 11), // B
                                    ParseTypes.GetDecimal(reader, 12), // NB
                                    ParseTypes.GetDecimal(reader, 13), // TI, 
                                    ParseTypes.GetDecimal(reader, 14), // CU, 
                                    ParseTypes.GetDecimal(reader, 15), // CR,
                                    ParseTypes.GetDecimal(reader, 16), // MO,
                                    ParseTypes.GetDecimal(reader, 17),  // NI
                                    ParseTypes.GetDecimal(reader, 18),  // V
                                    ParseTypes.GetDecimal(reader, 19),  // CA
                                    ParseTypes.GetDecimal(reader, 20),  // SN
                                    ParseTypes.GetDecimal(reader, 21),  // CX
                                    ParseTypes.GetString(reader, 23), // TURMA, 
                                    ParseTypes.GetInt(reader, 24), // TURNO
                                    ParseTypes.GetString(reader, 3),  // AMOSTRA 
                                    ParseTypes.GetDateTime(reader, 0), // DATA
                                    CPS_Service_Lib.LibUtils.Constantes.CodCentro, // CODCENTRO
                                    ParseTypes.GetString(reader, 25), // String PROCESSO, 
                                    ParseTypes.GetString(reader, 26), // String APLICACAO, 
                                    ParseTypes.GetString(reader, 27), // String ACO_LING, 
                                    ParseTypes.GetString(reader, 28), // String TEOR_P, 
                                    ParseTypes.GetString(reader, 29), // String TEOR_C, 
                                    ParseTypes.GetString(reader, 30),  // String CONSUM_LIGAS   
                                    (ParseTypes.GetDecimalDefault(reader, 7) + ParseTypes.GetDecimalDefault(reader, 4)) // Nullable<decimal> ps
                                    ));
                            }

                            string sRetorno = rest.AnaliseQuimicaAcoPA(acoPA);

                            CPSeventLog.WriteEntry("Aco PA: " + sRetorno, EventLogEntryType.Information, eventId++);
                            logEmail.Add("Aco PA: " + sRetorno);
                            logEmail.Add("Registros: " + acoPA.listaAnaliseAcoPA.Count.ToString());
                            logEmail.Add("Dias Retroativos: " + Utils.Constantes.DiasRetroativosAquisicao.ToString());

                        }
                        catch (Exception ex)
                        {
                            CPSeventLog.WriteEntry("Aco PA: " + ex.Message, EventLogEntryType.Error, eventId++);
                            logEmail.Add("Aco PA: " + ex.Message);
                        }
                    }
                }
            }

        }





        // ********************************************************************************************************* //
        //                                    B I O M E T R I C      I T                     (www.biometric-it.com)  //
        // --------------------------------------------------------------------------------------------------------- //
        //  @Autor: MAURICIO XAVIER                                                                                  //
        //  @Data:  19/05/2020                                                                                       //
        //  @Descricao:                                                                                              //
        // ********************************************************************************************************* //
        public void GetAcoFornoPanela()
        {
            this.CreateConnection();

            using (this.connection)
            {

                using (SqlCommand command = new SqlCommand(SqlBase.SelectAcoFP, connection))
                {
                    command.CommandTimeout = 120;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        RestTools rest = new RestTools();

                        try
                        {
                            AnaliseQuimicaAcoFPDTO acoFP = new AnaliseQuimicaAcoFPDTO();
                            acoFP.diasRetroativos = Constantes.DiasRetroativosAquisicao;
                            acoFP.codCentro = CPS_Service_Lib.LibUtils.Constantes.CodCentro;

                            while (reader.Read())
                            {
                                acoFP.listaAnaliseAcoFP.Add(new AnaliseQuimicaAcoFP(
                                    ParseTypes.GetInt(reader, 1), // CORRIDA, 
                                    ParseTypes.GetString(reader, 2), // ACO, 
                                    ParseTypes.GetDecimal(reader, 4), // S, 
                                    ParseTypes.GetDecimal(reader, 5), // C, 
                                    ParseTypes.GetDecimal(reader, 6), // MN, 
                                    ParseTypes.GetDecimal(reader, 7), // P, 
                                    ParseTypes.GetDecimal(reader, 8), // SI, 
                                    ParseTypes.GetDecimal(reader, 9), // AL
                                    ParseTypes.GetDecimal(reader, 10), // N
                                    ParseTypes.GetDecimal(reader, 11), // B
                                    ParseTypes.GetDecimal(reader, 12), // NB
                                    ParseTypes.GetDecimal(reader, 13), // TI, 
                                    ParseTypes.GetDecimal(reader, 14), // CU, 
                                    ParseTypes.GetDecimal(reader, 15), // CR,
                                    ParseTypes.GetDecimal(reader, 16), // MO,
                                    ParseTypes.GetDecimal(reader, 17),  // NI
                                    ParseTypes.GetDecimal(reader, 18),  // V
                                    ParseTypes.GetDecimal(reader, 19),  // CA
                                    ParseTypes.GetDecimal(reader, 20),  // SN                                   
                                    ParseTypes.GetString(reader, 22), // TURMA, 
                                    ParseTypes.GetInt(reader, 23), // TURNO
                                    ParseTypes.GetString(reader, 3),  // AMOSTRA 
                                    ParseTypes.GetDateTime(reader, 0), // DATA
                                    CPS_Service_Lib.LibUtils.Constantes.CodCentro, //CODCENTRO
                                    ParseTypes.GetString(reader, 24), // String PROCESSO, 
                                    ParseTypes.GetString(reader, 25), // String APLICACAO, 
                                    ParseTypes.GetString(reader, 26), // String ACO_LING, 
                                    ParseTypes.GetString(reader, 27), // String TEOR_P, 
                                    ParseTypes.GetString(reader, 28), // String TEOR_C, 
                                    ParseTypes.GetString(reader, 29),  // String CONSUM_LIGAS   
                                    (ParseTypes.GetDecimalDefault(reader, 7) + ParseTypes.GetDecimalDefault(reader, 4)) // Nullable<decimal> ps
                                    ));
                            }

                            string sRetorno = rest.AnaliseQuimicaAcoFP(acoFP);

                            CPSeventLog.WriteEntry("Aco FP: " + sRetorno, EventLogEntryType.Information, eventId++);
                            logEmail.Add("Aco FP: " + sRetorno);
                            logEmail.Add("Registros: " + acoFP.listaAnaliseAcoFP.Count.ToString());
                            logEmail.Add("Dias Retroativos: " + Utils.Constantes.DiasRetroativosAquisicao.ToString());
                        }
                        catch (Exception ex)
                        {
                            CPSeventLog.WriteEntry("Aco FP: " + ex.Message, EventLogEntryType.Error, eventId++);
                            logEmail.Add("Aco FP: " + ex.Message);
                        }
                    }
                }
            }

        }





        // ********************************************************************************************************* //
        //                                    B I O M E T R I C      I T                     (www.biometric-it.com)  //
        // --------------------------------------------------------------------------------------------------------- //
        //  @Autor: MAURICIO XAVIER                                                                                  //
        //  @Data:  19/05/2020                                                                                       //
        //  @Descricao:                                                                                              //
        // ********************************************************************************************************* //
        public void GetAcoFinal()
        {
            this.CreateConnection();

            using (this.connection)
            {

                using (SqlCommand command = new SqlCommand(SqlBase.SelectAcoFI, connection))
                {
                    command.CommandTimeout = 120;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        RestTools rest = new RestTools();

                        try
                        {
                            AnaliseQuimicaAcoFIDTO acoFI = new AnaliseQuimicaAcoFIDTO();
                            acoFI.diasRetroativos = Constantes.DiasRetroativosAquisicao;
                            acoFI.codCentro = CPS_Service_Lib.LibUtils.Constantes.CodCentro;

                            while (reader.Read())
                            {
                                acoFI.listaAnaliseAcoFI.Add(new AnaliseQuimicaAcoFI(
                                    ParseTypes.GetInt(reader, 1), // CORRIDA, 
                                    ParseTypes.GetString(reader, 2), // ACO, 
                                    ParseTypes.GetDecimal(reader, 4), // S, 
                                    ParseTypes.GetDecimal(reader, 5), // C, 
                                    ParseTypes.GetDecimal(reader, 6), // MN, 
                                    ParseTypes.GetDecimal(reader, 7), // P, 
                                    ParseTypes.GetDecimal(reader, 8), // SI, 
                                    ParseTypes.GetDecimal(reader, 9), // AL
                                    ParseTypes.GetDecimal(reader, 10), // N
                                    ParseTypes.GetDecimal(reader, 11), // B
                                    ParseTypes.GetDecimal(reader, 12), // NB
                                    ParseTypes.GetDecimal(reader, 13), // TI, 
                                    ParseTypes.GetDecimal(reader, 14), // CU, 
                                    ParseTypes.GetDecimal(reader, 15), // CR,
                                    ParseTypes.GetDecimal(reader, 16), // MO,
                                    ParseTypes.GetDecimal(reader, 17),  // NI
                                    ParseTypes.GetDecimal(reader, 18),  // V
                                    ParseTypes.GetDecimal(reader, 19),  // CA
                                    ParseTypes.GetDecimal(reader, 20),  // SN
                                    ParseTypes.GetDecimal(reader, 24),  // MN_S
                                    ParseTypes.GetString(reader, 22), //TURMA, 
                                    ParseTypes.GetInt(reader, 23), //TURNO
                                    ParseTypes.GetString(reader, 3),  // AMOSTRA 
                                    ParseTypes.GetDateTime(reader, 0), // DATA
                                    CPS_Service_Lib.LibUtils.Constantes.CodCentro, // CODCENTRO
                                    ParseTypes.GetString(reader, 25), // String PROCESSO, 
                                    ParseTypes.GetString(reader, 26), // String APLICACAO, 
                                    ParseTypes.GetString(reader, 27), // String ACO_LING, 
                                    ParseTypes.GetString(reader, 28), // String TEOR_P, 
                                    ParseTypes.GetString(reader, 29), // String TEOR_C, 
                                    ParseTypes.GetString(reader, 30)  // String CONSUM_LIGAS   
                                    ));
                            }

                            string sRetorno = rest.AnaliseQuimicaAcoFI(acoFI);

                            CPSeventLog.WriteEntry("Aco FI: " + sRetorno, EventLogEntryType.Information, eventId++);
                            logEmail.Add("Aco FI: " + sRetorno);
                            logEmail.Add("Registros: " + acoFI.listaAnaliseAcoFI.Count.ToString());
                            logEmail.Add("Dias Retroativos: " + Utils.Constantes.DiasRetroativosAquisicao.ToString());
                        }
                        catch (Exception ex)
                        {
                            CPSeventLog.WriteEntry("Aco FI: " + ex.Message, EventLogEntryType.Error, eventId++);
                            logEmail.Add("Aco FI: " + ex.Message);
                        }
                    }
                }
            }

        }






        // ********************************************************************************************************* //
        //                                    B I O M E T R I C      I T                     (www.biometric-it.com)  //
        // --------------------------------------------------------------------------------------------------------- //
        //  @Autor: MAURICIO XAVIER                                                                                  //
        //  @Data:  19/05/2020                                                                                       //
        //  @Descricao:                                                                                              //
        // ********************************************************************************************************* //
        public void GetEscoriaCV()
        {
            this.CreateConnection();

            using (this.connection)
            {

                using (SqlCommand command = new SqlCommand(SqlBase.SelectEscoriaCV, connection))
                {
                    command.CommandTimeout = 120;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        RestTools rest = new RestTools();

                        try
                        {
                            AnaliseQuimicaEscoriaCVDTO escoriaCV = new AnaliseQuimicaEscoriaCVDTO();
                            escoriaCV.diasRetroativos = Constantes.DiasRetroativosAquisicao;
                            escoriaCV.codCentro = CPS_Service_Lib.LibUtils.Constantes.CodCentro;

                            while (reader.Read())
                            {
                                escoriaCV.listaAnaliseEscoriaCV.Add(new AnaliseQuimicaEscoriaCV(
                                    ParseTypes.GetInt(reader, 1), // CORRIDA, 
                                    ParseTypes.GetString(reader, 2), // ACO, 
                                    ParseTypes.GetDecimal(reader, 4), // SIO2, 
                                    ParseTypes.GetDecimal(reader, 5), // FE, 
                                    ParseTypes.GetDecimal(reader, 6), // MNO, 
                                    ParseTypes.GetDecimal(reader, 7), // P2O5, 
                                    ParseTypes.GetDecimal(reader, 8), // AL2O3, 
                                    ParseTypes.GetDecimal(reader, 9), // CAO
                                    ParseTypes.GetDecimal(reader, 10), // MGO
                                    ParseTypes.GetString(reader, 12), //TURMA, 
                                    ParseTypes.GetInt(reader, 13), //TURNO
                                    ParseTypes.GetString(reader, 3).ToString(),  // AMOSTRA 
                                    ParseTypes.GetDateTime(reader, 0), // DATA
                                    CPS_Service_Lib.LibUtils.Constantes.CodCentro, // CODCENTRO
                                    ParseTypes.GetString(reader, 14), // String PROCESSO, 
                                    ParseTypes.GetString(reader, 15), // String APLICACAO, 
                                    ParseTypes.GetString(reader, 16), // String ACO_LING, 
                                    ParseTypes.GetString(reader, 17), // String TEOR_P, 
                                    ParseTypes.GetString(reader, 18), // String TEOR_C, 
                                    ParseTypes.GetString(reader, 19)  // String CONSUM_LIGAS   
                                    ));
                            }

                            string sRetorno = rest.AnaliseQuimicaEscoriaCV(escoriaCV);

                            CPSeventLog.WriteEntry("Escoria CV: " + sRetorno, EventLogEntryType.Information, eventId++);
                            logEmail.Add("Escoria CV: " + sRetorno);
                            logEmail.Add("Registros: " + escoriaCV.listaAnaliseEscoriaCV.Count.ToString());
                            logEmail.Add("Dias Retroativos: " + Utils.Constantes.DiasRetroativosAquisicao.ToString());
                        }
                        catch (Exception ex)
                        {
                           CPSeventLog.WriteEntry("Escoria CV: " + ex.Message, EventLogEntryType.Error, eventId++);
                            logEmail.Add("Escoria CV: " + ex.Message);
                        }
                    }
                }
            }

        }




        // ********************************************************************************************************* //
        //                                    B I O M E T R I C      I T                     (www.biometric-it.com)  //
        // --------------------------------------------------------------------------------------------------------- //
        //  @Autor: MAURICIO XAVIER                                                                                  //
        //  @Data:  19/05/2020                                                                                       //
        //  @Descricao:                                                                                              //
        // ********************************************************************************************************* //
        public void GetEscoriaFP()
        {
            this.CreateConnection();

            using (this.connection)
            {

                using (SqlCommand command = new SqlCommand(SqlBase.SelectEscoriaFP, connection))
                {
                    command.CommandTimeout = 120;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        RestTools rest = new RestTools();

                        try
                        {
                            AnaliseQuimicaEscoriaFPDTO escoriaFP = new AnaliseQuimicaEscoriaFPDTO();
                            escoriaFP.diasRetroativos = Constantes.DiasRetroativosAquisicao;
                            escoriaFP.codCentro = CPS_Service_Lib.LibUtils.Constantes.CodCentro;

                            while (reader.Read())
                            {
                                escoriaFP.listaAnaliseEscoriaFP.Add(new AnaliseQuimicaEscoriaFP(
                                    ParseTypes.GetInt(reader, 1), // CORRIDA, 
                                    ParseTypes.GetString(reader, 2), // ACO, 
                                    ParseTypes.GetDecimal(reader, 4), // SIO2, 
                                    ParseTypes.GetDecimal(reader, 5), // FE, 
                                    ParseTypes.GetDecimal(reader, 6), // MNO, 
                                    ParseTypes.GetDecimal(reader, 7), // P2O5, 
                                    ParseTypes.GetDecimal(reader, 8), // AL2O3, 
                                    ParseTypes.GetDecimal(reader, 9), // CAO
                                    ParseTypes.GetDecimal(reader, 10), // MGO
                                    ParseTypes.GetString(reader, 12), //TURMA, 
                                    ParseTypes.GetInt(reader, 13), //TURNO
                                    ParseTypes.GetString(reader, 3),  // AMOSTRA 
                                    ParseTypes.GetDateTime(reader, 0), // DATA
                                    CPS_Service_Lib.LibUtils.Constantes.CodCentro, // CODCENTRO
                                    ParseTypes.GetString(reader, 14), // String PROCESSO, 
                                    ParseTypes.GetString(reader, 15), // String APLICACAO, 
                                    ParseTypes.GetString(reader, 16), // String ACO_LING, 
                                    ParseTypes.GetString(reader, 17), // String TEOR_P, 
                                    ParseTypes.GetString(reader, 18), // String TEOR_C, 
                                    ParseTypes.GetString(reader, 19)  // String CONSUM_LIGAS   
                                   ));
                            }

                            string sRetorno = rest.AnaliseQuimicaEscoriaFP(escoriaFP);

                            CPSeventLog.WriteEntry("Escoria FP: " + sRetorno, EventLogEntryType.Information, eventId++);
                            logEmail.Add("Escoria FP: " + sRetorno);
                            logEmail.Add("Registros: " + escoriaFP.listaAnaliseEscoriaFP.Count.ToString());
                            logEmail.Add("Dias Retroativos: " + Utils.Constantes.DiasRetroativosAquisicao.ToString());
                        }
                        catch (Exception ex)
                        {
                            CPSeventLog.WriteEntry("Escoria FP: " + ex.Message, EventLogEntryType.Error, eventId++);
                            logEmail.Add("Escoria FP: " + ex.Message);
                        }
                    }
                }
            }

        }




        // ********************************************************************************************************* //
        //                                    B I O M E T R I C      I T                     (www.biometric-it.com)  //
        // --------------------------------------------------------------------------------------------------------- //
        //  @Autor: MAURICIO XAVIER                                                                                  //
        //  @Data:  19/05/2020                                                                                       //
        //  @Descricao:                                                                                              //
        // ********************************************************************************************************* //
        public void GetEscoriaPA()
        {
            this.CreateConnection();

            using (this.connection)
            {

                using (SqlCommand command = new SqlCommand(SqlBase.SelectEscoriaPA, connection))
                {
                    command.CommandTimeout = 120;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        RestTools rest = new RestTools();

                        try
                        {
                            AnaliseQuimicaEscoriaPADTO escoriaPA = new AnaliseQuimicaEscoriaPADTO();
                            escoriaPA.diasRetroativos = Constantes.DiasRetroativosAquisicao;
                            escoriaPA.codCentro = CPS_Service_Lib.LibUtils.Constantes.CodCentro;

                            while (reader.Read())
                            {
                                escoriaPA.listaAnaliseEscoriaPA.Add(new AnaliseQuimicaEscoriaPA(
                                    ParseTypes.GetInt(reader, 1), // CORRIDA, 
                                    ParseTypes.GetString(reader, 2), // ACO, 
                                    ParseTypes.GetDecimal(reader, 4), // SIO2, 
                                    ParseTypes.GetDecimal(reader, 5), // FE, 
                                    ParseTypes.GetDecimal(reader, 6), // MNO, 
                                    ParseTypes.GetDecimal(reader, 7), // P2O5, 
                                    ParseTypes.GetDecimal(reader, 8), // AL2O3, 
                                    ParseTypes.GetDecimal(reader, 9), // CAO
                                    ParseTypes.GetDecimal(reader, 10), // MGO
                                    ParseTypes.GetString(reader, 12), //TURMA, 
                                    ParseTypes.GetInt(reader, 13), //TURNO
                                    ParseTypes.GetString(reader, 3).ToString(),  // AMOSTRA 
                                    ParseTypes.GetDateTime(reader, 0), // DATA
                                    CPS_Service_Lib.LibUtils.Constantes.CodCentro, // CODCENTRO
                                    ParseTypes.GetString(reader, 14), // String PROCESSO, 
                                    ParseTypes.GetString(reader, 15), // String APLICACAO, 
                                    ParseTypes.GetString(reader, 16), // String ACO_LING, 
                                    ParseTypes.GetString(reader, 17), // String TEOR_P, 
                                    ParseTypes.GetString(reader, 18), // String TEOR_C, 
                                    ParseTypes.GetString(reader, 19)  // String CONSUM_LIGAS   
                                   ));
                            }

                            string sRetorno = rest.AnaliseQuimicaEscoriaPA(escoriaPA);

                            CPSeventLog.WriteEntry("Escoria PA: " + sRetorno, EventLogEntryType.Information, eventId++);
                            logEmail.Add("Escoria PA: " + sRetorno);
                            logEmail.Add("Registros: " + escoriaPA.listaAnaliseEscoriaPA.Count.ToString());
                            logEmail.Add("Dias Retroativos: " + Utils.Constantes.DiasRetroativosAquisicao.ToString());
                        }
                        catch (Exception ex)
                        {
                            CPSeventLog.WriteEntry("Escoria PA: " + ex.Message, EventLogEntryType.Error, eventId++);
                            logEmail.Add("Escoria PA: " + ex.Message);
                        }
                    }
                }
            }

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
            try
            {
                if (this.builder == null)
                {
                    builder = new SqlConnectionStringBuilder();
                }


                // pegar do arquivo .INI
                builder.DataSource = Constantes.DataSource;
                builder.UserID = Constantes.UserID;
                builder.Password = Constantes.Password;
                builder.InitialCatalog = Constantes.InitialCatalog;


                if (this.connection == null)
                {
                    connection = new SqlConnection(builder.ConnectionString);
                }


                if ((this.connection.State == System.Data.ConnectionState.Closed) ||
                    (this.connection.State == System.Data.ConnectionState.Broken))
                {
                    this.connection.ConnectionString = builder.ConnectionString;
                    this.connection.Open();

                }

            }
            catch (SqlException e)
            {
                this.CPSeventLog.WriteEntry("SqlServerTools.CreateConnection: " + e.ToString(), EventLogEntryType.Error, eventId++);
            }
        }





        // ********************************************************************************************************* //
        //                                    B I O M E T R I C      I T                     (www.biometric-it.com)  //
        // --------------------------------------------------------------------------------------------------------- //
        //  @Autor: MAURICIO XAVIER                                                                                  //
        //  @Data:  19/05/2020                                                                                       //
        //  @Descricao:                                                                                              //
        // ********************************************************************************************************* //
        public void CreateConnectionAFA()
        {
            try
            {
                if (this.builder == null)
                {
                    builder = new SqlConnectionStringBuilder();
                }


                // pegar do arquivo .INI
                builder.DataSource = Constantes.DataSourceAFA;
                builder.UserID = Constantes.UserIDAFA;
                builder.Password = Constantes.PasswordAFA;
                builder.InitialCatalog = Constantes.InitialCatalogAFA;


                if (this.connectionAFA == null)
                {
                    connectionAFA = new SqlConnection(builder.ConnectionString);
                }


                if ((this.connectionAFA.State == System.Data.ConnectionState.Closed) ||
                    (this.connectionAFA.State == System.Data.ConnectionState.Broken))
                {
                    this.connectionAFA.ConnectionString = builder.ConnectionString;
                    this.connectionAFA.Open();

                }

            }
            catch (SqlException e)
            {
                this.CPSeventLog.WriteEntry("SqlServerTools.CreateConnection: " + e.ToString(), EventLogEntryType.Error, eventId++);
            }
        }




    }
}
