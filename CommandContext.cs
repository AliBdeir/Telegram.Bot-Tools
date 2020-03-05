using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;

namespace TelegramCommandHandler
{
    public class CommandContext
    {
        public Message Message { get; set; }
        public int MessageId
        {
            get {
                return Message.MessageId;
            }
        }
        public Chat Chat
        {
            get {
                return Message.Chat;
            }
        }
        public long ChatId
        {
            get {
                return Message.Chat.Id;
            }
        }

        public CommandContext(Message message)
        {
            this.Message = message;
        }

    }
}
