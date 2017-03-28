using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;

namespace TrigerV2._0
{
    class ProcessActions
    {
        public static StreamWriter Writer;
        public static List<int> BuildsID = new List<int>();
        public static HttpClient HClient = new HttpClient();
        public static string DefaultConfigFilePath = "config.json";
        public static string DefaultHashFilePath = "hash.json";
        public static string HASHJSON, CONFIGJSON, FLAG;
        // TIME STAMP FOR LOG
        public static string TimeStampLong()
        {
            return DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        }
        public static string TimeStampShort()
        {
            return DateTime.Now.ToString("HH:mm:ss");
        }
        public static string Line()
        {
            string str = "";
            for (int i = 0; i < Console.WindowWidth; i++) str += "=";
            return str;
        }
        public static string Line(string Char)
        {
            string str = "";
            for (int i = 0; i < Console.WindowWidth; i++) str += Char;
            return str;
        }
        public static string Line(int max)
        {
            string str = "";
            for (int i = 0; i < max; i++) str += "=";
            str += "\n";
            return str;
        }
        public static string Line(int max,string Char)
        {
            string str = "";
            for (int i = 0; i < max; i++) str += Char;
            str += "\n";
            return str;
        }
        //================================================================================
        // WORK WITH FILE
        public static string GetFileContent(string filename)
        {
            StreamReader file = new StreamReader(filename);
            string content = file.ReadToEnd();
            file.Close();
            return content;
        }
        public static void FillFile(string filename)
        {
            Writer = new StreamWriter(filename);
            Writer.WriteLine("[\n\n]");
            Writer.Close();
        }
        public static Boolean IfExists(string filename)
        {
            if (File.Exists(filename)) return true;
            return false;
        }
        public static Boolean IfExists(string[] filename)
        {
            if (File.Exists(filename[0]) && File.Exists(filename[1])) return true;
            return false;
        }
        public static void Create(string filename)
        {
            Writer = new StreamWriter(filename);
            Writer.Write("[\n\n]");
            Writer.Close();
        }
        public static void FileInitialization()
        {
            if (IfExists(DefaultConfigFilePath))
            {
                Console.WriteLine("→File «{0}» exists.  ", DefaultConfigFilePath);
                if (GetFileContent(DefaultConfigFilePath) == "") FillFile(DefaultHashFilePath);
                CONFIGJSON = GetFileContent(DefaultConfigFilePath);
            }
            else
            {
                Console.WriteLine("→ Need to create «{0}» file.", DefaultConfigFilePath);
                Create(DefaultConfigFilePath);
            }

            if (IfExists(DefaultHashFilePath))
            {
                Console.WriteLine("→File «{0}» exists.", DefaultHashFilePath);
                if (GetFileContent(DefaultHashFilePath) == "") FillFile(DefaultHashFilePath);
                HASHJSON = GetFileContent(DefaultHashFilePath);
            }
            else
            {
                Console.WriteLine("→ Need to create «{0}» file.", DefaultHashFilePath);
                Create(DefaultHashFilePath);
            }
            Load(0);
            Load(1);
            if (!IfExists(new string[2] { DefaultConfigFilePath, DefaultHashFilePath }) &&
                GetFileContent(DefaultConfigFilePath) == "" &&
                GetFileContent(DefaultHashFilePath) == "") FileInitialization();


        }
        public static IEnumerable<RepositoryData> Load(int identifier)
        {
            switch (identifier)
            {
                case 0:
                    {
                        CONFIGJSON = GetFileContent(DefaultConfigFilePath);
                        IEnumerable<RepositoryData> config = JsonConvert.DeserializeObject<IEnumerable<RepositoryData>>(CONFIGJSON);
                        Console.WriteLine("\t→ Config file loaded.\n{0}\t\t\tCONTENT\n{0}{1}\n{0}",Line(),GJSONArray(CONFIGJSON));
                        return config;
                    }
                case 1:
                    {
                        HASHJSON = GetFileContent(DefaultHashFilePath);
                        IEnumerable<RepositoryData> hash = JsonConvert.DeserializeObject<IEnumerable<RepositoryData>>(HASHJSON);
                        Console.WriteLine("\t→ Hash file loaded.\n{0}\t\t\tCONTENT\n{0}{1}\n{0}", Line(), GJSONArray(HASHJSON));
                        return hash;
                    }
                default: return null;
            }

        }
        //================================================================================
        // HTTP CLIENT CONFIGURATION
        public static void HttpClientConfiguration(string auth_token)
        {
            HClient.DefaultRequestHeaders.Accept.Add(header());
            HClient.DefaultRequestHeaders.Authorization = AuthVal(auth_token);
        }
        public static void HttpClientConfiguration(HttpClient http_client, string auth_token)
        {
            http_client.DefaultRequestHeaders.Accept.Add(header());
            http_client.DefaultRequestHeaders.Authorization = AuthVal(auth_token);
        }
        public static void HttpClientConfiguration(HttpClient http_client, string content_type, string auth_token)
        {
            http_client.DefaultRequestHeaders.Accept.Add(header(content_type));
            http_client.DefaultRequestHeaders.Authorization = AuthVal(auth_token);
        }
        public static AuthenticationHeaderValue AuthVal(string auth_token)
        {
            string token = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", "", auth_token)));
            return new AuthenticationHeaderValue("Basic", token);
        }
        public static MediaTypeWithQualityHeaderValue header()
        {
            return new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json");
        }
        public static MediaTypeWithQualityHeaderValue header(string content_type)
        {
            return new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(content_type);
        }
        //================================================================================
        // HTTP CLIENT SEND METHODS
        // Some code for GET/POST methods
        //================================================================================
        // JSON GET/SET VALUES
        public static string JSON_GETDATA()
        {
            dynamic Content = "";
            switch (FLAG.ToLower())
            {
                case "h": try { Content = JArray.Parse(HASHJSON); return Content; } catch { return null; }
                case "c": try { Content = JArray.Parse(CONFIGJSON); return Content; } catch { return null; }
                default: return null;
            }
        }
        public static JObject GJSONObject(int id)
        {
            dynamic Content = "";
            switch (FLAG.ToLower())
            {
                case "h": try { Content = JArray.Parse(HASHJSON); return (JObject)Content[id]; } catch { return null; }
                case "c": try { Content = JArray.Parse(CONFIGJSON); return (JObject)Content[id]; } catch { return null; }
                default: return null;
            }
        }
        public static JObject GJSONObject(string Key)
        {
            dynamic Content = "";
            switch (FLAG.ToLower())
            {
                case "h": try { Content = JArray.Parse(HASHJSON); return (JObject)Content.Key; } catch { return null; }
                case "c": try { Content = JArray.Parse(CONFIGJSON); return (JObject)Content.Key; } catch { return null; }
                default: return null;
            }
        }
        public static JObject GJSONObject(int id, string Key)
        {
            dynamic Content = "";
            switch (FLAG.ToLower())
            {
                case "h": try { Content = JArray.Parse(HASHJSON); return (JObject)Content[id].Key; } catch { return null; }
                case "c": try { Content = JArray.Parse(CONFIGJSON); return (JObject)Content[id].Key; } catch { return null; }
                default: return null;
            }
        }
        public static JObject PJSONObject(string key, dynamic Value)
        {
            try { return new JObject(key, Value); } catch { return null; }
        }
        public static JArray GJSONArray()
        {
            switch (FLAG.ToLower())
            {
                case "h": try { return JArray.Parse(HASHJSON); } catch { return null; }
                case "c": try { return JArray.Parse(CONFIGJSON); } catch { return null; }
                default: return null;
            }
        }
        public static JArray GJSONArray(string JSON)
        {
            try { return JArray.Parse(JSON); } catch { return null; }
        }
     
        //================================================================================





    }
}
