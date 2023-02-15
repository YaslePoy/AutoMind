using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace AMTgBot
{
    internal class BotSession
    {
        public long ChatId;
        public virtual void NextMessage(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken) { }

    }
}
