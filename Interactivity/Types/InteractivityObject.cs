using Interactivity.Tools;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Telegram.Bot.Types;

namespace Interactivity.Types
{

    public class InteractivityObject<T>
    {

        public TimeSpan TimeSpan { get; set; } = TimeSpan.FromMinutes(2);
        public Chat Chat { get; set; }
        public Predicate<T> Predicate { get; set; }
        public AsyncAutoResetEvent WaitHandle { get; } = new AsyncAutoResetEvent();
        public InteractivityResult<T> InteractivityResult { get; set; }
        public CancellationTokenSource Token { get; private set; }
        public InteractivityObject(CancellationTokenSource token,
            Chat chat,
            Predicate<T> predicate = null)
        {
            this.Token = token;
            this.Chat = chat;
            this.Predicate = predicate;
        }

    }
}
