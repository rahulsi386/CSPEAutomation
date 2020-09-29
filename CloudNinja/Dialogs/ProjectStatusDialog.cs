using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CloudNinja.Cards;
using CloudNinja.Helpers;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;



namespace CloudNinja.Dialogs
{
    public class ProjectStatusDialog : ComponentDialog
    {
        private const string ProjectSearchQueryString = "ProjectSearchQueryString";

        public ProjectStatusDialog() : base(nameof(ProjectStatusDialog))
        {
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new ConfirmPrompt(nameof(ConfirmPrompt)));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                GetProjectIdStepAsync,
                GetProjectOwnerEmailStepAsync,
                SearchProjectStepAsync,
                ShowProjectStatusStepAsync,
           }));

            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> GetProjectIdStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {

            stepContext.Values[ProjectSearchQueryString] = new ProjectInfo();
            var promptOptions = new PromptOptions { Prompt = MessageFactory.Text("Enter Project Id (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxx)") };

            // Ask the user to enter the project id.
            return await stepContext.PromptAsync(nameof(TextPrompt), promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> GetProjectOwnerEmailStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // Set the project id to what they entered in response to the project id prompt.
            var projSearchQuery = (ProjectInfo)stepContext.Values[ProjectSearchQueryString];
            projSearchQuery.ProjectId = (string)stepContext.Result;

            var promptOptions = new PromptOptions { Prompt = MessageFactory.Text("Provide project owner's email id") };

            // Ask the user to enter project owner's email id
            return await stepContext.PromptAsync(nameof(TextPrompt), promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> SearchProjectStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var projSearchQuery = (ProjectInfo)stepContext.Values[ProjectSearchQueryString];
            projSearchQuery.ProjectOwnerEmail = (string)stepContext.Result;
            return await stepContext.PromptAsync(nameof(ConfirmPrompt), new PromptOptions
            {
                Prompt = MessageFactory.Text($"Do you want to search our database using Project Id: '{((ProjectInfo)stepContext.Values[ProjectSearchQueryString]).ProjectId}' and Owner's email: '{((ProjectInfo)stepContext.Values[ProjectSearchQueryString]).ProjectOwnerEmail}'?"),
                Choices = new List<Choice>
                {
                    new Choice { Value = "Yes" },
                    new Choice { Value = "No" }
                }
            }, cancellationToken
           );        
        }

        private async Task<DialogTurnResult> ShowProjectStatusStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var projSearchQuery = (ProjectInfo)stepContext.Values[ProjectSearchQueryString];

            if (stepContext.Context.Activity.Text is "Yes")
            {
                string projid = ((ProjectInfo)stepContext.Values[ProjectSearchQueryString]).ProjectId;                
                string owneremail = ((ProjectInfo)stepContext.Values[ProjectSearchQueryString]).ProjectOwnerEmail;
                await stepContext.Context.SendActivityAsync(MessageFactory.Attachment(ActivityCards.DisplayProjectStatusCard()),cancellationToken);
                //string responseResult = await AddProjectDetails.InvokeAddProjectDetailsFunction(projname, desc, owneremail, azservices, clarityid, ehlccd, costcenter, billingcontact);
                //if (responseResult.Contains("Status: OK - Successfully added new project detail in database."))
                //{
                //    await stepContext.Context.SendActivityAsync(MessageFactory.Text("We've registered your project request in CSPE database. Further detail is sent to Project Owner's email id."), cancellationToken);
                //    return await stepContext.EndDialogAsync(stepContext.Values[ProjectInfo], cancellationToken);
                //}
                //else
                //{
                //    await stepContext.Context.SendActivityAsync(MessageFactory.Text(responseResult), cancellationToken);
                return await stepContext.EndDialogAsync();
                //}


            }
            else if (stepContext.Context.Activity.Text is "No")
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("Search operation aborted!"));
                return await stepContext.EndDialogAsync();
            }
            else
            {
                return await stepContext.EndDialogAsync();
            }
        }
    }

   
}

