using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DataRetriever
{
    public class JsonFunctions
    {
        public static void WriteTo(string path, JObject jObject)
        {
            File.WriteAllText(path, jObject.ToString());
        }
        public static JObject ReadFrom(string path)
        {
            return JObject.Parse(File.ReadAllText(path));
        }
    }
}
