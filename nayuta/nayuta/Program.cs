using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using nayuta.Coroutine;
using nayuta.Internal;

namespace nayuta
{
    internal static class Program
    {
        private static DiscordSocketClient _client;
        private static Yielder yielder;
        private static Thread updateThread;

        public static void Main(string[] args)
        {
            yielder = new Yielder();
            
            Bot bot = new Bot("ODA5OTEyMTUzNTgyNzMxMzI0.YCb_eA.E6W2dgDkUrcO1ptZvlnPMX2yo3w", "n!");

            updateThread = new Thread(new ThreadStart(Update));
            updateThread.Start();

            AppDomain.CurrentDomain.ProcessExit += new EventHandler(Close);
        }

        private static void Close(object sender, EventArgs e)
        {
            //DatabaseManager.Instance.Close();
            Console.WriteLine("Closed.");
            InternalUserManager.Instance.SessionUsers.ForEach(user=>DatabaseManager.Instance.UpdateUser(user));
        }

        private static void Update()
        {
            if(yielder!=null)
                yielder.ProcessCoroutines();
            Thread.Sleep(0);
        }
    }
}
