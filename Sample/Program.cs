using CommandHandler;
using Interactivity;
using Sample.CommandModules;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

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

            botClient.UseInteractivity(new Interactivity.Types.InteractivityConfiguration()
            {
                DefaultTimeOutTime = TimeSpan.FromMinutes(1)
            });

            using (var cts = new CancellationTokenSource())
            {
                var receiverOptions = new ReceiverOptions { AllowedUpdates = new[] { UpdateType.Message, }, ThrowPendingUpdates = true };

                var handlersPipe = async (ITelegramBotClient botClient, Update update, CancellationToken cancellationToken) =>
                {
                   _ = Task.WhenAll( // Sholdn't be awaited since commandHandler can wait Interactivity
                            TelegramInteractivity.UpdateHandler(botClient, update, cancellationToken),
                            commandHandler.UpdateHandler(botClient, update, cancellationToken)
                        ).ConfigureAwait(false);
                };

                //MANDATORY
                botClient.StartReceiving(handlersPipe, ErrorHandler, receiverOptions, cts.Token);
                Console.ReadKey();
            }
        }

        static Task ErrorHandler(ITelegramBotClient botClient, Exception error, CancellationToken cancellationToken) { return Task.CompletedTask; }
    }
}
