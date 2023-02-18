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
using AutoMind;

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
                        button.CallbackData = "F:" + formula.GetHashCode();
                        buttons.Add(new List<InlineKeyboardButton>() { button });
                    }
                    await botClient.SendTextMessageAsync(
                chatId: ChatId,
                text: print,
                replyMarkup: new InlineKeyboardMarkup(buttons));
                    break;
                case "/properties":
                    print = "Доступные переменные";
                    var propList = new List<List<InlineKeyboardButton>>();
                    foreach (var pr in Bank.Full.Properties)
                    {
                        var button = new InlineKeyboardButton($"{pr.ToView()} - {pr.NameView}({pr.UnitsShort})");
                        button.CallbackData = "PV:" + pr.GetHashCode();
                        propList.Add(new List<InlineKeyboardButton>() { button });
                    }
                    await botClient.SendTextMessageAsync(
                chatId: ChatId,
                text: print,
                replyMarkup: new InlineKeyboardMarkup(propList));
                    break;
                case "/constants":
                    break;
            }
        }
        public async Task NextButton(ITelegramBotClient botClient, CallbackQuery message)
        {
            var print = String.Empty;
            var com = message.Data.Split(':')[0];
            var body = message.Data.Split(':')[1];
            switch (com)
            {
                case "F":
                    var formula = Bank.Full.Functions.FirstOrDefault(i => i.GetHashCode() == int.Parse(body));
                    if (formula == null) return;
                    var via = formula.TotalProperties.Select(formula.ExpressFrom).ToList();
                    via.Remove(null);
                    var stringed = via.Select(i => i.ToView()).ToList();
                    print = $"Производные формулы для\n---\n{formula.ToView()}\n";
                    print += "Где:\n";
                    print += string.Join('\n', formula.TotalProperties.Select(i => $"{i.View} - {i.NameView}, {i.UnitsView}({i.UnitsShort})"));
                    print += "\n---\n";
                    print += string.Join("\n", stringed);
                    await botClient.SendTextMessageAsync(
                    chatId: ChatId,
                    text: print);
                    break;
                case "PV":
                    var hash = body;
                    var viewProp = Bank.Full.Properties.FirstOrDefault(i => i.GetHashCode().ToString() == hash);
                    if (viewProp == null) return;
                    var linkFormula = Bank.EnvField.Get(viewProp).Links.Select(i => i.Data as Formula).ToList();
                    var buttons = new List<List<InlineKeyboardButton>>();
                    foreach (var reFormula in linkFormula)
                    {
                        var button = new InlineKeyboardButton(reFormula.ToView());
                        button.CallbackData = "F:" + reFormula.GetHashCode();
                        buttons.Add(new List<InlineKeyboardButton>() { button });
                    }
                    print = $"Связанные формулы с {viewProp.ToView()}";
                    await botClient.SendTextMessageAsync(
                    chatId: ChatId,
                    text: print,
                    replyMarkup: new InlineKeyboardMarkup(buttons));

                    break;
            }
        }
    }
}
