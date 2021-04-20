using Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime.Models;
using Microsoft.Bot.Builder;

namespace CoreBot
{
    public class ConversationData
    {
        public RecognizerResult RecognizerResult { get; set; }

        public LuisResult LuisResult { get; set; }
    }
}
