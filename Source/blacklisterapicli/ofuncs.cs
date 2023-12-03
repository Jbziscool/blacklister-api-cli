using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blacklisterapicli
{
    public class ofuncs
    {

        public static void ColouredText(string text, ConsoleColor consolecolor)
        {
            Console.ForegroundColor = consolecolor;
            Console.WriteLine(text);
            Console.ResetColor();
        }


        public static void SetApiKey(string apiKey)
        {
            string baseDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
            string folderPath = Path.Combine(baseDirectory, "blacklistercli");

            Directory.CreateDirectory(folderPath);

            string filePath = Path.Combine(folderPath, "apikey.txt");

            File.WriteAllText(filePath, apiKey);
        }


        public static string GetApiKey() 
        {
            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "blacklistercli");
            string filePath = Path.Combine(folderPath, "apikey.txt");
            string funnyapikey = File.ReadAllText(filePath);
            return funnyapikey;
        }


        public static void die()
        {
            Console.ReadKey();
            Environment.Exit(0);
        }


        //-----------------------------------------------------------------------------------------------------------------------------

        //Ei koodia ohi täällä

    }
}
