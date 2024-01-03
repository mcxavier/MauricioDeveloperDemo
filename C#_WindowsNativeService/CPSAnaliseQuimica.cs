using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Timers;
using System.Linq;
using System.ServiceProcess;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CPS_Service_Lib.Enumeracoes;
using CPS_Service_Lib.LibUtils;
using CPS_AnaliseQuimica.Utils;
using CPS_AnaliseQuimica.DataBase;
using System.Net;


namespace CPS_AnaliseQuimica
{
    // ********************************************************************************************************* //
    //                                    B I O M E T R I C      I T                     (www.biometric-it.com)  //
    // --------------------------------------------------------------------------------------------------------- //
    //  @Autor: MAURICIO XAVIER                                                                                  //
    //  @Data:  19/05/2020                                                                                       //
    //  @Descricao:                                                                                              //
    // ********************************************************************************************************* //
    public partial class CPSAnaliseQuimica : ServiceBase
    {

        // HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services

        public EventLog CPSeventLog { get; private set; }
        public List<string> logEmail { get; set; }
        private int eventId = 1;
        public SqlServerTools sql { get; set; }

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(System.IntPtr handle, ref ServiceStatus serviceStatus);


        // ********************************************************************************************************* //
        //                                    B I O M E T R I C      I T                     (www.biometric-it.com)  //
        // --------------------------------------------------------------------------------------------------------- //
        //  @Autor: MAURICIO XAVIER                                                                                  //
        //  @Data:  19/05/2020                                                                                       //
        //  @Descricao:                                                                                              //
        // ********************************************************************************************************* //
        public CPSAnaliseQuimica()
        {
            InitializeComponent();

            CPSeventLog = new EventLog();           

            EventLogEntryCollection log = CPSeventLog.Entries;


            if (!EventLog.SourceExists(Utils.Constantes.EventLogSource))
            {
                EventLog.CreateEventSource(Utils.Constantes.EventLogSource, CPS_Service_Lib.LibUtils.Constantes.EventLogName);
            }

            CPSeventLog.Source = Utils.Constantes.EventLogSource;
            CPSeventLog.Log = CPS_Service_Lib.LibUtils.Constantes.EventLogName;

        }





        // ********************************************************************************************************* //
        //                                    B I O M E T R I C      I T                     (www.biometric-it.com)  //
        // --------------------------------------------------------------------------------------------------------- //
        //  @Autor: MAURICIO XAVIER                                                                                  //
        //  @Data:  19/05/2020                                                                                       //
        //  @Descricao:                                                                                              //
        // ********************************************************************************************************* //
        private void GetInformacoesSAA()
        {
            DateTime data;
            TimeSpan delta;
            logEmail = new List<string>();

            logEmail.Add("Iniciando Aquisição de Dados da Análise Química em " + DateTime.Now.ToShortDateString() + " as " + DateTime.Now.ToShortTimeString() + " no servidor [" + Dns.GetHostName() + "]");
            logEmail.Add("BackEnd: " + Utils.Constantes.Servidor);
            logEmail.Add("");

            CPSeventLog.WriteEntry("Iniciando Aquisição de Dados da Análise Química.", EventLogEntryType.Information, eventId++);

                try
                {
                    sql = new SqlServerTools(CPSeventLog, logEmail);

                    try
                    {
                        data = DateTime.Now;
                        CPSeventLog.WriteEntry("Buscando dados de GUSA.", EventLogEntryType.Information, eventId++);
                        logEmail.Add("Buscando dados de GUSA:");
                        sql.GetGusa();
                        delta = (DateTime.Now - data);
                        logEmail.Add("Tempo de Processamento: " + delta.ToString());                        
                        logEmail.Add("");
                    }
                    catch (Exception ex) {
                        logEmail.Add("ERRO:" + ex.Message);
                        logEmail.Add("");
                    }


                    try
                    {
                        data = DateTime.Now;
                        CPSeventLog.WriteEntry("Buscando dados de AÇO CONVERTEDOR.", EventLogEntryType.Information, eventId++);
                        logEmail.Add("Buscando dados de AÇO CONVERTEDOR");
                        sql.GetAcoConvertedor();
                        delta = (DateTime.Now - data);
                        logEmail.Add("Tempo de Processamento: " + delta.ToString());
                        logEmail.Add("");
                    }
                    catch (Exception ex)
                    {
                        logEmail.Add("ERRO:" + ex.Message);
                        logEmail.Add("");
                    }


                    try
                    {
                        data = DateTime.Now;
                        CPSeventLog.WriteEntry("Buscando dados de AÇO FINAL.", EventLogEntryType.Information, eventId++);
                        logEmail.Add("Buscando Dados de AÇO FINAL");
                        sql.GetAcoFinal();
                        delta = (DateTime.Now - data);
                        logEmail.Add("Tempo de Processamento: " + delta.ToString());
                        logEmail.Add("");
                    }
                    catch (Exception ex)
                    {
                        logEmail.Add("ERRO:" + ex.Message);
                        logEmail.Add("");
                    }



                    try
                    {
                        data = DateTime.Now;
                        CPSeventLog.WriteEntry("Buscando dados de AÇO PANELA.", EventLogEntryType.Information, eventId++);
                        logEmail.Add("Buscando dados de AÇO PANELA");
                        sql.GetAcoPanela();
                        delta = (DateTime.Now - data);
                        logEmail.Add("Tempo de Processamento: " + delta.ToString());
                        logEmail.Add("");
                    }
                    catch (Exception ex)
                    {
                        logEmail.Add("ERRO:" + ex.Message);
                        logEmail.Add("");
                    }



                    try
                    {
                        data = DateTime.Now;
                        CPSeventLog.WriteEntry("Buscando dados de AÇO FORNO PANELA.", EventLogEntryType.Information, eventId++);
                        logEmail.Add("Buscando dados de AÇO FORNO PANELA");
                        sql.GetAcoFornoPanela();
                        delta = (DateTime.Now - data);
                        logEmail.Add("Tempo de Processamento: " + delta.ToString());
                        logEmail.Add("");
                    }
                    catch (Exception ex)
                    {
                        logEmail.Add("ERRO:" + ex.Message);
                        logEmail.Add("");
                    }


                try
                {
                    data = DateTime.Now;
                    CPSeventLog.WriteEntry("Buscando dados de AÇO MLC.", EventLogEntryType.Information, eventId++);
                    logEmail.Add("Buscando dados de AÇO MLC");
                    sql.GetAcoMLC();
                    delta = (DateTime.Now - data);
                    logEmail.Add("Tempo de Processamento: " + delta.ToString());
                    logEmail.Add("");
                }
                catch (Exception ex)
                {
                    logEmail.Add("ERRO:" + ex.Message);
                    logEmail.Add("");
                }

                try
                    {
                        data = DateTime.Now;
                        CPSeventLog.WriteEntry("Buscando dados de ESCÓRIA CV.", EventLogEntryType.Information, eventId++);
                        logEmail.Add("Buscando dados de ESCÓRIA CV");
                        sql.GetEscoriaCV();
                        delta = (DateTime.Now - data);
                        logEmail.Add("Tempo de Processamento: " + delta.ToString());
                        logEmail.Add("");
                    }
                    catch (Exception ex)
                    {
                        logEmail.Add("ERRO:" + ex.Message);
                        logEmail.Add("");
                    }


                    try
                    {
                        data = DateTime.Now;
                        CPSeventLog.WriteEntry("Buscando dados de ESCORIA FP.", EventLogEntryType.Information, eventId++);
                        logEmail.Add("Buscando dados de ESCORIA FP");
                        sql.GetEscoriaFP();
                        delta = (DateTime.Now - data);
                        logEmail.Add("Tempo de Processamento: " + delta.ToString());
                        logEmail.Add("");
                    }
                    catch (Exception ex)
                    {
                        logEmail.Add("ERRO:" + ex.Message);
                        logEmail.Add("");
                    }


                    try
                    {
                        data = DateTime.Now;
                        CPSeventLog.WriteEntry("Buscando dados de ESCÓRIA PA.", EventLogEntryType.Information, eventId++);
                        logEmail.Add("Buscando dados de ESCÓRIA PA");
                        sql.GetEscoriaPA();
                        delta = (DateTime.Now - data);
                        logEmail.Add("Tempo de Processamento: " + delta.ToString());
                        logEmail.Add("");
                    }
                    catch (Exception ex)
                    {
                        logEmail.Add("ERRO:" + ex.Message);
                        logEmail.Add("");
                    }


                try
                {
                    data = DateTime.Now;
                    CPSeventLog.WriteEntry("Buscando dados de GUSA AFA.", EventLogEntryType.Information, eventId++);
                    logEmail.Add("Buscando dados de GUSA AFA:");
                    sql.GetGusaAFA();
                    delta = (DateTime.Now - data);
                    logEmail.Add("Tempo de Processamento: " + delta.ToString());
                    logEmail.Add("");
                }
                catch (Exception ex)
                {
                    logEmail.Add("ERRO:" + ex.Message);
                    logEmail.Add("");
                }

            }
                catch (Exception e)
                {
                    this.CPSeventLog.WriteEntry("CPA_AnaliseQuimica.GetInformacoesSAA(): " + e.ToString(), EventLogEntryType.Error, eventId++);
                    logEmail.Add("CPA_AnaliseQuimica.GetInformacoesSAA(): " + e.ToString());
                }
                       
            
            logEmail.Add("Aquisição de Dados da Análise Química Finalizada em " + DateTime.Now.ToShortDateString() + " as " + DateTime.Now.ToShortTimeString());
            logEmail.Add("");
            logEmail.Add("");
            logEmail.Add("");


            // manda o e-mail
            SMTPService smtp = new SMTPService();

            //smtp.EmailResumoAquisicaoDados("RESUMO DA AQUISIÇÃO DE DADOS - ANÁLISE QUÍMICA", logEmail);

            sql = null;
            logEmail = null;
            smtp = null;
        }



        // ********************************************************************************************************* //
        //                                    B I O M E T R I C      I T                     (www.biometric-it.com)  //
        // --------------------------------------------------------------------------------------------------------- //
        //  @Autor: MAURICIO XAVIER                                                                                  //
        //  @Data:  19/05/2020                                                                                       //
        //  @Descricao:                                                                                              //
        // ********************************************************************************************************* //
        protected override void OnStart(string[] args)
        {            

            // Update the service state to Start Pending.
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            
            Timer timer = new Timer();
                             
            timer.Interval = 3600000; // 1 horas
            timer.Elapsed += new ElapsedEventHandler(this.OnTimer);
            timer.Start();
                       

            // Update the service state to Running.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            CPSeventLog.WriteEntry("Serviço iniciado...", EventLogEntryType.Information, eventId++);
            // manda o e-mail

            SMTPService smtp = new SMTPService();
            logEmail = new List<string>();
            logEmail.Add("O Serviço de aquisição de Dados CPA_ANALISEQUIMICA foi INICIADO em " + DateTime.Now.ToShortDateString() + " as " + DateTime.Now.ToShortTimeString() + " no servidor [" + Dns.GetHostName() + "]");
            smtp.EmailResumoAquisicaoDados(Utils.Constantes.EventLogSource + " Foi INICIADO.", logEmail);
            logEmail = null;
            smtp = null;
                      
            //GetInformacoesSAA();
        }



        // ********************************************************************************************************* //
        //                                    B I O M E T R I C      I T                     (www.biometric-it.com)  //
        // --------------------------------------------------------------------------------------------------------- //
        //  @Autor: MAURICIO XAVIER                                                                                  //
        //  @Data:  19/05/2020                                                                                       //
        //  @Descricao:                                                                                              //
        // ********************************************************************************************************* //
        public void OnTimer(object sender, ElapsedEventArgs args)
        {
           GetInformacoesSAA();
        }


        // ********************************************************************************************************* //
        //                                    B I O M E T R I C      I T                     (www.biometric-it.com)  //
        // --------------------------------------------------------------------------------------------------------- //
        //  @Autor: MAURICIO XAVIER                                                                                  //
        //  @Data:  19/05/2020                                                                                       //
        //  @Descricao:                                                                                              //
        // ********************************************************************************************************* //
        protected override void OnStop()
        {
            // Update the service state to Stop Pending.
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOP_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);


            CPSeventLog.WriteEntry("Serviço parado...", EventLogEntryType.Information, eventId++);

            SMTPService smtp = new SMTPService();
            logEmail = new List<string>();
            logEmail.Add("O Serviço de aquisição de Dados CPA_ANALISEQUIMICA foi PARADO em " + DateTime.Now.ToShortDateString() + " as " + DateTime.Now.ToShortTimeString() + " no servidor [" + Dns.GetHostName() + "]");
            smtp.EmailResumoAquisicaoDados(Utils.Constantes.EventLogSource + " Foi PARADO.", logEmail);
            logEmail = null;
            smtp = null;

            // Update the service state to Stopped.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOPPED;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

        }

        // ********************************************************************************************************* //
        //                                    B I O M E T R I C      I T                     (www.biometric-it.com)  //
        // --------------------------------------------------------------------------------------------------------- //
        //  @Autor: MAURICIO XAVIER                                                                                  //
        //  @Data:  19/05/2020                                                                                       //
        //  @Descricao:                                                                                              //
        // ********************************************************************************************************* //
        protected override void OnContinue()
        {
            CPSeventLog.WriteEntry("Serviço Continuando...", EventLogEntryType.Information, eventId++);

            SMTPService smtp = new SMTPService();
            logEmail = new List<string>();
            logEmail.Add("O Serviço de aquisição de Dados CPA_ANALISEQUIMICA foi CONTINUADO em " + DateTime.Now.ToShortDateString() + " as " + DateTime.Now.ToShortTimeString() + " no servidor [" + Dns.GetHostName() + "]");
            smtp.EmailResumoAquisicaoDados(Utils.Constantes.EventLogSource + " Foi CONTINUADO.", logEmail);
            logEmail = null;
            smtp = null;
        }


        // ********************************************************************************************************* //
        //                                    B I O M E T R I C      I T                     (www.biometric-it.com)  //
        // --------------------------------------------------------------------------------------------------------- //
        //  @Autor: MAURICIO XAVIER                                                                                  //
        //  @Data:  19/05/2020                                                                                       //
        //  @Descricao:                                                                                              //
        // ********************************************************************************************************* //
        protected override void OnPause()
        {
            CPSeventLog.WriteEntry("Serviço Pausado...", EventLogEntryType.Information, eventId++);

            SMTPService smtp = new SMTPService();
            logEmail = new List<string>();
            logEmail.Add("O Serviço de aquisição de Dados CPA_ANALISEQUIMICA foi PAUSADO em " + DateTime.Now.ToShortDateString() + " as " + DateTime.Now.ToShortTimeString() + " no servidor [" + Dns.GetHostName() + "]");
            smtp.EmailResumoAquisicaoDados(Utils.Constantes.EventLogSource + " Foi PAUSADO.", logEmail);
            logEmail = null;
            smtp = null;
        }


    }
}
