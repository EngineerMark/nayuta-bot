using System.Collections.Generic;
using System.Threading;
using Discord.WebSocket;
using nayuta.Commands;

namespace nayuta
{
    public class GuildThreadManager : Manager<GuildThreadManager>
    {
        /// <summary>
        /// Collection of threads for each guild
        /// </summary>
        private Dictionary<ulong, Thread> _guildThreadList;
        private List<CommandQueue> _queue;

        public GuildThreadManager()
        {
            _queue = new List<CommandQueue>();
            _guildThreadList = new Dictionary<ulong, Thread>();
        }

        public void EnqueueCommand(Bot bot, SocketMessage socketMessage)
        {
            ulong guildID = ((SocketGuildChannel) socketMessage.Channel).Guild.Id;
            if (!IsGuildThreaded(guildID))
            {
                Thread guildThread = new Thread(new ParameterizedThreadStart(InternalUpdateThread));
                _guildThreadList.Add(guildID, guildThread);
                guildThread.Start(guildID);
            }
            
            if(_queue.Find(a=>a.GuildID==guildID)==null)
                _queue.Add(new CommandQueue()
                {
                    GuildID = guildID
                });

            CommandQueue queue = _queue.Find(a => a.GuildID == guildID);
            queue.List.Enqueue(new CommandQueueItem()
            {
                Bot = bot,
                SocketMessage = socketMessage
            });
        }

        private CommandQueue GetGuildQueue(ulong GuildID)
        {
            return _queue.Find(a => a.GuildID == GuildID);
        }

        private bool IsGuildThreaded(ulong guildID) => _guildThreadList.ContainsKey(guildID);

        private void InternalUpdateThread(object data)
        {
            while (true)
            {
                CommandQueue queue = GetGuildQueue((ulong) data);
                if (queue.List.Count > 0)
                {
                    CommandQueueItem item = queue.List.Dequeue();
                    CommandManager.Instance.ProcessCommands(item.Bot, item.SocketMessage);
                }
            }
        }
    }

    public class CommandQueue
    {
        public ulong GuildID;
        public Queue<CommandQueueItem> List = new Queue<CommandQueueItem>();
    }

    public struct CommandQueueItem
    {
        public Bot Bot;
        public SocketMessage SocketMessage;
    }
}