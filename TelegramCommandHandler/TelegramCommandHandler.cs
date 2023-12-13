using CommandHandler.Attributes;
using CommandHandler.Tools;
using CommandHandler.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CommandHandler
{
    public class TelegramCommandHandler
    {
        private static Dictionary<Type, IReadOnlyCollection<MethodInfo>> _modulesAndMethods = new Dictionary<Type, IReadOnlyCollection<MethodInfo>>();

        /// <summary>
        /// All the registered command modules.
        /// </summary>
        public IReadOnlyCollection<Type> RegisteredCommandModules { get; } = _modulesAndMethods.Keys;

        /// <summary>
        /// Prefix for the commands. Defaults to a slach (/)
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// Determines whether or not commands are case-sensitive.
        /// </summary>
        public bool CaseSensitive { get; set; }

        /// <summary>
        /// Create a new telegram command handler.
        /// </summary>
        /// <param name="prefix">Determines what's the prefix for commands. Defaults to a slash (/)</param>
        public TelegramCommandHandler(string prefix = "/", bool caseSensitive = false)
        {
            Prefix = prefix;
            CaseSensitive = caseSensitive;
        }

        /// <summary>
        /// Register a CommandModule subclass as a command class. You can register multiple classes.
        /// </summary>
        /// <typeparam name="T">CommandModule subclass</typeparam>
        public void RegisterCommands<T>() where T : CommandModule
        {
            var commandModuleType = typeof(T);

            _modulesAndMethods[typeof(T)] = commandModuleType
                        .GetMethods()
                        .Where(method =>
                            method.GetParameters()
                                .Length == 1
                            && CommandHandlerUtils.IsSameOrSubclass(method.GetParameters()[0].ParameterType, typeof(CommandContext))
                            && method.IsAsync()
                            && CommandHandlerUtils.IsSameOrSubclass(method.ReturnType, typeof(Task))
                            && method.GetCustomAttributes(typeof(CommandAttribute), false)?.Any() == true)
                        .ToArray();
        }


        public async Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            //Get the message
            Message message = update.Message;
            bool caseSensitive = CaseSensitive;
            //Is it a command?
            if (message?.Text?.StartsWith(Prefix) != true)
                return;

            //Get the command without the prefix
            string command = message.Text.Substring(Prefix.Length);

            //For every command module in the registered command modules
            //For every method that satisfies the condition above
            foreach (var moduleMethodsPair in _modulesAndMethods)
            {
                //Create an instance of the command module
                var commandModule = (CommandModule)Activator.CreateInstance(moduleMethodsPair.Key);
                //Create a command context
                foreach (var method in moduleMethodsPair.Value)
                {
                    var commandAttribute = method.GetCustomAttribute(typeof(CommandAttribute), false) as CommandAttribute;
                    var aliasesAttribute = method.GetCustomAttribute(typeof(AliasesAttribute), false) as AliasesAttribute;

                    //If the method is linked to the sent command
                    if (commandAttribute
                        ?.CommandInvoker?
                        .ToLowerCaseConditioned(caseSensitive)
                            == command.ToLowerCaseConditioned(caseSensitive)
                        || aliasesAttribute
                        ?.Aliases
                        ?.Any(alias => alias.ToLowerCaseConditioned(caseSensitive) == command.ToLowerCaseConditioned(caseSensitive))
                            == true)
                    {
                        //Create a CommandContext to be used
                        CommandContext ctx = new CommandContext(message, botClient);
                        // Call on the Before Execution Async method
                        await commandModule.BeforeExecutionAsync(ctx);
                        //Call on its method.
                        await (Task)method.Invoke(commandModule, new object[] { ctx });
                        //Call on the After Execution Async method
                        await commandModule.AfterExecutionAsync(ctx);
                        //Finish everything up
                        return;
                    }
                }
            }

        }

    }
}
