// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.BotBuilderSamples.Dialogs
{
    using CoreBot.Dialogs.SimpleDialog;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Schema;
    using Microsoft.Recognizers.Text;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class SimpleDialog : BaseDialog
    {
        public SimpleDialog()
            : base(nameof(SimpleDialog))
        {
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new ConfirmPrompt(nameof(ConfirmPrompt), defaultLocale: Culture.Spanish));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                InitialStepAsync,
                ConfirmationStepAsync,
                ResolveStepAsync,
                FinalStepAsync,
            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> InitialStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var messageText = "What would you like to eat?";
            var promptMessage = MessageFactory.Text(messageText, messageText, InputHints.ExpectingInput);
            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
        }

        private async Task<DialogTurnResult> ConfirmationStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            ((SimpleData)stepContext.Options).Selection = stepContext.Result.ToString();
            var messageText = $"Please confirm that your order is: {stepContext.Result}";
            var promptMessage = MessageFactory.Text(messageText, messageText, InputHints.ExpectingInput);
            return await stepContext.PromptAsync(nameof(ConfirmPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
        }

        private async Task<DialogTurnResult> ResolveStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var messageText = $"It has been added to your order and will arrive shortly.";
            if (!(bool)stepContext.Result)
                return await BackStepAsync(stepContext, 3);

            await stepContext.Context.SendActivityAsync(MessageFactory.Text(messageText), cancellationToken);
            return await stepContext.NextAsync(null, cancellationToken);
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.EndDialogAsync(stepContext.Options, cancellationToken);
        }

        private async Task<DialogTurnResult> BackStepAsync(WaterfallStepContext stepContext, int steps)
        {
            // Change step
            stepContext.Stack.Where(x => x.Id.Equals("WaterfallDialog")).FirstOrDefault().State["stepIndex"] =
                (int)stepContext.Stack.Where(x => x.Id.Equals("WaterfallDialog")).FirstOrDefault().State["stepIndex"] - steps;

            //Globals.DisableContextState = true;
            return await stepContext.ContinueDialogAsync();
        }
    }
}
