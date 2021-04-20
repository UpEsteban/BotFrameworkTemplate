using Microsoft.Bot.Builder.AI.Luis;

namespace CoreBot.Helpers.BotService
{
    public interface IBotServices
    {
        LuisRecognizer Dispatch { get; }
    }
}
