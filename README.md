# Telegram.Bot Tools
A library that aims to support the C# [Telegram.Bot](https://github.com/TelegramBots/Telegram.Bot) library by:

- [Providing an easier way of handling Telegram commands](/TelegramCommandHandler).
 
- [Providing an easy way to carry out interactions between users and bots](/TelegramInteractivityHelper).

## Samples

For a sample on how to utilize both these libraries, check out the [Sample directory](/Sample).


## p.s. 
It's important to understand. The way Interactivity works is not suitable for high-intensity Bots (thousands of active users). Since waiting takes one thread and one task.
But this approach is very convenient (IMHO) for developing small bots. I guess, hundreds of active users shouldn't be a problem.

For high-load bots, it is better to do the implementation through the “request context” without waiting.
