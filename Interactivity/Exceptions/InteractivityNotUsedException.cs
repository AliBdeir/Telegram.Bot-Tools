using System;
using System.Collections.Generic;
using System.Text;

namespace Interactivity.Exceptions
{
    public class InteractivityNotUsedException : Exception
    {
        public InteractivityNotUsedException() : base("Interactivity isn't used with this client. Consider using TelegramBotClient.UseInteractivity.")
        {

        }
    }
}
