using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.Extensions
{
    public static partial class ObjectExtension
    {
        public static string AsJson(this object instance, bool ignoreNull = false, bool microsoftDateFormat = false)
        {
            /* NullValueHandling: incluir porque os códigos em javascript precisam de todas as propriedades. */
            var settings = new JsonSerializerSettings()
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Unspecified,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = ignoreNull ? NullValueHandling.Ignore : NullValueHandling.Include,
                DefaultValueHandling = ignoreNull ? DefaultValueHandling.Ignore : DefaultValueHandling.Include
            };

            if (microsoftDateFormat)
                settings.DateFormatHandling = DateFormatHandling.MicrosoftDateFormat;

            return JsonConvert.SerializeObject(instance, Formatting.None, settings);
        }
    }
}
