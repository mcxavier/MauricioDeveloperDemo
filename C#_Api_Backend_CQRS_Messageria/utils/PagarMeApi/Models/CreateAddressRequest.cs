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
    public class CreateAddressRequest : BaseModel 
    {
        // These fields hold the values for the public properties.
        private string street;
        private string number;
        private string zipCode;
        private string neighborhood;
        private string city;
        private string state;
        private string country;
        private string complement;
        private Dictionary<string, string> metadata;
        private string line1;
        private string line2;

        /// <summary>
        /// Street
        /// </summary>
        [JsonProperty("street")]
        public string Street 
        { 
            get 
            {
                return this.street; 
            } 
            set 
            {
                this.street = value;
                onPropertyChanged("Street");
            }
        }

        /// <summary>
        /// Number
        /// </summary>
        [JsonProperty("number")]
        public string Number 
        { 
            get 
            {
                return this.number; 
            } 
            set 
            {
                this.number = value;
                onPropertyChanged("Number");
            }
        }

        /// <summary>
        /// The zip code containing only numbers. No special characters or spaces.
        /// </summary>
        [JsonProperty("zip_code")]
        public string ZipCode 
        { 
            get 
            {
                return this.zipCode; 
            } 
            set 
            {
                this.zipCode = value;
                onPropertyChanged("ZipCode");
            }
        }

        /// <summary>
        /// Neighborhood
        /// </summary>
        [JsonProperty("neighborhood")]
        public string Neighborhood 
        { 
            get 
            {
                return this.neighborhood; 
            } 
            set 
            {
                this.neighborhood = value;
                onPropertyChanged("Neighborhood");
            }
        }

        /// <summary>
        /// City
        /// </summary>
        [JsonProperty("city")]
        public string City 
        { 
            get 
            {
                return this.city; 
            } 
            set 
            {
                this.city = value;
                onPropertyChanged("City");
            }
        }

        /// <summary>
        /// State
        /// </summary>
        [JsonProperty("state")]
        public string State 
        { 
            get 
            {
                return this.state; 
            } 
            set 
            {
                this.state = value;
                onPropertyChanged("State");
            }
        }

        /// <summary>
        /// Country. Must be entered using ISO 3166-1 alpha-2 format. See https://pt.wikipedia.org/wiki/ISO_3166-1_alfa-2
        /// </summary>
        [JsonProperty("country")]
        public string Country 
        { 
            get 
            {
                return this.country; 
            } 
            set 
            {
                this.country = value;
                onPropertyChanged("Country");
            }
        }

        /// <summary>
        /// Complement
        /// </summary>
        [JsonProperty("complement")]
        public string Complement 
        { 
            get 
            {
                return this.complement; 
            } 
            set 
            {
                this.complement = value;
                onPropertyChanged("Complement");
            }
        }

        /// <summary>
        /// Metadata
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
        /// Line 1 for address
        /// </summary>
        [JsonProperty("line_1")]
        public string Line1 
        { 
            get 
            {
                return this.line1; 
            } 
            set 
            {
                this.line1 = value;
                onPropertyChanged("Line1");
            }
        }

        /// <summary>
        /// Line 2 for address
        /// </summary>
        [JsonProperty("line_2")]
        public string Line2 
        { 
            get 
            {
                return this.line2; 
            } 
            set 
            {
                this.line2 = value;
                onPropertyChanged("Line2");
            }
        }
    }
} 