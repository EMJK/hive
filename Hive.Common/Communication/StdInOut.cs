using System;
using System.IO;
using System.Text;

namespace Hive.Common.Communication
{
    public class StdInOut
    {
        public static void WriteLine(TextWriter stream, object obj)
        {
            var json = Json.Serialize(obj);
            stream.WriteLine(json);
            stream.Flush();
        }

        public static T ReadLine<T>(TextReader stream)
        {
            StringBuilder sb = new StringBuilder();
            while (true)
            {
                char ch = Convert.ToChar(stream.Read());
                sb.Append(ch);
                if (ch == '\n')
                {
                    break;
                }
            }
            var json = sb.ToString().Trim();
            var obj = Json.Deserialize<T>(json);
            return obj;
        }
    }
}
