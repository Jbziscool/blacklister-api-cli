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
                Console.Write("Enter 1 argument or enter a user or server id, eg: blist 12389472389472348972\n\n\n1: Check a server or user id\n2: Api key settings\n3: Exit program :c\n\nOption: ");

                ulong option = ulong.Parse(Console.ReadLine()); //Ulong niin se ei bug out ja voimme debugata, jos se on suuri määrä.
                if (option >= 4)
                {
                    string useradiscrimforinput = await uasfuncs.FetchDiscordUsername(option);

                    await uasfuncs.CheckUserBlacklist(useradiscrimforinput, option);
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
                        Console.Write("Paste your api key or type view to see it:");
                        string apikey = Console.ReadLine();
                        if (apikey == "view")
                        {
                            string currentapikey = ofuncs.GetApiKey();
                            Console.WriteLine(currentapikey);
                            ofuncs.die();
                        }
                        ofuncs.SetApiKey(apikey);
                        await Console.Out.WriteLineAsync($"Your api key was set as: {apikey}");
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
