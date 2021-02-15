using Discord;

namespace nayuta
{
    public static class EmbedHelper
    {
        public static EmbedFieldBuilder BlankField = new EmbedFieldBuilder()
        {
            Name = "\u200B",
            Value = "\u200B",
            IsInline = true
        };
    }
}