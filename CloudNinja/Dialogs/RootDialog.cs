using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using CloudNinja.Cards;
using Microsoft.Bot.Schema;
using System.Collections.Generic;
using CloudNinja.Helpers;

namespace CloudNinja.Dialogs
{
    public class RootDialog : ComponentDialog
    {
        private readonly UserState _userState;

        public RootDialog(UserState userState) : base(nameof(RootDialog))
        {
            _userState = userState;
            AddDialog(new NewProjectDialog());
            AddDialog(new ExistingProjectDialog());
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                InitialStepAsync,
                FinalStepAsync,
            }));

            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> InitialStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {           
            await stepContext.Context.SendActivityAsync(
            MessageFactory.SuggestedActions(new List<CardAction>
            {
                new CardAction{Title="New Project",Type=ActionTypes.PostBack,Value="New Project"},
                new CardAction{Title="Existing Project",Type=ActionTypes.PostBack,Value="Existing Project"},
                new CardAction{Title="Schedule Meeting",Type=ActionTypes.PostBack,Value="Schedule Meeting"}
            }), cancellationToken);

            if (stepContext.Context.Activity.Text is "New Project")
            {
                return await stepContext.BeginDialogAsync(nameof(NewProjectDialog), null, cancellationToken);
            }
            else if(stepContext.Context.Activity.Text is "Existing Project")
            {
                return await stepContext.BeginDialogAsync(nameof(ExistingProjectDialog), null, cancellationToken);
            }
            else
            {
                return await stepContext.EndDialogAsync();
            }
            
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var projInfo = (ProjectInfo)stepContext.Result;
            var accessor = _userState.CreateProperty<ProjectInfo>(nameof(ProjectInfo));
            await accessor.SetAsync(stepContext.Context, projInfo, cancellationToken);

            return await stepContext.EndDialogAsync(null, cancellationToken);
        }
    }
}
