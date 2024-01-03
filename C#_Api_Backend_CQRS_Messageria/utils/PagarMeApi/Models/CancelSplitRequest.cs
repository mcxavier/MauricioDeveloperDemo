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
    public class CancelSplitRequest : BaseModel 
    {
        // These fields hold the values for the public properties.
        private string type;
        private int amount;
        private string recipientId;
        private Models.CreateSplitOptionsRequest options;
        private string splitRuleId;

        /// <summary>
        /// Split type
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
        /// Amount
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
        /// Recipient id
        /// </summary>
        [JsonProperty("recipient_id")]
        public string RecipientId 
        { 
            get 
            {
                return this.recipientId; 
            } 
            set 
            {
                this.recipientId = value;
                onPropertyChanged("RecipientId");
            }
        }

        /// <summary>
        /// The split options request
        /// </summary>
        [JsonProperty("options")]
        public Models.CreateSplitOptionsRequest Options 
        { 
            get 
            {
                return this.options; 
            } 
            set 
            {
                this.options = value;
                onPropertyChanged("Options");
            }
        }

        /// <summary>
        /// Rule id
        /// </summary>
        [JsonProperty("split_rule_id")]
        public string SplitRuleId 
        { 
            get 
            {
                return this.splitRuleId; 
            } 
            set 
            {
                this.splitRuleId = value;
                onPropertyChanged("SplitRuleId");
            }
        }
    }
} 