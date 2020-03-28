using CommandHandler;
using CommandHandler.Attributes;
using CommandHandler.Types;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Interactivity.Extensions;

namespace Sample.CommandModules
{
    public class InteractivityCommands : CommandModule
    {

        [Command("conversation")]
        public async Task StartConversation(CommandContext ctx)
        {
            await ctx.RespondAsync($"Hello! What's your name?");
            var result = await ctx.BotClient.GetInteractivity().WaitForMessageAsync(ctx.Chat);
            if (result.Value == null)
            {
                //Timed out
                await ctx.RespondAsync($"Timed out. Please try again.");
            } else
            {
                var message = result.Value;
                var me = await ctx.BotClient.GetMeAsync();
                await ctx.RespondAsync($"Hello, {message.Text}! I am {me.FirstName}.");
            }
        }

    }
}
