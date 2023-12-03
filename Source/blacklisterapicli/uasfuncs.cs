using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace blacklisterapicli
{

    public class Blacklist
    {
        [JsonProperty("blacklisted")]
        public bool Blacklisted { get; set; }

        [JsonProperty("reason")]
        public string? Reason { get; set; }

        [JsonProperty("date")]
        public string? Date { get; set; }

        [JsonProperty("evidence")]
        public string? Evidence { get; set; }
    }

    public class uasfuncs
    {

        private static ulong useridarg;
        static string apikey = ofuncs.GetApiKey();

        public static void SetUserIdCliArg(ulong userid)
        {
            useridarg = userid;
        }


        public static async Task<string> FetchDiscordUsername(ulong idtouse)
        {
            bool found;
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync($"https://lookup.phish.gg/user/{idtouse}");

                string? userData = await response.Content.ReadAsStringAsync();
                JObject? jsonuserdata = JObject.Parse(userData);

                if (!jsonuserdata.ToString().Contains("username"))
                {
                    bool resultiffound = await CheckServerId(idtouse);
                    if (!resultiffound)
                    {
                        ofuncs.ColouredText("\nVirhe: user-id virheellinen ja pavilein-id ei löydy tietokannassa", ConsoleColor.Yellow);
                    }
                    
                    found = false;
                    ofuncs.die();
                    
                }
                found = true;
                return $"{jsonuserdata["username"]}#{jsonuserdata["discriminator"]}";
            }
        }


        public static async Task<bool> CheckUserBlacklist(string username, ulong userId)
        {
            bool isBlacklisted;

            using (HttpClient client = new HttpClient())
            {
                HttpRequestMessage request = new(HttpMethod.Get, $"https://api.blacklister.xyz/{userId}");
                request.Headers.Add("Authorization", apikey);
                HttpResponseMessage response = await client.SendAsync(request);

                string json = await response.Content.ReadAsStringAsync();

                Blacklist bdata = JsonConvert.DeserializeObject<Blacklist>(json);

                if (json.Contains("Unauthorized"))
                {
                    ofuncs.ColouredText("Error: Invalid api key, make sure you have set one up.", ConsoleColor.Yellow);
                    ofuncs.die();
                }

                if (bdata.Blacklisted)
                {
                    ofuncs.ColouredText($"\n{username} - {userId} is blacklisted from the blacklister api", ConsoleColor.Red);
                    ofuncs.ColouredText("---------------------------------------------------------------------------------------------", ConsoleColor.Gray);
                    Console.WriteLine($"They are blacklisted for {bdata.Reason} --- at date: {bdata.Date}\nEvidence: {bdata.Evidence}");
                    isBlacklisted = true;
                }
                else
                {
                    ofuncs.ColouredText($"\n{username} - {userId} isn't blacklisted!\n", ConsoleColor.Green);
                    isBlacklisted = false;
                }
            }

            return isBlacklisted;
        }



        public static async Task<bool> CheckServerId(ulong id)
        {
            bool found;
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync($"https://api.phish.gg/server?id={id}");

                var jsonResponse = await response.Content.ReadAsStringAsync();
                dynamic data = JsonConvert.DeserializeObject(jsonResponse);

                if (jsonResponse.ToString().Contains("reason"))
                {
                    found = true;
                    ofuncs.ColouredText($"Blacklisted server found.", ConsoleColor.White);
                    ofuncs.ColouredText($"server: {data.name} - {id} is blacklisted for reason {data.reason} (Key {data.key})", ConsoleColor.Red);
                }
                else
                {
                    found = false;
                    ofuncs.ColouredText($"The server id {id} is not a blacklisted server or user/is not a valid server id.", ConsoleColor.Green);
                }
            }
            return found;
        }


    }
}
