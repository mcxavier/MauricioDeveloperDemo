using PortalParceiroLibrary.BO;
using PortalParceiroLibrary.BO.BancoDeDados;
using PortalParceiroLibrary.BO.Schedule;
using PortalParceiroLibrary.DAO.Schedule;
using System;
using System.Collections.Generic;

namespace PortalParceiroLibrary.CO.Schedule
{
    public class ScheduleCO
    {
        private readonly ScheduleDAO _scheduleDAO;

        public ScheduleCO()
        {
            _scheduleDAO = new ScheduleDAO();
        }

        public enum Schedules
        {
            GestaoDeClientes = 1,
            AtualizacaoDeCobrancasEmAberto = 2,
            ConsultaDeNovasCobrancas = 3,
            CancelamentoDeLicencas = 4,
            CalculadoraDePrecos = 5,
            AtualizacaoDeContasCanceladas = 6,
            AtribuicaoDeAtividadesDoRadar = 7,
            AtribuicaoDeLeads = 8,
            AtualizacaoDaDataDeVigencia = 9,
            SincronizacaoClienteECobrancasSuperlogica = 10,
            SincronizadorDeCobrancasSuperlogica = 11,
            RenovacaoDeContratos = 12,
            GeracaoLoteDeCobrancas = 13,
            SincronismoSalesforce = 14
        }

        public int AtualizarUltimaExecucao(BancoDadosBaseBO db, int scheduleId, DateTime ultimaExecucao) => _scheduleDAO.AtualizarUltimaExecucao(db, scheduleId, ultimaExecucao);

        public ICollection<ScheduleBO> Listar(BancoDadosBO db) => _scheduleDAO.Listar(db);

        public ScheduleBO Retornar(BancoDadosBaseBO db, int scheduleId) => _scheduleDAO.Retornar(db, scheduleId);

        public bool TodasAsSchedulesRodaramHoje(BancoDadosBO db) => _scheduleDAO.TodasAsSchedulesRodaramHoje(db);
    }
}