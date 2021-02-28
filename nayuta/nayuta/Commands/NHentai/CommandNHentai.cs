using System.Collections.Generic;
using Discord;
using Discord.WebSocket;
using Humanizer;
using nayuta.Modules.NHentai;

namespace nayuta.Commands.NHentai
{
    public class CommandNHentai : Command
    {
        public CommandNHentai() : base("nh", "Find a book by ID", true)
        {
            InputValue = true;
            IsNsfw = true;
        }

        public override object CommandHandler(SocketMessage socketMessage, string input, CommandArguments arguments)
        {
            if (input.Length<=0)
                return "Please enter a book ID";

            Book book = NHentaiApi.GetBook(input);
            if (book == null || book.Error!=null)
                return "This book was not found";

            string tagString = "";

            foreach (BookTag tag in book.Tags)
            {
                if (tag.Type == "tag")
                {
                    tagString += tag.Name + ", ";
                }
            }

            if (tagString.Length >= 2)
                tagString = tagString.Substring(0, tagString.Length - 2);

            EmbedBuilder embed = new EmbedBuilder()
            {
                Title = book.Title.Pretty,
                Url = "https://nhentai.net/g/"+book.ID+"/",
                Description = book.Title.Japanese,
                Fields = new List<EmbedFieldBuilder>()
                {
                    new EmbedFieldBuilder()
                    {
                        Name = "Language",
                        Value = book.Language.Humanize(),
                        IsInline = true
                    },
                    new EmbedFieldBuilder()
                    {
                        Name = "Size",
                        Value = book.PageCount+" pages",
                        IsInline = true
                    },
                    new EmbedFieldBuilder()
                    {
                        Name = "Tags",
                        Value = tagString,
                        IsInline = false
                    }
                },
                ImageUrl = "https://t.nhentai.net/galleries/"+book.GalleryID+"/cover.jpg"
            };

            return embed;
        }
    }
}