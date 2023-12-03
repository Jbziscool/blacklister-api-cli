using System;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Newtonsoft.Json.Linq;
using System.Net;

namespace blacklisterapicli
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.Write("Käytä yks argumentti, eg: blist 12389472389472348972\n\n\n1: Tarkista palvelin tai käyttäjä\n2: Määritä api-avain\n3: Poistu :c\n\nValitse vaihtoehto: ");
                
                ulong option = ulong.Parse(Console.ReadLine()); //Ulong niin se ei bug out ja voimme debugata, jos se on suuri määrä.
                if (option >= 4)
                {
                    ofuncs.ColouredText("Virhe: valitse ensi kerralla pätevä vaihtoehto.", ConsoleColor.Yellow);
                    ofuncs.die();
                }

                switch (option)
                {
                    case 1:
                        Console.Write("\nSyötä id: ");
                        ulong useridinput = ulong.Parse(Console.ReadLine());
                        string useradiscrimforinput = await uasfuncs.FetchDiscordUsername(useridinput);

                        await uasfuncs.CheckUserBlacklist(useradiscrimforinput, useridinput);
                        ofuncs.die();
                        break;

                    case 2:
                        Console.Write("Syötä api-avaim:");
                        string apikey = Console.ReadLine();
                        if (apikey == "view")
                        {
                            string currentapikey = ofuncs.GetApiKey();
                            Console.WriteLine(currentapikey);
                            ofuncs.die();
                        }
                        ofuncs.SetApiKey(apikey);
                        await Console.Out.WriteLineAsync($"Api-avaimesi on onnistuneesti tallennettu: {apikey}");
                        ofuncs.die();
                        break;


                }
            }

            ulong useridarg = ulong.Parse(args[0]);
            uasfuncs.SetUserIdCliArg(useridarg);
            string useradiscrim = await uasfuncs.FetchDiscordUsername(useridarg);
            await uasfuncs.CheckUserBlacklist(useradiscrim, useridarg);
            ofuncs.die();
        }
    }
}