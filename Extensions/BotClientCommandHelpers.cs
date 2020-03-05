using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using TelegramCommandHelper.Attributes;
using TelegramCommandHelper.Tools;

namespace TelegramCommandHelper.Extensions
{
    public static class BotClientCommandHandler
    {


        /// <summary>
        /// Initialize the TelegramCommandHandler with the client
        /// </summary>
        /// <param name="client">Client to initialize commands with</param>
        /// <param name="commandHandler">Command handler</param>
        public static void InitializeCommands(this TelegramBotClient client, TelegramCommandHandler commandHandler)
        {

            client.OnMessage += async(sender, e) =>
            {
                await OnMessageReceived(sender, e, client, commandHandler);
            };
        }

        private static async Task OnMessageReceived(object sender, MessageEventArgs e, TelegramBotClient client, TelegramCommandHandler commandHandler)
        {
            Message message = e.Message;
            if (message?.Text?.StartsWith(commandHandler.Prefix) == true)
            {
                //Command Detected
                Console.WriteLine("Command detected");
                string command = e.Message.Text.Substring(commandHandler.Prefix.Length);
                //For every command module in the registered command modules
                foreach (var commandModule in commandHandler.RegisteredCommandModules)
                {
                    //Get all the methods that have on a single CommandContext parameter, is async, has a return type of Task, and has the [Command] attribute.
                    var methods = commandModule.GetMethods()
                        .Where(method =>
                            method.GetParameters()
                                .Length == 1
                            && CommandHandlerUtils.IsSameOrSubclass(method.GetParameters()[0].ParameterType, typeof(CommandContext))
                            && method.IsAsync()
                            && CommandHandlerUtils.IsSameOrSubclass(method.ReturnType, typeof(Task))
                            && method.GetCustomAttributes(typeof(Command), false)?.Any() == true);
                    //For every method that satisfies the condition above
                    foreach (var method in methods)
                    {
                        //If the method is linked to the sent command
                        if (((Command)method.GetCustomAttributes(typeof(Command), false).FirstOrDefault())?.CommandInvoker?.ToLower() == command.ToLower())
                        {
                            //Call on its method.
                            await (Task) method.Invoke(Activator.CreateInstance(commandModule), new object[] {
                                new CommandContext(e.Message, client)
                            });
                            return;
                        }
                    }
                }
            }
        }

    }
}
