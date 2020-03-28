using Interactivity.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace Interactivity.Handlers
{
    public class InteractivityMessageHandler
    {
        public static void OnMessageSent(object sender, MessageEventArgs e, TelegramInteractivity interactivity)
        {
            if (e.Message?.Text == null || e.Message.Text.StartsWith(interactivity.Configuration.CommandPrefix))
            {
                return;
            }
            var iObject = interactivity.CurrentMessageInteractivityObjects
                .FirstOrDefault(obj =>
                    e.Message.Chat.Id == obj.Chat.Id
                    && (obj.Predicate == null || obj.Predicate.Invoke(e.Message)));
            if (iObject != null)
            {
                iObject.InteractivityResult = new InteractivityResult<Message>(e.Message, iObject.InteractivityResult?.TimedOut ?? false);
                if (iObject.InteractivityResult?.TimedOut == null
                    || iObject.InteractivityResult.TimedOut == false)
                {
                    interactivity.CurrentMessageInteractivityObjects.Remove(iObject);
                    iObject.Token.Cancel();
                    iObject.WaitHandle.Set();
                }
            }
        }

    }
}
