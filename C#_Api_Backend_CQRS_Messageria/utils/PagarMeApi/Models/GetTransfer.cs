/*
 * PagarMeApi
 *
 * This file was automatically generated by APIMATIC v2.0 ( https://apimatic.io ).
 */
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PagarMeApi;
using PagarMeApi.Utilities;


namespace PagarMeApi.Models
{
    public class GetTransfer : BaseModel 
    {
        // These fields hold the values for the public properties.
        private string id;
        private string gatewayId;
        private int amount;
        private string status;
        private DateTime createdAt;
        private DateTime updatedAt;
        private Dictionary<string, string> metadata;
        private int? fee;
        private DateTime? fundingDate;
        private DateTime? fundingEstimatedDate;
        private string type;
        private Models.GetTransferSourceResponse source;
        private Models.GetTransferTargetResponse target;

        /// <summary>
        /// TODO: Write general description for this method
        /// </summary>
        [JsonProperty("id")]
        public string Id 
        { 
            get 
            {
                return this.id; 
            } 
            set 
            {
                this.id = value;
                onPropertyChanged("Id");
            }
        }

        /// <summary>
        /// TODO: Write general description for this method
        /// </summary>
        [JsonProperty("gateway_id")]
        public string GatewayId 
        { 
            get 
            {
                return this.gatewayId; 
            } 
            set 
            {
                this.gatewayId = value;
                onPropertyChanged("GatewayId");
            }
        }

        /// <summary>
        /// TODO: Write general description for this method
        /// </summary>
        [JsonProperty("amount")]
        public int Amount 
        { 
            get 
            {
                return this.amount; 
            } 
            set 
            {
                this.amount = value;
                onPropertyChanged("Amount");
            }
        }

        /// <summary>
        /// TODO: Write general description for this method
        /// </summary>
        [JsonProperty("status")]
        public string Status 
        { 
            get 
            {
                return this.status; 
            } 
            set 
            {
                this.status = value;
                onPropertyChanged("Status");
            }
        }

        /// <summary>
        /// TODO: Write general description for this method
        /// </summary>
        [JsonConverter(typeof(IsoDateTimeConverter))]
        [JsonProperty("created_at")]
        public DateTime CreatedAt 
        { 
            get 
            {
                return this.createdAt; 
            } 
            set 
            {
                this.createdAt = value;
                onPropertyChanged("CreatedAt");
            }
        }

        /// <summary>
        /// TODO: Write general description for this method
        /// </summary>
        [JsonConverter(typeof(IsoDateTimeConverter))]
        [JsonProperty("updated_at")]
        public DateTime UpdatedAt 
        { 
            get 
            {
                return this.updatedAt; 
            } 
            set 
            {
                this.updatedAt = value;
                onPropertyChanged("UpdatedAt");
            }
        }

        /// <summary>
        /// TODO: Write general description for this method
        /// </summary>
        [JsonProperty("metadata")]
        public Dictionary<string, string> Metadata 
        { 
            get 
            {
                return this.metadata; 
            } 
            set 
            {
                this.metadata = value;
                onPropertyChanged("Metadata");
            }
        }

        /// <summary>
        /// TODO: Write general description for this method
        /// </summary>
        [JsonProperty("fee")]
        public int? Fee 
        { 
            get 
            {
                return this.fee; 
            } 
            set 
            {
                this.fee = value;
                onPropertyChanged("Fee");
            }
        }

        /// <summary>
        /// TODO: Write general description for this method
        /// </summary>
        [JsonConverter(typeof(IsoDateTimeConverter))]
        [JsonProperty("funding_date")]
        public DateTime? FundingDate 
        { 
            get 
            {
                return this.fundingDate; 
            } 
            set 
            {
                this.fundingDate = value;
                onPropertyChanged("FundingDate");
            }
        }

        /// <summary>
        /// TODO: Write general description for this method
        /// </summary>
        [JsonConverter(typeof(IsoDateTimeConverter))]
        [JsonProperty("funding_estimated_date")]
        public DateTime? FundingEstimatedDate 
        { 
            get 
            {
                return this.fundingEstimatedDate; 
            } 
            set 
            {
                this.fundingEstimatedDate = value;
                onPropertyChanged("FundingEstimatedDate");
            }
        }

        /// <summary>
        /// TODO: Write general description for this method
        /// </summary>
        [JsonProperty("type")]
        public string Type 
        { 
            get 
            {
                return this.type; 
            } 
            set 
            {
                this.type = value;
                onPropertyChanged("Type");
            }
        }

        /// <summary>
        /// TODO: Write general description for this method
        /// </summary>
        [JsonProperty("source")]
        public Models.GetTransferSourceResponse Source 
        { 
            get 
            {
                return this.source; 
            } 
            set 
            {
                this.source = value;
                onPropertyChanged("Source");
            }
        }

        /// <summary>
        /// TODO: Write general description for this method
        /// </summary>
        [JsonProperty("target")]
        public Models.GetTransferTargetResponse Target 
        { 
            get 
            {
                return this.target; 
            } 
            set 
            {
                this.target = value;
                onPropertyChanged("Target");
            }
        }
    }
} 