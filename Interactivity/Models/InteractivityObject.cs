using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Telegram.Bot.Types;

namespace Interactivity.Models
{

    public interface IInteractivityObject { }

    public class InteractivityObject<T> : IInteractivityObject
    {

        public TimeSpan TimeSpan { get; set; } = TimeSpan.FromMinutes(2);
        public Chat Chat { get; set; }
        public Predicate<T> Predicate { get; set; }
        public EventWaitHandle WaitHandle { get; set; } = new EventWaitHandle(false, EventResetMode.AutoReset);

        public InteractivityObject(Chat chat, Predicate<T> predicate)
        {
            this.Chat = chat;
            this.Predicate = predicate;
        }

    }
}
