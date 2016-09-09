using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Hive.Common
{
    public class Json
    {
        private static readonly JsonSerializerSettings Settings;

        static Json()
        {
            Settings = new JsonSerializerSettings();
            Settings.Formatting = Formatting.None;
            Settings.PreserveReferencesHandling = PreserveReferencesHandling.None;
            Settings.TypeNameHandling = TypeNameHandling.All;
            Settings.TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple;
        }

        public static string Serialize(object obj)
        {
            string json = JsonConvert.SerializeObject(obj, Settings);
            return json;
        }

        public static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, Settings);
        }
    }
}
