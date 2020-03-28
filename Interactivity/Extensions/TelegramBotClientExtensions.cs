using Interactivity.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Interactivity.Extensions
{
    public static class TelegramBotClientExtensions
    {

        private static TelegramInteractivity currentInteractivity;

        public static void UseInteractivity(this TelegramBotClient client, TelegramInteractivity interactivity)
        {
            currentInteractivity = interactivity;
        }

        public static TelegramInteractivity GetInteractivity(this TelegramBotClient client)
        {
            return currentInteractivity;
        }

        public static async Task<InteractivityResult<Message>> WaitForMessageAsync(
            this TelegramBotClient client,
            Predicate<Message> predicate,
            TimeSpan? defaultTimeSpanOverride,
            )

    }
}
