using Interactivity.Models;
using System.Collections.Generic;
using System.Threading;
using Telegram.Bot;

namespace Interactivity
{
    public class TelegramInteractivity
    {
        private TelegramBotClient client;
        private List<IInteractivityObject> CurrentObjects = new List<IInteractivityObject>();
        public TelegramInteractivity(TelegramBotClient client)
        {
            this.client = client;
            Setup();
        }

        private void Setup()
        {
            client.OnMessage += (sender, e) =>
            {
                
            };
        }

    }
}
