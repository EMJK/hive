using System;
using System.IO;
using System.Text;

namespace Hive.Common.Communication
{
    public class StreamHelper
    {
        public static void WriteLine(TextWriter stream, object obj)
        {
            var json = Json.Serialize(obj);
            stream.WriteLine(json);
            stream.Flush();
        }

        public static T ReadLine<T>(TextReader stream)
        {
            var sb = new StringBuilder();
            while (true)
            {
                var ch = Convert.ToChar(stream.Read());
                sb.Append(ch);
                if (ch == '\n')
                    break;
            }
            var json = sb.ToString().Trim();
            var obj = Json.Deserialize<T>(json);
            return obj;
        }
    }
}