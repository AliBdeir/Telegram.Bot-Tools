using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using TelegramCommandHandler.Attributes;
using TelegramCommandHandler.Tools;

namespace TelegramCommandHandler.Extensions
{
    public static class BotClientCommandHelpers
    {

        /// <summary>
        /// Initialize the TelegramCommandHandler with the client
        /// </summary>
        /// <param name="client">Client to initialize commands with</param>
        /// <param name="commandHandler">Command handler</param>
        public static void InitializeCommands(this TelegramBotClient client, TelegramCommandHandler commandHandler)
        {
            client.OnMessage += async (sender, e) =>
            {
                await OnMessageReceived(sender, e, commandHandler);
            };
        }

        private static async Task OnMessageReceived(object sender, MessageEventArgs e, TelegramCommandHandler commandHandler)
        {
            Message message = e.Message;
            if (message?.Text?.StartsWith("/") == true)
            {
                //Command Detected
                string command = e.Message.Text.Substring(1);
                Console.WriteLine("Command detected");
                foreach (var commandModule in commandHandler.CommandModules)
                {
                    var methods = commandModule.GetMethods()
                        .Where(method =>
                            method.GetParameters()
                                .Length == 1
                            && CommandHandlerUtils.IsSameOrSubclass(method.GetParameters()[0].ParameterType, typeof(CommandContext))
                            && method.IsStatic
                            && method.IsAsync()
                            && CommandHandlerUtils.IsSameOrSubclass(method.ReturnType, typeof(Task))
                            && method.GetCustomAttributes(typeof(Command), false)?.Any() == true);
                    foreach (var method in methods)
                    {
                        if (((Command)method.GetCustomAttributes(typeof(Command), false)[0]).CommandInvoker == command)
                        {
                            //Command found, call on its method.
                            await (Task) method.Invoke(Activator.CreateInstance(commandModule), new object[] {
                                new CommandContext(e.Message)
                            });
                        }
                    }
                }
            }
        }

    }
}
