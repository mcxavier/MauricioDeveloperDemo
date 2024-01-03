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
    public class CreateSetupRequest : BaseModel 
    {
        // These fields hold the values for the public properties.
        private int amount;
        private string description;
        private Models.CreatePaymentRequest payment;

        /// <summary>
        /// Setup amount
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
        /// Description
        /// </summary>
        [JsonProperty("description")]
        public string Description 
        { 
            get 
            {
                return this.description; 
            } 
            set 
            {
                this.description = value;
                onPropertyChanged("Description");
            }
        }

        /// <summary>
        /// Payment data
        /// </summary>
        [JsonProperty("payment")]
        public Models.CreatePaymentRequest Payment 
        { 
            get 
            {
                return this.payment; 
            } 
            set 
            {
                this.payment = value;
                onPropertyChanged("Payment");
            }
        }
    }
} 