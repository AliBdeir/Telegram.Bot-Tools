using CommandHandler;
using CommandHandler.Extensions;
using Sample.CommandModules;
using System;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Sample
{
    class Program
    {

        static void Main(string[] args) => MainAsync(args).GetAwaiter().GetResult();

        static async Task MainAsync(string[] args)
        {
            var botClient = new TelegramBotClient(Environment.GetEnvironmentVariable("TelegramKey"));
            var me = await botClient.GetMeAsync();
            Console.WriteLine(
              $"Hello, World! I am user {me.Id} and my name is {me.FirstName}."
            );
            var commandHandler = new TelegramCommandHandler();
            commandHandler.RegisterCommands<BasicCommands>();
            botClient.InitializeCommands(commandHandler);
            //MANDATORY
            botClient.StartReceiving();
            Console.ReadKey();
        }
    }
}
