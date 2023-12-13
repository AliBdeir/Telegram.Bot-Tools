using Interactivity.Exceptions;
using Interactivity.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Interactivity
{
    public static class TelegramInteractivity
    {
        /// <summary>
        /// The current Message Interactivity processes with this client.
        /// </summary>
        private static List<InteractivityProcess<Message>> _currentMessageInteractivityObjects { get; set; } = new List<InteractivityProcess<Message>>();

        /// <summary>
        /// Current interactivity objects
        /// </summary>
        private static readonly Dictionary<ITelegramBotClient, InteractivityConfiguration> _interactivitiesConfigurations = new Dictionary<ITelegramBotClient, InteractivityConfiguration>();


        /// <summary>
        /// Use Interactivity with this Bot Client.
        /// </summary>
        /// <param name="client">Your TelegramBotClient</param>
        /// <param name="configuration">Interactivity Configuration</param>
        public static void UseInteractivity(this ITelegramBotClient client, InteractivityConfiguration configuration)
        {
            _interactivitiesConfigurations.Add(client, configuration);
        }

        /// <summary>
        /// Wait for a message with a condition.
        /// </summary>
        /// <param name="chat">What Chat to wait for a message in.</param>
        /// <param name="author">Which user to wait for a message from.</param>
        /// <param name="condition">The message's condition. If null, all messages will be accepted as a result.</param>
        /// <param name="defaultTimeoutTimeOverride">Overrides the TelegramInteractivity's configuration's DefaultTimeOutTime.</param>
        /// <returns></returns>
        public static async Task<InteractivityResult<Message>> WaitForMessageAsync(
            this ITelegramBotClient client,
            Chat chat,
            User author,
            Predicate<Message> condition = null,
            TimeSpan? defaultTimeoutTimeOverride = null)
        {

            var configuration = _interactivitiesConfigurations[client];

            // Check if there is already an ongoing process.
            if (_currentMessageInteractivityObjects.Any(x => x.BotId == client.BotId && x.Author.Id == author.Id))
            {
                await client.SendTextMessageAsync(chat, configuration.UserAlreadyHasOngoingOperationMessage);
                return new InteractivityResult<Message>(null, false, false);
            }

            // Get the timeout time.
            var timeOutTime = defaultTimeoutTimeOverride.HasValue ? defaultTimeoutTimeOverride : configuration.DefaultTimeOutTime;

            // Create a new cancellation token for the time out thread.
            var cancellationTokenSource = new CancellationTokenSource();
            // Create a new process object.
            var iObject = new InteractivityProcess<Message>(client.BotId.Value, chat, author, cancellationTokenSource, condition);

            // Add it to the current processes.
            _currentMessageInteractivityObjects.Add(iObject);

            // If the timespan is not infinite/null, create a new thread to cancel the process after the time.
            if (timeOutTime.HasValue)
            {
                var timeSpanValue = timeOutTime.Value;
                new Thread(() =>
                {
                    // Wait until cancelled or time has passed.
                    var cancelled = cancellationTokenSource.Token.WaitHandle.WaitOne(timeSpanValue);
                    // If it hasn't been cancelled
                    if (!cancelled)
                    {
                        _currentMessageInteractivityObjects.Remove(iObject);
                        iObject.InteractivityResult = new InteractivityResult<Message>(null, true, false);
                        iObject.WaitHandle.Set();
                    }
                }).Start();
            }
            // Wait until a result gets passed.
            await iObject.WaitHandle.WaitAsync();
            return iObject.InteractivityResult;
        }


        public static void ClearOnGoingProcesses(this ITelegramBotClient client)
        {
            _currentMessageInteractivityObjects.RemoveAll(x => x.BotId == client.BotId);
        }


        public static async Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var configuration = GetInteractivityConfiguration(botClient);

            // Get the interactivity object of this message.
            var iObject = _currentMessageInteractivityObjects.FirstOrDefault(obj => obj.BotId == botClient.BotId && update.Message.Chat.Id == obj.Chat.Id && (obj.Predicate == null || obj.Predicate.Invoke(update.Message)));

            if (iObject == null)
                return;

            var isCommand = update.Message.Text?.StartsWith(configuration.CommandPrefix) == true;

            // Set its result.
            iObject.InteractivityResult = new InteractivityResult<Message>(update.Message,  
                iObject.InteractivityResult?.IsTimedOut ?? false,
                iObject.InteractivityResult?.IsInterrupted ?? isCommand);

            // If it hasn't timed out
            if (iObject.InteractivityResult?.IsTimedOut == false)
            {
                _currentMessageInteractivityObjects.Remove(iObject);
                iObject.TimeoutThreadToken.Cancel();
                iObject.WaitHandle.Set();
            }
        }

        private static InteractivityConfiguration GetInteractivityConfiguration(ITelegramBotClient botClient)
        {
            if (_interactivitiesConfigurations.TryGetValue(botClient, out var interactivityConfiguration))
                return interactivityConfiguration;

            throw new InteractivityNotUsedException();
        }
    }

}
