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
            Settings.Formatting = Formatting.Indented;
            Settings.PreserveReferencesHandling = PreserveReferencesHandling.None;
            Settings.TypeNameHandling = TypeNameHandling.All;
            Settings.TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple;
        }

        public static byte[] Serialize(object obj)
        {
            string json = JsonConvert.SerializeObject(obj, Settings);
            return Encoding.UTF8.GetBytes(json);
        }

        public static T Deserialize<T>(byte[] data)
        {
            string json = Encoding.UTF8.GetString(data);
            return JsonConvert.DeserializeObject<T>(json, Settings);
        }
    }
}
