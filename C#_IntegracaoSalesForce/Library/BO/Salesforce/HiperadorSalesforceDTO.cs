﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalParceiroLibrary.BO.Salesforce
{
    public class HiperadorSalesforceDto: SalesforceDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string RecordTypeId { get; set; }
        public string Atendimento_Hiper__c { get; set; }
        public string Bairro__c { get; set; }
        public string CNAE__c { get; set; }
        public string CNPJ__c { get; set; }
        public string Complemento__c { get; set; }
        public string DDD__c { get; set; }
        public string E_mail_c__c { get; set; }
        public string ID_Cliente__c { get; set; }
        public string Inscricao_Estadual__c { get; set; }
        public string Nome_Fantasia__c { get; set; }
        public string Numero__c { get; set; }
        public string Phone { get; set; }
        public string Prius__c { get; set; }
        public string Segmento__c { get; set; }


        public HiperadorSalesforceDto()
        {
            this.RecordTypeId = "0123t000000Ga16AAC";
        }
    }
}
