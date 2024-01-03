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
    public class GetTransferTargetResponse : BaseModel 
    {
        // These fields hold the values for the public properties.
        private string targetId;
        private string type;

        /// <summary>
        /// TODO: Write general description for this method
        /// </summary>
        [JsonProperty("target_id")]
        public string TargetId 
        { 
            get 
            {
                return this.targetId; 
            } 
            set 
            {
                this.targetId = value;
                onPropertyChanged("TargetId");
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
    }
} 