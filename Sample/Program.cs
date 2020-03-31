using CommandHandler;
using CommandHandler.Extensions;
using Sample.CommandModules;
using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Interactivity.Extensions;

namespace Sample
{
    class Program
    {

        static void Main(string[] args) => MainAsync(args).GetAwaiter().GetResult();

        static async Task MainAsync(string[] args)
        {
            // Create a bot client.
            var botClient = new TelegramBotClient(Environment.GetEnvironmentVariable("TelegramKey"));
            // Get the bot's User.
            var me = await botClient.GetMeAsync();
            Console.WriteLine(
              $"Hello, World! I am user {me.Id} and my name is {me.FirstName}."
            );
            var commandHandler = new TelegramCommandHandler();
            commandHandler.RegisterCommands<BasicCommands>();
            commandHandler.RegisterCommands<InteractivityCommands>();
            botClient.InitializeCommands(commandHandler);
            botClient.UseInteractivity(new Interactivity.Types.InteractivityConfiguration()
            {
                DefaultTimeOutTime = TimeSpan.FromSeconds(5)
            });
            //MANDATORY
            botClient.StartReceiving();
            Console.ReadKey();
        }
    }
}
