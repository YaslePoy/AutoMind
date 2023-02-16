using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using System.Threading;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;

namespace AMTgBot
{
    internal class BotSession
    {
        public long ChatId;
        public async Task NextMessageAsync(ITelegramBotClient botClient, Message message)
        {
            if (message.Text is not { } messageText)
                return;
            Console.WriteLine($"Received a '{messageText}' message in chat {ChatId}.");

            string print = string.Empty;

            switch (messageText)
            {
                case "/formula":
                    print = "Доступные формулы";
                    var buttons = new List<List<InlineKeyboardButton>>();
                    foreach (var formula in Bank.Full.Functions)
                    {
                        var button = new InlineKeyboardButton(formula.ToView());
                        button.CallbackData = "F" + formula.GetHashCode();
                        buttons.Add(new List<InlineKeyboardButton>() { button });
                    }
                    await botClient.SendTextMessageAsync(
                chatId: ChatId,
                text: print,
                replyMarkup: new InlineKeyboardMarkup(buttons));
                    break;
                case "/properties":
                    break;
                case "/constants":
                    break;
            }
        }
        public async Task NextButton(ITelegramBotClient botClient, CallbackQuery message)
        {
            switch (message.Data[0])
            {
                case 'F':
                    var formula = Bank.Full.Functions.FirstOrDefault(i => i.GetHashCode() == int.Parse(message.Data.Substring(1)));
                    if (formula == null) return;
                    var via = formula.TotalProperties.Select(formula.ExpressFrom).ToList();
                    via.Remove(null);
                    var stringed = via.Select(i => i.ToView()).ToList();
                    var print = $"Производные формулы для\n---\n{formula.ToView()}\n";
                    print += "Где:\n";
                    print += string.Join('\n', formula.TotalProperties.Select(i => $"{i.View} - {i.NameView}, {i.UnitsView}({i.UnitsShort})"));
                    print += "\n---\n";
                    print += string.Join("\n", stringed);
                    await botClient.SendTextMessageAsync(
                    chatId: ChatId,
                    text: print);
                    break;
            }
        }
    }
}
