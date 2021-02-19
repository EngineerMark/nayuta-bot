using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using Discord;
using Discord.WebSocket;
using nayuta.Math;

namespace nayuta.Commands
{
    public class CommandSystem : Command
    {
        private readonly Dictionary<string, string> systemData = new Dictionary<string, string>()
        {
            ["CPU"] = "",
        };
        
        public CommandSystem() : base("status", "Shows what Nayuta runs on")
        {

        }
        
        public override object CommandHandler(SocketMessage socketMessage, string input)
        {
            SelectQuery cpuQuery = new SelectQuery("Win32_Processor");
            SelectQuery gpuQuery = new SelectQuery("Win32_VideoController");
            SelectQuery netQuery = new SelectQuery("Win32_NetworkAdapter");

            ManagementObjectSearcher cpuDetails = new ManagementObjectSearcher(cpuQuery);
            ManagementObjectCollection cpuDetailsCollection = cpuDetails.Get();
            ManagementObject cpuMo = cpuDetailsCollection.OfType<ManagementObject>().FirstOrDefault();

            ManagementObjectSearcher gpuDetails = new ManagementObjectSearcher(gpuQuery);
            ManagementObjectCollection gpuDetailsCollection = gpuDetails.Get();
            ManagementObject gpuMo = gpuDetailsCollection.OfType<ManagementObject>().FirstOrDefault();
                
            ManagementObjectSearcher netDetails = new ManagementObjectSearcher(netQuery);
            ManagementObjectCollection netDetailsCollection = netDetails.Get();
            ManagementObject netMo = netDetailsCollection.OfType<ManagementObject>().FirstOrDefault();

            // return (string)mo["Name"];
            EmbedBuilder embed = new EmbedBuilder()
            {
                Title = "Nayuta runs on these specs",
                Fields = new List<EmbedFieldBuilder>()
                {
                    new EmbedFieldBuilder()
                    {
                        Name = "CPU",
                        Value = (string)cpuMo["Name"]
                    },
                    new EmbedFieldBuilder()
                    {
                        Name = "Network",
                        Value = netMo["MaxSpeed"]+""
                    }
                }
            };

            return embed;
        }

        private string NetworkSpeed()
        {
            System.Net.WebClient wc = new System.Net.WebClient();

            //DateTime Variable To Store Download Start Time.
            DateTime dt1 = DateTime.Now;

            //Number Of Bytes Downloaded Are Stored In ‘data’
            byte[] data = wc.DownloadData("http://google.com");

            //DateTime Variable To Store Download End Time.
            DateTime dt2 = DateTime.Now;

            //To Calculate Speed in Kb Divide Value Of data by 1024 And Then by End Time Subtract Start Time To Know Download Per Second.
            return Mathf.Round((float)(((data.Length / 1024) / (dt2 - dt1).TotalSeconds)/1024))+"mbps";
        }
    }
}