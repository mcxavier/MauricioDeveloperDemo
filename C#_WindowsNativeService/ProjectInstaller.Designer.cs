namespace CPS_AnaliseQuimica
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Designer de Componentes

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.CPA_AnaliseQuimicaProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.CPA_AnaliseQuimicaInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // CPA_AnaliseQuimicaProcessInstaller
            // 
            this.CPA_AnaliseQuimicaProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.CPA_AnaliseQuimicaProcessInstaller.Password = null;
            this.CPA_AnaliseQuimicaProcessInstaller.Username = null;
            // 
            // CPA_AnaliseQuimicaInstaller
            // 
            this.CPA_AnaliseQuimicaInstaller.Description = "Serviço de Aquisição de Dados da Análise Química";
            this.CPA_AnaliseQuimicaInstaller.DisplayName = "MES-CPA Análise Química";
            this.CPA_AnaliseQuimicaInstaller.ServiceName = "CPA_AnaliseQuimica";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.CPA_AnaliseQuimicaProcessInstaller,
            this.CPA_AnaliseQuimicaInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller CPA_AnaliseQuimicaProcessInstaller;
        private System.ServiceProcess.ServiceInstaller CPA_AnaliseQuimicaInstaller;
    }
}