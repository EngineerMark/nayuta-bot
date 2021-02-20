# Nayuta Kani

Nayuta Kani is a Discord Bot primarily made to give users data of certain games or statistics.

## Important

For all builds, a sqlite database is required for the bot to function currently.
Currently, the structure is as followed:

| Datatables | | | |
| --- | --- | --- | --- |
| users | id INT (primary, autoincrement) | discord_id TEXT | osu_id TEXT |

The database has to reside as `database.db` in the root application folder, under the .exe basically.

Since this is pretty important, I will try to keep this as up to date as possible. If however the last update in the README is much older than the development branch (or main, whichever you prefer to work off), please open an issue and I will update it.
In the meanwhile you can look in DatabaseManager.cs for most of the information in such case.

This is a personal project. Don't expect enterprise-style support, help and development. I am doing stuff my way, but keeping it open source.
To see upcoming features and in development material, refer to the links at the bottom.

## Used packages

[Discord.Net 2.3.0](https://www.nuget.org/packages/Discord.Net/)

[Humanizer 2.8.28](https://www.nuget.org/packages/Humanizer/)

[HtmlAgilityPack 1.11.30](https://www.nuget.org/packages/HtmlAgilityPack/) (Currently unused, but I have plans for it)

[sqlite-net-pcl 1.8.0-beta](https://www.nuget.org/packages/sqlite-net-pcl)


## osu!

osu! is the main game and focus of this bot. While PP is not necessarily correct, the error margin is pretty small and works for all gamemodes. Catch The Beat in particular has quite an error margin, but all is subject to change and I try to keep up with changes from the official calculations.

## Adding Commands

The command system is relatively easy to deal with, and it's very easy to create new commands.
The main thing to keep in mind is to inherit your command class from the base Command class, and implement all required members. The return type of an command is an literal object, so anything that is an object can be returned.
Currently, the command system deals with the following object types:
```cs
System.string
Discord.Embed
Discord.EmbedBuilder
```

In the future, new/other types might be added and conversions will happen for you, but expect these types to be the best and try to keep it to those.

It's possible to derive from your command class, just make sure the CommandHandler is abstract in that case.

## API Keys
APIKeys.cs contains a small notice on storing API keys and other sensitive information.

## Notice
Readme is not always up to date, and I will not actively keep it up to date.
When stuff is missing, just open an issue and I will deal with it within reasonable time.

## License
[MIT](https://choosealicense.com/licenses/mit/)

## Links
[Trello Board](https://trello.com/b/Sgk7Slsh/nayuta)
