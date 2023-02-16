using AMTgBot;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

List<BotSession> sessions = new List<BotSession>();


Bank.LoadFull();
var botClient = new TelegramBotClient("6222012143:AAHSY35vk5EwqDgcMsMLSQhn_CJx4dAR6-s");

using CancellationTokenSource cts = new();

// StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
ReceiverOptions receiverOptions = new()
{
    AllowedUpdates = Array.Empty<UpdateType>() // receive all update types
};

botClient.StartReceiving(
    updateHandler: HandleUpdateAsync,
    pollingErrorHandler: HandlePollingErrorAsync,
    receiverOptions: receiverOptions,
    cancellationToken: cts.Token
);

var me = await botClient.GetMeAsync();

Console.WriteLine($"Start listening for @{me.Username}");
Console.ReadLine();

// Send cancellation request to stop bot
cts.Cancel();

async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    if (update.Message is { } message)
    {
        if (message.Text is not { } messageText)
            return;
        var chatId = message.Chat.Id;
        var ses = GetSession(chatId);
        ses.NextMessageAsync(botClient, message);
    }
    if (update.CallbackQuery is { } call)
    {
        var chatId = call.Message.Chat.Id;
        var ses = GetSession(chatId);
        ses.NextButton(botClient, call);
    }
    //Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

    //Message sentMessage = await botClient.SendTextMessageAsync(
    //    chatId: chatId,
    //    text: "You said:\n" + messageText);
}

BotSession GetSession(long id)
{
    var ses = sessions.FirstOrDefault(i => i.ChatId == id);
    if (ses == null)
    {
        ses = new BotSession() { ChatId = id };
        sessions.Add(ses);
    }
    return ses;
}
 
Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{
    var ErrorMessage = exception switch
    {
        ApiRequestException apiRequestException
            => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
        _ => exception.ToString()
    };

    Console.WriteLine(ErrorMessage);
    return Task.CompletedTask;
}
