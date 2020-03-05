﻿using System;

namespace TelegramCommandHelper.Attributes
{
    /// <summary>
    /// Marks a Task as a Command
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class Command : Attribute
    {

        /// <summary>
        /// Determines what invokes the command to be called
        /// </summary>
        public string CommandInvoker { get; set; }

        /// <summary>
        /// New Command attribute
        /// </summary>
        /// <param name="commandInvoker">Determines what invokes the command to be called, without prefix</param>
        public Command(string commandInvoker)
        {
            CommandInvoker = commandInvoker;
        }

    }
}
