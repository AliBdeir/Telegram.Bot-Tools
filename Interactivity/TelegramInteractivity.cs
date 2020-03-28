using Interactivity.Exceptions;
using Interactivity.Extensions;
using Interactivity.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace Interactivity
{
    public partial class TelegramInteractivity
    {
        private readonly TelegramBotClient client;
        public List<InteractivityObject<Message>> CurrentMessageInteractivityObjects { get; set; } = new List<InteractivityObject<Message>>();
        public InteractivityConfiguration Configuration { get; set; }

        public TelegramInteractivity(TelegramBotClient client, InteractivityConfiguration configuration)
        {
            this.client = client;
            this.Configuration = configuration;
            Setup();
        }

        private void Setup()
        {
            client.OnMessage += (sender, e) =>
            {
                Handlers.InteractivityMessageHandler.OnMessageSent(sender, e, this);
            };
        }

        public async Task<InteractivityResult<Message>> WaitForMessageAsync(
            Chat chat,
            Predicate<Message> predicate = null,
            TimeSpan? defaultTimeoutTimeOverride = null
            )
        {
            var interactivity = client.GetInteractivity();
            if (interactivity == null)
            {
                throw new InteractivityNotUsedException();
            }
            var timeSpan = defaultTimeoutTimeOverride.HasValue ? defaultTimeoutTimeOverride :
                interactivity.Configuration.DefaultTimeOutTime;
            var cancellationTokenSource = new CancellationTokenSource();
            var iObject = new InteractivityObject<Message>(cancellationTokenSource, chat, predicate);
            interactivity.CurrentMessageInteractivityObjects
                .Add(iObject);
            if (timeSpan.HasValue)
            {
                var timeSpanValue = timeSpan.Value;
                new Thread(() =>
                {
                    var cancelled = cancellationTokenSource.Token.WaitHandle.WaitOne(timeSpanValue);
                    if (!cancelled)
                    {
                        CurrentMessageInteractivityObjects.Remove(iObject);
                        iObject.InteractivityResult = new InteractivityResult<Message>(null, true);
                        iObject.WaitHandle.Set();
                    }
                }).Start();
            }
            await iObject.WaitHandle.WaitAsync();
            return iObject.InteractivityResult;
        }

    }
}
