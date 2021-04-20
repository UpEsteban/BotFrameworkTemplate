// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.BotBuilderSamples.Dialogs
{
    using CoreBot;
    using CoreBot.Dialogs.SimpleDialog;
    using CoreBot.Helpers;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Extensions.Logging;
    using System.Threading;
    using System.Threading.Tasks;

    public class MainDialog : ComponentDialog
    {
        protected readonly ILogger Logger;

        private IStatePropertyAccessor<ConversationData> _conversationStateAccessor;

        public MainDialog(ConversationState conversationState, ILogger<MainDialog> logger)
            : base(nameof(MainDialog))
        {
            _conversationStateAccessor = conversationState.CreateProperty<ConversationData>(nameof(ConversationData));

            Logger = logger;

            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new SimpleDialog());
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                IntroStepAsync,
                ActStepAsync,
                FinalStepAsync,
            }));

            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> IntroStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.NextAsync(null, cancellationToken);
        }

        private async Task<DialogTurnResult> ActStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
                return await BotRouter.Router(stepContext, _conversationStateAccessor);
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.EndDialogAsync(null, cancellationToken);
        }
    }
}
