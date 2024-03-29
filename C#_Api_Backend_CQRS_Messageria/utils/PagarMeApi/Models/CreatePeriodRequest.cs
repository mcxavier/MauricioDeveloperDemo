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
    public class CreatePeriodRequest : BaseModel 
    {
        // These fields hold the values for the public properties.
        private DateTime? endAt;

        /// <summary>
        /// TODO: Write general description for this method
        /// </summary>
        [JsonConverter(typeof(IsoDateTimeConverter))]
        [JsonProperty("end_at")]
        public DateTime? EndAt 
        { 
            get 
            {
                return this.endAt; 
            } 
            set 
            {
                this.endAt = value;
                onPropertyChanged("EndAt");
            }
        }
    }
} 