using System;

namespace TelegramCommandHandler.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class Command : Attribute
    {

        private string CommandInvoker { get; set; }

        public Command(string commandInvoker)
        {
            CommandInvoker = commandInvoker;
        }

    }
}
