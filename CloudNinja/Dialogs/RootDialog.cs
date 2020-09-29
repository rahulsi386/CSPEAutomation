using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using CloudNinja.Cards;
using Microsoft.Bot.Schema;
using System.Collections.Generic;
using Bot.Builder.Community.Dialogs.Prompts;
using CloudNinja.Helpers;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace CloudNinja.Dialogs
{
    public class RootDialog : ComponentDialog
    {
        private readonly UserState _userState;
        
        //public AdaptiveCardPrompt UploadDocDialog;

        public RootDialog(UserState userState) : base(nameof(RootDialog))
        {
            _userState = userState;

            AddDialog(new NewProjectDialog());
            AddDialog(new ProjectStatusDialog());
            AddDialog(new UploadDocDialog());
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                InitialStepAsync,
                //FinalStepAsync,
            }));

            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> InitialStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync(
            MessageFactory.SuggestedActions(ActivityCards.InitialOptions()), cancellationToken);

            switch (stepContext.Context.Activity.Text)
            {
                case "New Project":
                    return await stepContext.BeginDialogAsync(nameof(NewProjectDialog), null, cancellationToken);
                case "Project Status":
                    return await stepContext.BeginDialogAsync(nameof(ProjectStatusDialog), null, cancellationToken);         
                case "Upload Docs":
                    return await stepContext.BeginDialogAsync(nameof(UploadDocDialog), null, cancellationToken);
                    
                default:
                    return await stepContext.EndDialogAsync();                    
            }
            //if (stepContext.Context.Activity.Text is "New Project")
            //{
                
            //}
            //else if(stepContext.Context.Activity.Text is "Project Status")
            //{
            //    return await stepContext.BeginDialogAsync(nameof(ExistingProjectDialog), null, cancellationToken);
            //}
            //else if (stepContext.Context.Activity.Text is "Upload Docs")
            //{
            //    return await stepContext.BeginDialogAsync(nameof(UploadDocDialog), null, cancellationToken);
            //}
            //else
            //{
            //    return await stepContext.EndDialogAsync();
            //}
            
        }

        //private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        //{
        //    var projInfo = (ProjectInfo)stepContext.Result;
        //    var accessor = _userState.CreateProperty<ProjectInfo>(nameof(ProjectInfo));
        //    await accessor.SetAsync(stepContext.Context, projInfo, cancellationToken);

        //    return await stepContext.EndDialogAsync(null, cancellationToken);
        //}
    }
}
