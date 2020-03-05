using System;
using System.Collections.Generic;
using System.Text;

namespace TelegramCommandHandler
{
    public class TelegramCommandHandler
    {

        public readonly List<Type> CommandModules = new List<Type>();

        public TelegramCommandHandler() { }

        public void RegisterCommands<T>(T commandModule) where T : CommandModule
        {
            CommandModules.Add(commandModule.GetType());
        }

    }
}
