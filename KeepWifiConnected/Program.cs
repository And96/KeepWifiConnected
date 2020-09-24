using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KeepWifiConnected
{
    class Program
    {

        const int TimeRecheck = 10;
        const string fileLogName = @"KeepWifiConnected.log";
        const string urlRequest = @"http://google.com/generate_204";

        static void Main()
        {
            Log("Start");
            while (true)
            {
                string wifiNetworkName = GetWifiNetworkName();
                if (wifiNetworkName.Length > 1)
                {
                    Log("Wifi Network: " + wifiNetworkName);
                    if (IsInternetOn())
                    {
                        Log("Internet ON");
                    }
                    else
                    {
                        Log("Internet OFF");
                        WifiReconnect();
                        Log("Reconnection");
                    }
                }
                else
                {
                    Log("No Wifi Network");
                }
                Thread.Sleep(TimeRecheck * 1000);
            }

        }

        static string GetWifiNetworkName()
        {
            try
            {
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = @"netsh.exe";
                p.StartInfo.Arguments = @"wlan show interfaces";
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.Start();
                string s = p.StandardOutput.ReadToEnd();
                if (s.IndexOf("SSID") < 0) return "";
                string s1 = s.Substring(s.IndexOf("SSID"));
                s1 = s1.Substring(s1.IndexOf(":"));
                s1 = s1.Substring(2, s1.IndexOf("\n")).Trim();
                p.WaitForExit();
                return s1;
            }
            catch
            {
                return "";
            }
        }

        static bool IsInternetOn()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead(urlRequest))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        static void WifiConnect(string networkName)
        {
            try
            {
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = @"netsh.exe";
                p.StartInfo.Arguments = @"wlan connect name = """ + networkName + @"";
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.Start();
                p.WaitForExit();
            }
            catch { }
        }

        static void WifiDisconnect()
        {
            try
            {
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = @"netsh.exe";
                p.StartInfo.Arguments = @"wlan disconnect";
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.Start();
                p.WaitForExit();
            }
            catch { }
        }

        static void WifiReconnect()
        {
            try
            {
                string wifiNetworkName = GetWifiNetworkName();
                if (wifiNetworkName.Length > 1)
                {
                    WifiDisconnect();
                    WifiConnect(wifiNetworkName);
                }
                
            }
            catch { }
        }

        static void Log(string message)
        {
            try
            {
                string fileLogPath = Path.Combine(Directory.GetCurrentDirectory(), fileLogName);
                string content = DateTime.Now + ": " + message;
                Console.WriteLine(content);
                File.AppendAllText(fileLogPath, content);
                File.AppendAllText(fileLogPath, Environment.NewLine);
            }
            catch { }
        }

    }
}
