using Interactivity.Exceptions;
using Interactivity.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Interactivity.Extensions
{
    public static class TelegramBotClientExtensions
    {

        private static TelegramInteractivity currentInteractivity;

        public static void UseInteractivity(this TelegramBotClient client, InteractivityConfiguration configuration)
        {
            var interactivity = new TelegramInteractivity(client, configuration);
            currentInteractivity = interactivity;
        }

        public static TelegramInteractivity GetInteractivity(this TelegramBotClient client)
        {
            return currentInteractivity;
        }

    }
}
