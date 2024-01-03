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
    public class CreatePhonesRequest : BaseModel 
    {
        // These fields hold the values for the public properties.
        private Models.CreatePhoneRequest homePhone;
        private Models.CreatePhoneRequest mobilePhone;

        /// <summary>
        /// TODO: Write general description for this method
        /// </summary>
        [JsonProperty("home_phone")]
        public Models.CreatePhoneRequest HomePhone 
        { 
            get 
            {
                return this.homePhone; 
            } 
            set 
            {
                this.homePhone = value;
                onPropertyChanged("HomePhone");
            }
        }

        /// <summary>
        /// TODO: Write general description for this method
        /// </summary>
        [JsonProperty("mobile_phone")]
        public Models.CreatePhoneRequest MobilePhone 
        { 
            get 
            {
                return this.mobilePhone; 
            } 
            set 
            {
                this.mobilePhone = value;
                onPropertyChanged("MobilePhone");
            }
        }
    }
} 