using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace MS_Finance.Client
{
    class Program
    {
        static string host = "http://localhost:53438/";

        static void Main(string[] args)
        {
            Console.WriteLine("Attempting to Log in with default admin user");

            // Get hold of a Dictionary representing the JSON in the response Body:
            var responseDictionary =
                GetResponseAsDictionary("admin@example.com", "Admin@123456");
            foreach (var kvp in responseDictionary)
            {
                Console.WriteLine("{0}: {1}", kvp.Key, kvp.Value);
            }
            Console.Read();
        }


        static Dictionary<string, string> GetResponseAsDictionary(
            string userName, string password)
        {
            HttpClient client = new HttpClient();
            var pairs = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>( "grant_type", "password" ), 
                    new KeyValuePair<string, string>( "username", userName ), 
                    new KeyValuePair<string, string> ( "Password", password )
                };
            var content = new FormUrlEncodedContent(pairs);

            // Attempt to get a token from the token endpoint of the Web Api host:
            HttpResponseMessage response =
                client.PostAsync(host + "Token", content).Result;
            var result = response.Content.ReadAsStringAsync().Result;
            // De-Serialize into a dictionary and return:
            Dictionary<string, string> tokenDictionary =
                JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
            return tokenDictionary;
        }

    }
}
