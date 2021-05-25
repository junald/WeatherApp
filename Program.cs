using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    
    class Program
    {
         
        static string API_KEY = "065c6401dec6c37135283756b1a3faaf";

        static void Main(string[] args)
        {
            Console.WriteLine("Enter ZipCode: ");
            int zipCode = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Please wait. Getting weather information.");
            HttpClient client = new HttpClient();
            string address = String.Format("http://api.weatherstack.com/current?access_key={0}&query={1}",API_KEY,zipCode);
            client.BaseAddress = new Uri(address);

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            GetWeather(client).Wait();
        }

        static async Task GetWeather(HttpClient cons)

        {
            using (cons)
            {
                HttpResponseMessage res = await cons.GetAsync("");
                res.EnsureSuccessStatusCode();
                if (res.IsSuccessStatusCode)
                {
                    string weather = await res.Content.ReadAsStringAsync();
                    JObject jobj = JObject.Parse(weather);
                    JToken jToken = jobj["current"];
                  //  Console.WriteLine(jToken.ToString());
                    string uvIndex = jToken["uv_index"].ToString();
                    string windSpeed = jToken["wind_speed"].ToString();
                    string precip = jToken["wind_speed"].ToString();
                     
                    Console.WriteLine("Should I go outside? " + shouldIGoOutside(int.Parse(precip)));                  
                    Console.WriteLine("Should I wear sunscreen? " + shouldIWhereSunscreen(int.Parse(uvIndex)));
                    Console.WriteLine("Can I fly my kite? " + canIFlyMyKite(int.Parse(precip), int.Parse(windSpeed)));
                }
            }
        }

        static string shouldIGoOutside(int precip)
        {
            if(precip > 50)
            {
                return "No its raining!";
            }
            else
            {
                return "Yes";
            }
            
        }

        static string shouldIWhereSunscreen(int uvIndex)
        {
            //check UV index above 3 then yes
            if (uvIndex > 3)
            {
                return "Yes";
            }
            else {
                return "No";
            }
            
        }

        static string canIFlyMyKite(int precip , int windSpeed)
        {
            //Yes if not raining and wind speed over 15
            if (precip < 50 && windSpeed > 15)
            {
                return "Yes";
            }else
            {
                return "No";
            }
        }

    }
}
