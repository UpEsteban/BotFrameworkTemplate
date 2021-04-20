using CoreBot.Dialogs.SimpleDialog;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.BotBuilderSamples.Dialogs;
using System.Threading.Tasks;

namespace CoreBot.Helpers
{
    public class BotRouter
    {
        public static async Task<DialogTurnResult> Router(WaterfallStepContext stepContext, IStatePropertyAccessor<ConversationData> _conversationStateAccessor)
        {
            var conversationData = await _conversationStateAccessor.GetAsync(stepContext.Context, () => new ConversationData());

            LuisResult lr = conversationData.LuisResult;

            string nextDialog = string.Empty;

            switch (lr.TopScoringIntent.Intent)
            {
                case "TestIntent":
                    await stepContext.Context.SendActivityAsync("TestIntent");
                    break;
                case "SimpleTest":
                    return await stepContext.BeginDialogAsync(nameof(SimpleDialog), new SimpleData());
                default:
                    await stepContext.Context.SendActivityAsync("I'm sorry I didn't understand you.");
                    break;
            }

            return await stepContext.EndDialogAsync();
        }
    }
}
