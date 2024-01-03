using PortalParceiro.Configuration;
using PortalParceiroHangfire.SchedulesLogger;
using PortalParceiroLibrary.BO;
using PortalParceiroLibrary.BO.Salesforce;
using PortalParceiroLibrary.CO;
using PortalParceiroLibrary.CO.Contratacao;
using PortalParceiroLibrary.CO.GrupoAtendimento;
using PortalParceiroLibrary.CO.Salesforce;
using SalesforceSharp;
using SalesforceSharp.Security;
using System;
using System.Text.Json;
using static PortalParceiroLibrary.CO.Salesforce.SincronizacaoSalesforceCO;

namespace PortalParceiroLibrary.Services.SincronizacaoSalesforce
{
    public class SincronizacaoSalesforceService : ISincronizacaoSalesforceService
    {
        private readonly SincronizacaoSalesforceCO _sincronizacaoSalesforceCO;
        private readonly ContratacaoCO _contratacaoCO;
        private SalesforceClient client;
        

        public SincronizacaoSalesforceService(SincronizacaoSalesforceCO sincronizacaoSalesforceCO, ContratacaoCO contratacaoCO)
        {
            _sincronizacaoSalesforceCO = sincronizacaoSalesforceCO;
            _contratacaoCO = contratacaoCO;
        }



        private void InsertGrupoAntendimento(BancoDadosBO db, int scheduleId, GrupoAtendimentoSalesforceBO grupoAtendimento)
        {
            client.Create("Account", new GrupoAtendimentoSalesforceDto()
            {
                Description = grupoAtendimento.Description
            });

            var records = client.Query<GrupoAtendimentoSalesforceDto>(
                String.Format("SELECT id, name, description, RecordTypeId  FROM Account where name = '{0}'", grupoAtendimento.Description)
                );

            if (records.Count > 0)
            {
                GrupoAtendimentoCO.AlterarIdSalesforce(db, grupoAtendimento.IdClass, records[0].Id);
                _sincronizacaoSalesforceCO.RemoveSincronismoRealizado(db, scheduleId);
            }
            else
            {
                _sincronizacaoSalesforceCO.LogarSincronismoNaoCompletado(db, scheduleId,
                    String.Format("Erro na INCLUSÃO do ID [{0}]: Não foi possivel recuperar o Id_Salesforce!", grupoAtendimento.Id));
            }
        }


        private void InsertContrato(BancoDadosBO db, int scheduleId, ContratoSalesforceBO contrato)
        {
            client.Create("Contract", new ContratoSalesforceDto()
            {
                Description = contrato.Description,
                Plano__c = contrato.Plano__c,
                Valor_Assinatura__c = contrato.Valor_Assinatura__c,
                Produto__c = contrato.Produto__c
            });

            var records = client.Query<ContratoSalesforceDto>(
                String.Format("select id, description, accountId, Valor_Assinatura__c, Plano__c, Produto__c from contract  where id = '{0}'", contrato.Id)
                );

            if (records.Count > 0)
            {
                _contratacaoCO.AlterarIdSalesforce(db, contrato.IdClass, records[0].Id);
                _sincronizacaoSalesforceCO.RemoveSincronismoRealizado(db, scheduleId);
            }
            else
            {
                _sincronizacaoSalesforceCO.LogarSincronismoNaoCompletado(db, scheduleId,
                    String.Format("Erro na INCLUSÃO do ID [{0}]: Não foi possivel recuperar o Id_Salesforce!", contrato.Id));
            }
        }


        private void InsertCliente(BancoDadosBO db, int scheduleId, ClienteSalesforceBO cliente)
        {

            client.Create("Account", new ClienteSalesforceDto()
            {
                Description = cliente.Description,
                Name = cliente.Name,
                Atendimento_Hiper__c = cliente.Atendimento_Hiper__c,
                Bairro__c = cliente.Bairro__c,
                CNAE__c = cliente.CNAE__c,
                CNPJ__c = cliente.CNPJ__c,
                Complemento__c = cliente.Complemento__c,
                DDD__c = cliente.DDD__c,
                E_mail_c__c = cliente.E_mail_c__c,
                ID_Cliente__c = cliente.ID_Cliente__c,
                Inscricao_Estadual__c = cliente.Inscricao_Estadual__c,
                Nome_Fantasia__c = cliente.Nome_Fantasia__c,
                Numero__c = cliente.Numero__c,
                Phone = cliente.Phone,
                Prius__c = cliente.Prius__c,
                Segmento__c = cliente.Segmento__c
            });

            var records = client.Query<ClienteSalesforceDto>(
                String.Format("SELECT id, name, description, RecordTypeId  FROM Account where name = '{0}'", cliente.Name)
                );

            if (records.Count > 0)
            {
                ClienteCO.AlterarIdSalesforce(db, cliente.IdClass, records[0].Id);
                _sincronizacaoSalesforceCO.RemoveSincronismoRealizado(db, scheduleId);
            }
            else
            {
                _sincronizacaoSalesforceCO.LogarSincronismoNaoCompletado(db, scheduleId,
                    String.Format("Erro na INCLUSÃO do ID [{0}]: Não foi possivel recuperar o Id_Salesforce!", cliente.Id));
            }

        }


        private void InsertUsuario(BancoDadosBO db, int scheduleId, UsuarioSalesforceBO usuario)
        {
            client.Create("Account", new UsuarioSalesforceDto()
            {
                Description = usuario.Description,
                Name = usuario.Name,
                Atendimento_Hiper__c = usuario.Atendimento_Hiper__c,
                Bairro__c = usuario.Bairro__c,
                CNAE__c = usuario.CNAE__c,
                CNPJ__c = usuario.CNPJ__c,
                Complemento__c = usuario.Complemento__c,
                DDD__c = usuario.DDD__c,
                E_mail_c__c = usuario.E_mail_c__c,
                ID_Cliente__c = usuario.ID_Cliente__c,
                Inscricao_Estadual__c = usuario.Inscricao_Estadual__c,
                Nome_Fantasia__c = usuario.Nome_Fantasia__c,
                Numero__c = usuario.Numero__c,
                Phone = usuario.Phone,
                Prius__c = usuario.Prius__c,
                Segmento__c = usuario.Segmento__c
            });

            var records = client.Query<UsuarioSalesforceDto>(
                String.Format("SELECT id, name, description, RecordTypeId  FROM Account where name = '{0}'", usuario.Name)
                );

            if (records.Count > 0)
            {
                UsuarioCO.AlterarIdSalesforce(db, usuario.IdClass, records[0].Id);
                _sincronizacaoSalesforceCO.RemoveSincronismoRealizado(db, scheduleId);
            }
            else
            {
                _sincronizacaoSalesforceCO.LogarSincronismoNaoCompletado(db, scheduleId,
                    String.Format("Erro na INCLUSÃO do ID [{0}]: Não foi possivel recuperar o Id_Salesforce!", usuario.Id));
            }
        }


        private void InsertHiperador(BancoDadosBO db, int scheduleId, HiperadorSalesforceBO hiperador)
        {
            client.Create("Account", new HiperadorSalesforceDto()
            {
                Description = hiperador.Description,
                Name = hiperador.Name,
                Atendimento_Hiper__c = hiperador.Atendimento_Hiper__c,
                Bairro__c = hiperador.Bairro__c,
                CNAE__c = hiperador.CNAE__c,
                CNPJ__c = hiperador.CNPJ__c,
                Complemento__c = hiperador.Complemento__c,
                DDD__c = hiperador.DDD__c,
                E_mail_c__c = hiperador.E_mail_c__c,
                ID_Cliente__c = hiperador.ID_Cliente__c,
                Inscricao_Estadual__c = hiperador.Inscricao_Estadual__c,
                Nome_Fantasia__c = hiperador.Nome_Fantasia__c,
                Numero__c = hiperador.Numero__c,
                Phone = hiperador.Phone,
                Prius__c = hiperador.Prius__c,
                Segmento__c = hiperador.Segmento__c
            });

            var records = client.Query<HiperadorSalesforceDto>(
                String.Format("SELECT id, name, description, RecordTypeId  FROM Account where name = '{0}'", hiperador.Name)
                );

            if (records.Count > 0)
            {
                ParceiroCO.AtualizarIdSalesforce(db, hiperador.IdClass, records[0].Id);
                _sincronizacaoSalesforceCO.RemoveSincronismoRealizado(db, scheduleId);
            }
            else
            {
                _sincronizacaoSalesforceCO.LogarSincronismoNaoCompletado(db, scheduleId,
                    String.Format("Erro na INCLUSÃO do ID [{0}]: Não foi possivel recuperar o Id_Salesforce!", hiperador.Id));
            }
        }

        private void InsertSalesforce(BancoDadosBO db, int scheduleId, SalesforceDto salesforceDto)
        {
            switch (salesforceDto.GetType().Name)
            {
                case "GrupoAtendimentoSalesforceBO":
                    {
                        try
                        {
                            InsertGrupoAntendimento(db, scheduleId, ((GrupoAtendimentoSalesforceBO)salesforceDto));
                        }
                        catch (Exception ex)
                        {
                            _sincronizacaoSalesforceCO.LogarSincronismoNaoCompletado(db, scheduleId,
                            String.Format("Erro na INCLUSÃO do ID [{0}]: {1}!", ((GrupoAtendimentoSalesforceBO)salesforceDto).Id, ex.Message));
                        }
                        break;
                    }
                case "ContratoSalesforceBO":
                    {
                        try
                        {
                            InsertContrato(db, scheduleId, ((ContratoSalesforceBO)salesforceDto));
                        }
                        catch (Exception ex)
                        {
                            _sincronizacaoSalesforceCO.LogarSincronismoNaoCompletado(db, scheduleId,
                            String.Format("Erro na INCLUSÃO do ID [{0}]: {1}!", ((ContratoSalesforceBO)salesforceDto).Id, ex.Message));
                        }
                        break;
                    }
                case "ClienteSalesforceBO":
                    {
                        try
                        {
                            InsertCliente(db, scheduleId, ((ClienteSalesforceBO)salesforceDto));
                        }
                        catch (Exception ex)
                        {
                            _sincronizacaoSalesforceCO.LogarSincronismoNaoCompletado(db, scheduleId,
                            String.Format("Erro na INCLUSÃO do ID [{0}]: {1}!", ((ClienteSalesforceBO)salesforceDto).Id, ex.Message));
                        }
                        break;
                    }
                case "UsuarioSalesforceBO":
                    {
                        try
                        {
                            InsertUsuario(db, scheduleId, ((UsuarioSalesforceBO)salesforceDto));
                        }
                        catch (Exception ex)
                        {
                            _sincronizacaoSalesforceCO.LogarSincronismoNaoCompletado(db, scheduleId,
                            String.Format("Erro na INCLUSÃO do ID [{0}]: {1}!", ((UsuarioSalesforceBO)salesforceDto).Id, ex.Message));
                        }
                        break;
                    }
                case "HiperadorSalesforceBO":
                    {
                        try
                        {
                            InsertHiperador(db, scheduleId, ((HiperadorSalesforceBO)salesforceDto));
                        }
                        catch (Exception ex)
                        {
                            _sincronizacaoSalesforceCO.LogarSincronismoNaoCompletado(db, scheduleId,
                            String.Format("Erro na INCLUSÃO do ID [{0}]: {1}!", ((HiperadorSalesforceBO)salesforceDto).Id, ex.Message));
                        }
                        break;
                    }
            }

        }



        private void UpdateGrupoAntendimento(BancoDadosBO db, int scheduleId, GrupoAtendimentoSalesforceBO grupoAtendimento)
        {
            if (client.Update("Account", grupoAtendimento.Id, new GrupoAtendimentoSalesforceDto()
            {
                Description = grupoAtendimento.Description
            }))
                _sincronizacaoSalesforceCO.RemoveSincronismoRealizado(db, scheduleId);
            else
                _sincronizacaoSalesforceCO.LogarSincronismoNaoCompletado(db, scheduleId,
                    String.Format("Não foi possível ATUALIZAR o GRUPO DE ATENDIMENTO com ID [{0}]!", grupoAtendimento.Id));
        }


        private void UpdateContrato(BancoDadosBO db, int scheduleId, ContratoSalesforceBO contrato)
        {
            if (client.Update("Contract", contrato.Id, new ContratoSalesforceDto()
            {
                accountId = contrato.accountId,
                Description = contrato.Description,
                Plano__c = contrato.Plano__c,
                Produto__c = contrato.Produto__c,
                Valor_Assinatura__c = contrato.Valor_Assinatura__c
            }))
                _sincronizacaoSalesforceCO.RemoveSincronismoRealizado(db, scheduleId);
            else
                _sincronizacaoSalesforceCO.LogarSincronismoNaoCompletado(db, scheduleId,
                    String.Format("Não foi possível ATUALIZAR o CONTRATO com ID [{0}]!", contrato.Id));
        }



        private void UpdateCliente(BancoDadosBO db, int scheduleId, ClienteSalesforceBO cliente)
        {
            if (client.Update("Account", cliente.Id, new ClienteSalesforceDto()
            {
                Description = cliente.Description,
                Name = cliente.Name,
                Atendimento_Hiper__c = cliente.Atendimento_Hiper__c,
                Bairro__c = cliente.Bairro__c,
                CNAE__c = cliente.CNAE__c,
                CNPJ__c = cliente.CNPJ__c,
                Complemento__c = cliente.Complemento__c,
                DDD__c = cliente.DDD__c,
                E_mail_c__c = cliente.E_mail_c__c,
                ID_Cliente__c = cliente.ID_Cliente__c,
                Inscricao_Estadual__c = cliente.Inscricao_Estadual__c,
                Nome_Fantasia__c = cliente.Nome_Fantasia__c,
                Numero__c = cliente.Numero__c,
                Phone = cliente.Phone,
                Prius__c = cliente.Prius__c,
                Segmento__c = cliente.Segmento__c
            }))
                _sincronizacaoSalesforceCO.RemoveSincronismoRealizado(db, scheduleId);
            else
                _sincronizacaoSalesforceCO.LogarSincronismoNaoCompletado(db, scheduleId,
                    String.Format("Não foi possível ATUALIZAR o CLIENTE com ID [{0}]!", cliente.Id));
        }


        private void UpdateUsuario(BancoDadosBO db, int scheduleId, UsuarioSalesforceBO usuario)
        {
            if (client.Update("Account", usuario.Id, new UsuarioSalesforceDto()
            {
                Description = usuario.Description,
                Name = usuario.Name,
                Atendimento_Hiper__c = usuario.Atendimento_Hiper__c,
                Bairro__c = usuario.Bairro__c,
                CNAE__c = usuario.CNAE__c,
                CNPJ__c = usuario.CNPJ__c,
                Complemento__c = usuario.Complemento__c,
                DDD__c = usuario.DDD__c,
                E_mail_c__c = usuario.E_mail_c__c,
                ID_Cliente__c = usuario.ID_Cliente__c,
                Inscricao_Estadual__c = usuario.Inscricao_Estadual__c,
                Nome_Fantasia__c = usuario.Nome_Fantasia__c,
                Numero__c = usuario.Numero__c,
                Phone = usuario.Phone,
                Prius__c = usuario.Prius__c,
                Segmento__c = usuario.Segmento__c
            }))
                _sincronizacaoSalesforceCO.RemoveSincronismoRealizado(db, scheduleId);
            else
                _sincronizacaoSalesforceCO.LogarSincronismoNaoCompletado(db, scheduleId,
                    String.Format("Não foi possível ATUALIZAR o USUÁRIO com ID [{0}]!", usuario.Id));
        }


        private void UpdateHiperador(BancoDadosBO db, int scheduleId, HiperadorSalesforceBO hiperador)
        {
            if (client.Update("Account", hiperador.Id, new HiperadorSalesforceDto()
            {
                Description = hiperador.Description,
                Name = hiperador.Name,
                Atendimento_Hiper__c = hiperador.Atendimento_Hiper__c,
                Bairro__c = hiperador.Bairro__c,
                CNAE__c = hiperador.CNAE__c,
                CNPJ__c = hiperador.CNPJ__c,
                Complemento__c = hiperador.Complemento__c,
                DDD__c = hiperador.DDD__c,
                E_mail_c__c = hiperador.E_mail_c__c,
                ID_Cliente__c = hiperador.ID_Cliente__c,
                Inscricao_Estadual__c = hiperador.Inscricao_Estadual__c,
                Nome_Fantasia__c = hiperador.Nome_Fantasia__c,
                Numero__c = hiperador.Numero__c,
                Phone = hiperador.Phone,
                Prius__c = hiperador.Prius__c,
                Segmento__c = hiperador.Segmento__c
            }))
                _sincronizacaoSalesforceCO.RemoveSincronismoRealizado(db, scheduleId);
            else
                _sincronizacaoSalesforceCO.LogarSincronismoNaoCompletado(db, scheduleId,
                    String.Format("Não foi possível ATUALIZAR o HIPERADOR com ID [{0}]!", hiperador.Id));
        }



        private void UpdateSalesforce(BancoDadosBO db, int scheduleId, SalesforceDto salesforceDto)
        {
            switch (salesforceDto.GetType().Name)
            {
                case "GrupoAtendimentoSalesforceBO":
                    {
                        try
                        {
                            UpdateGrupoAntendimento(db, scheduleId, ((GrupoAtendimentoSalesforceBO)salesforceDto));
                        }
                        catch (Exception ex)
                        {
                            _sincronizacaoSalesforceCO.LogarSincronismoNaoCompletado(db, scheduleId,
                            String.Format("Erro na ATUALIZAÇÃO do ID [{0}]: {1}!", ((GrupoAtendimentoSalesforceBO)salesforceDto).Id, ex.Message));
                        }

                        break;
                    }
                case "ContratoSalesforceBO":
                    {
                        try
                        {
                            UpdateContrato(db, scheduleId, ((ContratoSalesforceBO)salesforceDto));
                        }
                        catch (Exception ex)
                        {
                            _sincronizacaoSalesforceCO.LogarSincronismoNaoCompletado(db, scheduleId,
                            String.Format("Erro na ATUALIZAÇÃO do ID [{0}]: {1}!", ((ContratoSalesforceBO)salesforceDto).Id, ex.Message));
                        }
                        break;
                    }
                case "ClienteSalesforceBO":
                    {
                        try
                        {
                            UpdateCliente(db, scheduleId, ((ClienteSalesforceBO)salesforceDto));
                        }
                        catch (Exception ex)
                        {
                            _sincronizacaoSalesforceCO.LogarSincronismoNaoCompletado(db, scheduleId,
                            String.Format("Erro na ATUALIZAÇÃO do ID [{0}]: {1}!", ((ClienteSalesforceBO)salesforceDto).Id, ex.Message));
                        }
                        break;
                    }
                case "UsuarioSalesforceBO":
                    {
                        try
                        {
                            UpdateUsuario(db, scheduleId, ((UsuarioSalesforceBO)salesforceDto));
                        }
                        catch (Exception ex)
                        {
                            _sincronizacaoSalesforceCO.LogarSincronismoNaoCompletado(db, scheduleId,
                            String.Format("Erro na ATUALIZAÇÃO do ID [{0}]: {1}!", ((UsuarioSalesforceBO)salesforceDto).Id, ex.Message));
                        }
                        break;
                    }
                case "HiperadorSalesforceBO":
                    {
                        try
                        {
                            UpdateHiperador(db, scheduleId, ((HiperadorSalesforceBO)salesforceDto));
                        }
                        catch (Exception ex)
                        {
                            _sincronizacaoSalesforceCO.LogarSincronismoNaoCompletado(db, scheduleId,
                            String.Format("Erro na ATUALIZAÇÃO do ID [{0}]: {1}!", ((HiperadorSalesforceBO)salesforceDto).Id, ex.Message));
                        }
                        break;
                    }
            }

        }


        private void DeleteGrupoAtendimento(BancoDadosBO db, int scheduleId, GrupoAtendimentoSalesforceBO grupoAtendimento)
        {
            if (client.Delete("Account", grupoAtendimento.Id))
                _sincronizacaoSalesforceCO.RemoveSincronismoRealizado(db, scheduleId);
            else
                _sincronizacaoSalesforceCO.LogarSincronismoNaoCompletado(db, scheduleId,
                    String.Format("Não foi possível EXCLUIR o GRUPO DE ATENDIMENTO com ID [{0}]!", grupoAtendimento.Id));
        }



        private void DeleteContrato(BancoDadosBO db, int scheduleId, ContratoSalesforceBO contrato)
        {
            if (client.Delete("Contract", contrato.Id))
                _sincronizacaoSalesforceCO.RemoveSincronismoRealizado(db, scheduleId);
            else
                _sincronizacaoSalesforceCO.LogarSincronismoNaoCompletado(db, scheduleId,
                    String.Format("Não foi possível EXCLUIR o CONTRATO com ID [{0}]!", contrato.Id));
        }


        private void DeleteCliente(BancoDadosBO db, int scheduleId, ClienteSalesforceBO cliente)
        {
            if (client.Delete("Account", cliente.Id))
                _sincronizacaoSalesforceCO.RemoveSincronismoRealizado(db, scheduleId);
            else
                _sincronizacaoSalesforceCO.LogarSincronismoNaoCompletado(db, scheduleId,
                    String.Format("Não foi possível EXCLUIR o CLIENTE com ID [{0}]!", cliente.Id));
        }


        private void DeleteUsuario(BancoDadosBO db, int scheduleId, UsuarioSalesforceBO usuario)
        {
            if (client.Delete("Account", usuario.Id))
                _sincronizacaoSalesforceCO.RemoveSincronismoRealizado(db, scheduleId);
            else
                _sincronizacaoSalesforceCO.LogarSincronismoNaoCompletado(db, scheduleId,
                    String.Format("Não foi possível EXCLUIR o USUÁRIO com ID [{0}]!", usuario.Id));
        }


        private void DeleteHiperador(BancoDadosBO db, int scheduleId, HiperadorSalesforceBO hiperador)
        {
            if (client.Delete("Account", hiperador.Id))
                _sincronizacaoSalesforceCO.RemoveSincronismoRealizado(db, scheduleId);
            else
                _sincronizacaoSalesforceCO.LogarSincronismoNaoCompletado(db, scheduleId,
                    String.Format("Não foi possível EXCLUIR o HIPERADOR com ID [{0}]!", hiperador.Id));
        }



        private void DeleteSalesforce(BancoDadosBO db, int scheduleId, SalesforceDto salesforceDto)
        {
            switch (salesforceDto.GetType().Name)
            {
                case "GrupoAtendimentoSalesforceBO":
                    {
                        try
                        {
                            DeleteGrupoAtendimento(db, scheduleId, ((GrupoAtendimentoSalesforceBO)salesforceDto));
                        }
                        catch (Exception ex)
                        {
                            _sincronizacaoSalesforceCO.LogarSincronismoNaoCompletado(db, scheduleId,
                            String.Format("Erro na EXCLUSÃO do ID [{0}]: {1}!", ((GrupoAtendimentoSalesforceBO)salesforceDto).Id, ex.Message));
                        }
                        break;
                    }
                case "ContratoSalesforceBO":
                    {
                        try
                        {
                            DeleteContrato(db, scheduleId, ((ContratoSalesforceBO)salesforceDto));
                        }
                        catch (Exception ex)
                        {
                            _sincronizacaoSalesforceCO.LogarSincronismoNaoCompletado(db, scheduleId,
                            String.Format("Erro na EXCLUSÃO do ID [{0}]: {1}!", ((ContratoSalesforceBO)salesforceDto).Id, ex.Message));
                        }
                        break;
                    }
                case "ClienteSalesforceBO":
                    {
                        try
                        {
                            DeleteCliente(db, scheduleId, ((ClienteSalesforceBO)salesforceDto));
                        }
                        catch (Exception ex)
                        {
                            _sincronizacaoSalesforceCO.LogarSincronismoNaoCompletado(db, scheduleId,
                            String.Format("Erro na EXCLUSÃO do ID [{0}]: {1}!", ((ClienteSalesforceBO)salesforceDto).Id, ex.Message));
                        }
                        break;
                    }
                case "UsuarioSalesforceBO":
                    {
                        try
                        {
                            DeleteUsuario(db, scheduleId, ((UsuarioSalesforceBO)salesforceDto));
                        }
                        catch (Exception ex)
                        {
                            _sincronizacaoSalesforceCO.LogarSincronismoNaoCompletado(db, scheduleId,
                            String.Format("Erro na EXCLUSÃO do ID [{0}]: {1}!", ((UsuarioSalesforceBO)salesforceDto).Id, ex.Message));
                        }
                        break;
                    }
                case "HiperadorSalesforceBO":
                    {
                        try
                        {
                            DeleteHiperador(db, scheduleId, ((HiperadorSalesforceBO)salesforceDto));
                        }
                        catch (Exception ex)
                        {
                            _sincronizacaoSalesforceCO.LogarSincronismoNaoCompletado(db, scheduleId,
                            String.Format("Erro na ATUALIZAÇÃO do ID [{0}]: {1}!", ((HiperadorSalesforceBO)salesforceDto).Id, ex.Message));
                        }
                        break;
                    }
            }

        }




        private void Sincronizar(BancoDadosBO db, int scheduleId, IdentAction action, SalesforceDto salesforceDto)
        {
            switch (action)
            {
                case IdentAction.Insert:
                    InsertSalesforce(db, scheduleId, salesforceDto);
                    break;
                case IdentAction.Update:
                    UpdateSalesforce(db, scheduleId, salesforceDto);
                    break;
                case IdentAction.Delete:
                    DeleteSalesforce(db, scheduleId, salesforceDto);
                    break;
            }
        }






        void ISincronizacaoSalesforceService.SincronizaSalesForce(BancoDadosBO db, IScheduleLogger logger)
        {
            ConfiguracoesDto config = ConfiguracaoManager.CarregarConfiguracao();
            UsernamePasswordAuthenticationFlow authFlow;

            var registros = _sincronizacaoSalesforceCO.SincronismosProgramados(db);

            if (registros.Count > 0 )
            {               
                client = new SalesforceClient();
                authFlow = new UsernamePasswordAuthenticationFlow(
                    config.Salesforce.ClientId,
                    config.Salesforce.ClientSecret,
                    config.Salesforce.UserName,
                    config.Salesforce.Password
                    );

                authFlow.TokenRequestEndpointUrl = config.Salesforce.TokenRequestEndpointUrl;

                try
                {
                    this.client.Authenticate(authFlow);

                    foreach (SincronismoSalesforceBO reg in registros)
                    {
                        switch (reg.IdentClass)
                        {
                            case (int)IdentClass.Usuario:
                                {
                                    var usuario = JsonSerializer.Deserialize<UsuarioSalesforceBO>(reg.Json);
                                    Sincronizar(db, reg.Id, (IdentAction)reg.IdentAction, usuario);
                                    break;
                                }
                            case (int)IdentClass.Hiperador:
                                {
                                    var hiperador = JsonSerializer.Deserialize<HiperadorSalesforceBO>(reg.Json);
                                    Sincronizar(db, reg.Id, (IdentAction)reg.IdentAction, hiperador);
                                    break;
                                }
                            case (int)IdentClass.Cliente:
                                {
                                    var cliente = JsonSerializer.Deserialize<ClienteSalesforceBO>(reg.Json);
                                    Sincronizar(db, reg.Id, (IdentAction)reg.IdentAction, cliente);
                                    break;
                                }
                            case (int)IdentClass.Contrato:
                                {
                                    var contrato = JsonSerializer.Deserialize<ContratoSalesforceBO>(reg.Json);
                                    Sincronizar(db, reg.Id, (IdentAction)reg.IdentAction, contrato);
                                    break;
                                }
                            case (int)IdentClass.GrupoAtendimento:
                                {
                                    var grupoAtendimento = JsonSerializer.Deserialize<GrupoAtendimentoSalesforceBO>(reg.Json);
                                    Sincronizar(db, reg.Id, (IdentAction)reg.IdentAction, grupoAtendimento);
                                    break;
                                }
                        }

                    }

                }
                catch (Exception ex)
                {
                    throw new SalesforceException(SalesforceError.AuthenticationFailure, (String.Format("Salesforce Authentication failed: {0}", ex.Message)));
                }

            }



        }


    }
}
