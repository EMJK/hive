using System.Runtime.Serialization.Formatters;
using Newtonsoft.Json;

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
            Settings.ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor;
        }

        public static string Serialize(object obj)
        {
            var json = JsonConvert.SerializeObject(obj, Settings);
            return json;
        }

        public static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, Settings);
        }
    }
}