﻿using System;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Reimu.Database;
using Reimu.Database.Models;

namespace Reimu.Core
{
    public class BotContext : SocketCommandContext
    {
        public IDocumentSession Session { get; } // TODO: can we get a session from embedded db?
        public DatabaseHandler Database { get; }
        public BotConfig Config { get; }
        public GuildConfig GuildConfig { get; }

        public BotContext(DiscordSocketClient client, SocketUserMessage msg, IServiceProvider provider) : base(client,
            msg)
        {
            //Session = provider.GetRequiredService<IDocumentStore>().OpenSession();
            Database = provider.GetRequiredService<DatabaseHandler>();
            Config = Database.Get<BotConfig>("Config");
            if (Guild != null)
                GuildConfig = Database.Get<GuildConfig>($"guild-{Guild.Id}");
        }
    }
}