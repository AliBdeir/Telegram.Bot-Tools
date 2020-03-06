using CommandHandler;
using CommandHandler.Attributes;
using CommandHandler.Types;
using System;
using System.Threading.Tasks;

namespace Sample.CommandModules
{
    public class BasicCommands : CommandModule
    {

        // Optional override. Called before a command is executed.
        public async override Task BeforeExecutionAsync(CommandContext ctx)
        {
            Console.WriteLine($"Executing Command...");
            await Task.Delay(0);
        }

        // Optional override. Called after a command is executed.
        public async override Task AfterExecutionAsync(CommandContext ctx)
        {
            Console.WriteLine($"Executed Command.");
            await Task.Delay(0);
        }

        [Command("ping")]
        public async Task PingCommand(CommandContext ctx)
        {
            Console.WriteLine("Ping command request detected.");
            await ctx.RespondAsync($"Pong!");
        }

        [Command("date")]
        public async Task DateCommand(CommandContext ctx)
        {
            Console.WriteLine("Date command request detected.");
            string formattedDate = DateTime.Now.ToString("dddd, dd MMMM yyyy");
            await ctx.RespondAsync($"Today is {formattedDate}");
        }

        [Command("whoami")]
        public async Task WhoAmICommand(CommandContext ctx)
        {
            Console.WriteLine("Who Am I command detected.");
            await ctx.RespondAsync($"You are {ctx.Chat.FirstName} {ctx.Chat.LastName} of Telegram ID {ctx.ChatId}.");
        }

    }
}
