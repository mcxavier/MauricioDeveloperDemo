using PortalParceiroLibrary.BO.Salesforce;
using PortalParceiroLibrary.DAO.Salesforce;
using PortalParceiroLibrary.BO;
using PortalParceiroLibrary.BO.BancoDeDados;
using System;
using System.Collections.Generic;

namespace PortalParceiroLibrary.CO.Salesforce
{
    public class SincronizacaoSalesforceCO
    {
        private readonly SincronizacaoSalesforceDao _sincronizacaoSalesforceDAO;

        public SincronizacaoSalesforceCO()
        {
            _sincronizacaoSalesforceDAO = new SincronizacaoSalesforceDao();
        }

        public enum IdentClass
        {
            Usuario = 1,
            Hiperador = 2,
            Cliente = 3,
            Contrato = 4,
            GrupoAtendimento = 5
        }

        public enum IdentAction
        {
            Insert = 1,
            Update = 2,
            Delete = 3
        }


        public int AddRegistroSincronismo(BancoDadosBaseBO db, IdentClass identClass, int idRegistro, IdentAction identAction, DateTime dataAction, string jsonFile) => 
            _sincronizacaoSalesforceDAO.AddRegistroSincronismo(db, ((int)identClass), idRegistro, ((int)identAction), dataAction, jsonFile);

        public int RemoveSincronismoRealizado(BancoDadosBaseBO db, int scheduleId) => _sincronizacaoSalesforceDAO.DeleteRegistroSincronismo(db, scheduleId);

        public int LogarSincronismoNaoCompletado(BancoDadosBaseBO db, int scheduleId, string messageErr) => _sincronizacaoSalesforceDAO.LogRegistroSincronismo(db, scheduleId, messageErr);

        public ICollection<SincronismoSalesforceBO> SincronismosProgramados(BancoDadosBO db) => _sincronizacaoSalesforceDAO.Listar(db);

        public SincronismoSalesforceBO GetSincronismo(BancoDadosBaseBO db, int scheduleId) => _sincronizacaoSalesforceDAO.Retornar(db, scheduleId);

    }
}
