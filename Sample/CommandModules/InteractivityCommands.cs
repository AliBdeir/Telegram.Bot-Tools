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
            // Ask the user what's their name.
            await ctx.RespondAsync($"Hello! What's your name?");
            // Wait for a result.
            var result = await ctx.BotClient.GetInteractivity().WaitForMessageAsync(ctx.Chat, ctx.Message.From,);
            if (result.Value == null)
            {
                //Timed out
                await ctx.RespondAsync($"Timed out. Please try again.");
            } else
            {
                //Didn't time out.
                //Get the message
                var message = result.Value;
                //Get the bot's user.
                var me = await ctx.BotClient.GetMeAsync();
                //Respond to the command.
                await ctx.RespondAsync($"Hello, {message.Text}! I am {me.FirstName}.");
            }
        }

    }
}
