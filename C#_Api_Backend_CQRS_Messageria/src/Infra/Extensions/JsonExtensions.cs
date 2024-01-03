using System;
using System.Text;
using Newtonsoft.Json;

namespace Infra.Extensions
{
    public static class JsonExtensions
    {
        public static T GetDataFromHash<T>(string hash)
        {
            var bytes = Convert.FromBase64String(hash);
            return JsonConvert.DeserializeObject<T>(Encoding.ASCII.GetString(bytes));
        }
    }
}