using System;
using System.Web;
using Discord.WebSocket;
using Humanizer;
using nayuta.Internal;
using nayuta.Modules.Osu;

namespace nayuta.Commands
{
    public abstract class CommandOsu : Command
    {
        protected OsuUser _osuUser;
        protected OsuMode _osuMode;
        
        public CommandOsu(string commandName, string commandDescription = null) : base("osu"+commandName, commandDescription)
        {
        }

        public abstract override object CommandHandler(SocketMessage socketMessage, string input, CommandArguments arguments);

        protected void ApplyMode(CommandArguments args)
        {
            _osuMode = OsuMode.Standard;

            if (args.Get("m") != null)
            {
                switch (args.Get("m"))
                {
                    case "standard":
                        _osuMode = OsuMode.Standard;
                        break;
                    case "mania":
                        _osuMode = OsuMode.Mania;
                        break;
                    case "ctb":
                    case "catch":
                        _osuMode = OsuMode.Catch;
                        break;
                    case "taiko":
                        _osuMode = OsuMode.Taiko;
                        break;
                    default:
                        _osuMode = OsuMode.Standard;
                        break;
                }
            }
            
            // if (string.IsNullOrEmpty(input))
            //     return input;
            // if (input.Contains("-m ") || input.Contains(" -m "))
            // {
            //     string foundMode = "";
            //     if (input.Contains(" -m "))
            //     {
            //         foundMode = input.Substring(input.IndexOf(" -m ", StringComparison.Ordinal) + " -m ".Length);
            //         input = input.Substring(0, input.IndexOf(" -m ", StringComparison.Ordinal));
            //     }
            //     else
            //     {
            //         foundMode = input.Substring(input.IndexOf("-m ", StringComparison.Ordinal) + "-m ".Length);
            //         input = input.Substring(0, input.IndexOf("-m ", StringComparison.Ordinal));
            //     }
            //
            //     switch (foundMode.ToLower())
            //     {
            //         case "standard":
            //             _osuMode = OsuMode.Standard;
            //             break;
            //         case "mania":
            //             _osuMode = OsuMode.Mania;
            //             break;
            //         case "ctb":
            //         case "catch":
            //             _osuMode = OsuMode.Catch;
            //             break;
            //         case "taiko":
            //             _osuMode = OsuMode.Taiko;
            //             break;
            //         default:
            //             _osuMode = OsuMode.Standard;
            //             break;
            //     }
            // }
            // return input;
        }

        protected void ApplyPlayer(ulong DiscordID, string input)
        {
            _osuUser = null;
            if (string.IsNullOrEmpty(input))
            {
                InternalUser internalUser = DatabaseManager.Instance.GetUser(DiscordID);
                if (!string.IsNullOrEmpty(internalUser.OsuUserID))
                    _osuUser = OsuApi.GetUser(internalUser.OsuUserID, _osuMode);
            }

            if (_osuUser == null)
            {
                string username = HttpUtility.HtmlEncode(input); // Test value
                _osuUser = OsuApi.GetUser(username, _osuMode);
            }
        }
    }
}