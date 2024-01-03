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
using CPS_Service_Lib.Utils;
using CPS_AnaliseQuimica.Utils;
using CPS_AnaliseQuimica.DataBase;

namespace CPS_AnaliseQuimica
{
    public partial class CPSAnaliseQuimica : ServiceBase
    {

        // HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services

        public EventLog CPSeventLog { get; private set; }
        private int eventId = 1;
        private SqlServerTools sql { get; set; }

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(System.IntPtr handle, ref ServiceStatus serviceStatus);


        public CPSAnaliseQuimica()
        {
            InitializeComponent();

            CPSeventLog = new EventLog();

            if (!EventLog.SourceExists(Utils.Constantes.EventLogSource))
            {
                EventLog.CreateEventSource(Utils.Constantes.EventLogSource, CPS_Service_Lib.Utils.Constantes.EventLogName);
            }

            CPSeventLog.Source = Utils.Constantes.EventLogSource;
            CPSeventLog.Log = CPS_Service_Lib.Utils.Constantes.EventLogName;

        }



        protected override void OnStart(string[] args)
        {
            CPSeventLog.WriteEntry("In OnStart.");

            // Update the service state to Start Pending.
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            if (sql == null)
            {
                try
                {
                    SqlServerTools sql = new SqlServerTools(CPSeventLog);

                } catch (Exception e)
                {
                    this.CPSeventLog.WriteEntry("CPS_AnaliseQuimica.OnStart: " + e.ToString());

                    // Update the service state to Running.
                    serviceStatus.dwCurrentState = ServiceState.SERVICE_STOPPED;
                    SetServiceStatus(this.ServiceHandle, ref serviceStatus);
                }
                
            }
         

            Timer timer = new Timer();
            timer.Interval = 60000; // 60 seconds
            timer.Elapsed += new ElapsedEventHandler(this.OnTimer);
            timer.Start();


            // Update the service state to Running.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

        }
        

        public void OnTimer(object sender, ElapsedEventArgs args)
        {
            // TODO: Insert monitoring activities here.
            CPSeventLog.WriteEntry("Monitoring the System", EventLogEntryType.Information, eventId++);
        }


        protected override void OnStop()
        {
            // Update the service state to Stop Pending.
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOP_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);


            CPSeventLog.WriteEntry("In OnStop.");


            // Update the service state to Stopped.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOPPED;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }

        protected override void OnContinue()
        {
            CPSeventLog.WriteEntry("In OnContinue.");
        }

    }
}
