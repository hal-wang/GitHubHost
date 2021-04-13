using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GitHubHost
{
    class Program
    {
        static void Main(string[] args)
        {
            SetIP().Wait();
            Console.ReadKey();
        }

        static async Task<string> GetIP()
        {
            var httpClient = new HttpClient();
            var res = await httpClient.GetAsync("https://github.com.ipaddress.com");
            res.EnsureSuccessStatusCode();

            var str = await res.Content.ReadAsStringAsync();
            var match = Regex.Match(str, "<li>\\d{1,3}.\\d{1,3}.\\d{1,3}.\\d{1,3}</li>");
            var matchStr = match.Value;
            return matchStr.Substring(4, matchStr.Length - 9);
        }

        static async Task SetIP()
        {
            var ip = await GetIP();

            Console.Write("Get ip: ");
            Console.WriteLine(ip);

            var filePath = @"C:\Windows\System32\drivers\etc\hosts";
            var str = File.ReadAllText(filePath);
            var match = Regex.Match(str, "\\d{1,3}.\\d{1,3}.\\d{1,3}.\\d{1,3} github.com");
            if (match.Success)
            {
                str = str.Replace(match.Value, "");
            }
            str += $"\n{ip} github.com";

            File.WriteAllText(filePath, str);
            Console.WriteLine("Finished");
        }
    }
}
