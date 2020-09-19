using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using CloudNinja.Cards;
using CloudNinja.Helpers;

namespace CloudNinja.Dialogs
{
    public class NewProjectDialog : ComponentDialog
    {
        // Define value names for values tracked inside the dialogs.
        private const string ProjectInfo = "value-projectInfo";

        public NewProjectDialog() : base(nameof(NewProjectDialog))
        {
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            //AddDialog(new SubmitProjectDialog());
            AddDialog(new ConfirmPrompt(nameof(ConfirmPrompt)));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                ProjectNameStepAsync,
                ProjectDescriptionStepAsync,
                ProjectOwnerEmailStepAsync,
                AzureServicesUsedStepAsync,
                ClarityIdStepAsync,
                EHlccdStepAsync,
                CostCenterStepAsync,
                BillingContactStepAsync,
                ConfirmDetailsStepAsync,
                SubmitProjectDetailsStepAsync,
            }));

            InitialDialogId = nameof(WaterfallDialog);
        }


        private static async Task<DialogTurnResult> ProjectNameStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // Create an object in which to collect the user's information within the dialog.
            stepContext.Values[ProjectInfo] = new ProjectInfo();
         
            var promptOptions = new PromptOptions { Prompt = MessageFactory.Text("Enter project name") };

            // Ask the user to enter the project name.
            return await stepContext.PromptAsync(nameof(TextPrompt), promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> ProjectDescriptionStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // Set the project name to what they entered in response to the project name prompt.
            var projInfo = (ProjectInfo)stepContext.Values[ProjectInfo];
            projInfo.ProjectName = (string)stepContext.Result;

            var promptOptions = new PromptOptions { Prompt = MessageFactory.Text("Provide project description") };

            // Ask the user to enter project description.
            return await stepContext.PromptAsync(nameof(TextPrompt), promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> ProjectOwnerEmailStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // Set the description to what they entered in response to the project description prompt.
            var projInfo = (ProjectInfo)stepContext.Values[ProjectInfo];
            projInfo.Description = (string)stepContext.Result;

            var promptOptions = new PromptOptions { Prompt = MessageFactory.Text("Provide project owner's Email Id") };

            // Ask the user to enter project owner's email.
            return await stepContext.PromptAsync(nameof(TextPrompt), promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> AzureServicesUsedStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // Set the project owner's email to what they entered in response to the owner's email prompt.
            var projInfo = (ProjectInfo)stepContext.Values[ProjectInfo];
            projInfo.ProjectOwnerEmail = (string)stepContext.Result;

            var promptOptions = new PromptOptions { Prompt = MessageFactory.Text("Specify the name(comma separated) of Azure services that you will use in your project") };

            // Ask the user to specify the Azure services to be used in the project
            return await stepContext.PromptAsync(nameof(TextPrompt), promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> ClarityIdStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // Set the Azure services to what they entered in response to the Azure services used in project prompt.
            var projInfo = (ProjectInfo)stepContext.Values[ProjectInfo];
            projInfo.AzureServicesUsed = (string)stepContext.Result;

            var promptOptions = new PromptOptions { Prompt = MessageFactory.Text("Provide Clarity Id for this project") };

            // Ask the user to enter the project clarity id
            return await stepContext.PromptAsync(nameof(TextPrompt), promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> EHlccdStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // Set the clarity id to what they entered in response to the clarity id prompt.
            var projInfo = (ProjectInfo)stepContext.Values[ProjectInfo];
            projInfo.ClarityId = (string)stepContext.Result;

            var promptOptions = new PromptOptions { Prompt = MessageFactory.Text("Provide eHLCCD id for this project") };

            // Ask the user to enter the eHLCCD id.
            return await stepContext.PromptAsync(nameof(TextPrompt), promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> CostCenterStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // Set the eHLCCD to what they entered in response to the EHlccd prompt.
            var projInfo = (ProjectInfo)stepContext.Values[ProjectInfo];
            projInfo.eHLCCD = (string)stepContext.Result;

            var promptOptions = new PromptOptions { Prompt = MessageFactory.Text("Specify the project cost center for cross charging") };

            // Ask the user to enter the cost center.
            return await stepContext.PromptAsync(nameof(TextPrompt), promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> BillingContactStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // Set the cost center to what they entered in response to the cost center prompt.
            var projInfo = (ProjectInfo)stepContext.Values[ProjectInfo];
            projInfo.CostCenter = (string)stepContext.Result;

            var promptOptions = new PromptOptions { Prompt = MessageFactory.Text("Provide email id of billing contact") };

            // Ask the user to enter the email id of billing contact
            return await stepContext.PromptAsync(nameof(TextPrompt), promptOptions, cancellationToken);
        }

        //private async Task<DialogTurnResult> StartSelectionStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        //{
        //    // Set the user's age to what they entered in response to the age prompt.
        //    var userProfile = (UserProfile)stepContext.Values[UserInfo];
        //    userProfile.Age = (int)stepContext.Result;

        //    if (userProfile.Age < 25)
        //    {
        //        // If they are too young, skip the review selection dialog, and pass an empty list to the next step.
        //        await stepContext.Context.SendActivityAsync(
        //            MessageFactory.Text("You must be 25 or older to participate."),
        //            cancellationToken);
        //        return await stepContext.NextAsync(new List<string>(), cancellationToken);
        //    }
        //    else
        //    {
        //        // Otherwise, start the review selection dialog.
        //        return await stepContext.BeginDialogAsync(nameof(ReviewSelectionDialog), null, cancellationToken);
        //    }
        //}

        private async Task<DialogTurnResult> ConfirmDetailsStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // Set the billing contact email to what they entered in response to the billing contact.
            var projInfo = (ProjectInfo)stepContext.Values[ProjectInfo];
            projInfo.BillingContactEmail = (string)stepContext.Result;
            await stepContext.Context.SendActivityAsync(
               MessageFactory.Attachment(
                   ActivityCards.ProjInfoConfirmationCard(
                       ((ProjectInfo)stepContext.Values[ProjectInfo]).ProjectName,
                       ((ProjectInfo)stepContext.Values[ProjectInfo]).Description,
                       ((ProjectInfo)stepContext.Values[ProjectInfo]).ProjectOwnerEmail,
                       ((ProjectInfo)stepContext.Values[ProjectInfo]).AzureServicesUsed,
                       ((ProjectInfo)stepContext.Values[ProjectInfo]).ClarityId,
                       ((ProjectInfo)stepContext.Values[ProjectInfo]).eHLCCD,
                       ((ProjectInfo)stepContext.Values[ProjectInfo]).CostCenter,
                       ((ProjectInfo)stepContext.Values[ProjectInfo]).BillingContactEmail
                       )),cancellationToken);

            //Confirm before submitting project details and adding to the database
           return await stepContext.PromptAsync(nameof(ConfirmPrompt), new PromptOptions
            {
                Prompt = MessageFactory.Text("Do you want to Submit this Project details?"),
                Choices = new List<Choice>
                {
                    new Choice { Value = "Yes" },
                    new Choice { Value = "No" }
                }
            }, cancellationToken
           );
            //return await stepContext.NextAsync(stepContext.Values[ProjectInfo], cancellationToken);
        }

        private async Task<DialogTurnResult> SubmitProjectDetailsStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var projInfo = (ProjectInfo)stepContext.Values[ProjectInfo];

            if (stepContext.Context.Activity.Text is "Yes")
            {                
                string projname = ((ProjectInfo)stepContext.Values[ProjectInfo]).ProjectName;
                string desc = ((ProjectInfo)stepContext.Values[ProjectInfo]).Description;
                string owneremail = ((ProjectInfo)stepContext.Values[ProjectInfo]).ProjectOwnerEmail;
                string azservices = ((ProjectInfo)stepContext.Values[ProjectInfo]).AzureServicesUsed;
                string clarityid = ((ProjectInfo)stepContext.Values[ProjectInfo]).ClarityId;
                string ehlccd = ((ProjectInfo)stepContext.Values[ProjectInfo]).eHLCCD;
                string costcenter = ((ProjectInfo)stepContext.Values[ProjectInfo]).CostCenter;
                string billingcontact = ((ProjectInfo)stepContext.Values[ProjectInfo]).BillingContactEmail;

                string responseResult= await AddProjectDetails.InvokeAddProjectDetailsFunction(projname, desc, owneremail, azservices, clarityid, ehlccd, costcenter, billingcontact);
                if (responseResult.Contains("Status: OK - Successfully added new project detail in database."))
                {
                    await stepContext.Context.SendActivityAsync(MessageFactory.Text("We've registered your project request in CSPE database. Further detail is sent to Project Owner's email id."), cancellationToken);
                    return await stepContext.EndDialogAsync(stepContext.Values[ProjectInfo], cancellationToken);
                }
                else
                {
                    await stepContext.Context.SendActivityAsync(MessageFactory.Text(responseResult),cancellationToken);
                    return await stepContext.EndDialogAsync();
                }

                
            }
            else if(stepContext.Context.Activity.Text is "No")
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("All the entries will be discarded."));
                return await stepContext.PromptAsync(nameof(ConfirmPrompt), new PromptOptions
                {
                    Prompt = MessageFactory.Text("Are you sure, you want to discard Project details?"),
                    Choices = new List<Choice>
                {
                    new Choice { Value = "Yes" },
                    new Choice { Value = "No" }
                }
                }, cancellationToken);
            }
            else
            {
                return await stepContext.EndDialogAsync(stepContext.Values[ProjectInfo], cancellationToken);
            }
            
        }
    }
}
