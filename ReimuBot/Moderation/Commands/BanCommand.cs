﻿using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Reimu.Core;
using Reimu.Common.Data.Parts;

namespace Reimu.Moderation.Commands
{
    [Name("Moderation")]
    public class BanCommand : ReimuBase
    {
        [Command("ban"), RequireBotPermission(GuildPermission.BanMembers),
         RequireUserPermission(GuildPermission.BanMembers)]
        public async Task BanUserAsync(SocketGuildUser user, [Remainder] string reason = null)
        {
            if (user.Id == Context.Client.CurrentUser.Id)
            {
                await ReplyAsync("no.");
                return;
            }

            if (!(Context.User as SocketGuildUser).IsUserHigherThan(user))
            {
                await ReplyAsync("Cannot perform this action on a user that is the same or ranked higher than you.");
                return;
            }

            if (!Context.Guild.CurrentUser.IsUserHigherThan(user))
            {
                await ReplyAsync("Cannot perform this action on a user that is the same or ranked higher than me.");
                return;
            }

            var banReason = reason?.Length > 512 ? reason.Substring(0, 512) : reason;
            await (await user.GetOrCreateDMChannelAsync()).SendMessageAsync(
                $"**[Banned from {Context.Guild.Name}]**\n" +
                $"Reason: {reason ?? "No reason provided."}");
            await user.BanAsync(1, banReason);
            await ModerationHelper.LogAsync(Context, user, CaseType.Ban, reason);
            await ReplyAsync($"{user.Nickname ?? user.Username} was banned.");
        }

        // TODO: We may also want to ban users not in our server by id
        [Command("ban"), RequireBotPermission(GuildPermission.BanMembers),
         RequireUserPermission(GuildPermission.BanMembers)]
        public async Task BanUserAsync(ulong id, [Remainder] string reason = null)
            => await BanUserAsync(await ModerationHelper.ResolveUser(Context.Guild, id), reason);
    }
}
