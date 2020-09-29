using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CloudNinja.Cards;
using CloudNinja.Helpers;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Bot.Builder.Community.Dialogs.Prompts;
using System.Net;
using Microsoft.Bot.Schema;
using Newtonsoft.Json.Linq;
using System.Linq;
using Microsoft.Bot.Builder.Dialogs.Choices;
using CloudNinjaBot.Helpers;

namespace CloudNinja.Dialogs
{
    public class UploadDocDialog : ComponentDialog
    {
        private const string uploadDocInfo = "value-uploadDocInfo";
        public UploadDocDialog() : base(nameof(UploadDocDialog))
        {
            //var promptSettings = new AdaptiveCardPromptSettings();
            //promptSettings.Card = ActivityCards.UploadDocCard();
            //promptSettings.RequiredInputIds = new string[] { "projectid", "filepath" };
            //promptSettings.PromptId = "docuploadprompt";
            //AddDialog(new AdaptiveCardPrompt(nameof(AdaptiveCardPrompt),promptSettings));

            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
            AddDialog(new ConfirmPrompt(nameof(ConfirmPrompt)));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {   
                GetProjectIdStepAsync,
                GetDocTypeStepAsync,
                GetFilePathStepAsync,
                ConfirmFileUploadStepAsync,   
                UploadDocStepAsync,
            }));

            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> GetProjectIdStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            stepContext.Values[uploadDocInfo] = new UploadDocDetails();
            var promptOptions = new PromptOptions { Prompt = MessageFactory.Text("Enter Project Id") };

            // Ask the user to enter the project name.
            return await stepContext.PromptAsync(nameof(TextPrompt), promptOptions, cancellationToken);

            //return await stepContext.PromptAsync(nameof(AdaptiveCardPrompt), new PromptOptions());
        }

        private async Task<DialogTurnResult> GetDocTypeStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // Set the project id to what they entered in response to the project id prompt.
            var upldDocInfo = (UploadDocDetails)stepContext.Values[uploadDocInfo];
            upldDocInfo.ProjectId = (string)stepContext.Result;

            // Ask the user to enter document type to upload.
            return await stepContext.PromptAsync(nameof(ChoicePrompt), new PromptOptions
            {
                Choices = new List<Choice>
                {
                    new Choice{Value="Visio Design"},
                    new Choice{Value="Sizing-Config Doc"},
                    new Choice{Value="Deployment Doc"},
                    new Choice{Value="Handover Doc"}
                },
                Prompt = MessageFactory.Text("Select document type to upload"),
                Style = ListStyle.HeroCard
            }, cancellationToken);
        }

        private async Task<DialogTurnResult> GetFilePathStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var upldDocInfo = (UploadDocDetails)stepContext.Values[uploadDocInfo];
            upldDocInfo.DocType = ((FoundChoice)stepContext.Result).Value;
            var promptOptions = new PromptOptions { Prompt = MessageFactory.Text("Provide full path of the file to upload") };

            // Ask the user to enter the full path of the file to upload.
            return await stepContext.PromptAsync(nameof(TextPrompt), promptOptions, cancellationToken);

            //return await stepContext.PromptAsync(nameof(AdaptiveCardPrompt), new PromptOptions());
        }

        private async Task<DialogTurnResult> ConfirmFileUploadStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var upldDocInfo = (UploadDocDetails)stepContext.Values[uploadDocInfo];
            upldDocInfo.FilePath = (string)stepContext.Result;

            await stepContext.Context.SendActivityAsync(
                MessageFactory.Text(
                    $"Project ID: {((UploadDocDetails)stepContext.Values[uploadDocInfo]).ProjectId};\t" +
                    $"Document Type: {((UploadDocDetails)stepContext.Values[uploadDocInfo]).DocType};\t" +
                    $"File Path: {((UploadDocDetails)stepContext.Values[uploadDocInfo]).FilePath}"), cancellationToken);
            // Ask the user to enter the full path of the file to upload.
            return await stepContext.PromptAsync(nameof(ConfirmPrompt), new PromptOptions
            {
                Prompt = MessageFactory.Text("Are you sure you want to upload this file?"),
                Choices = new List<Choice>
                {
                    new Choice { Value = "Yes" },
                    new Choice { Value = "No" }
                }
            }, cancellationToken
          );

            //return await stepContext.PromptAsync(nameof(AdaptiveCardPrompt), new PromptOptions());
        }

        private async Task<DialogTurnResult> UploadDocStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
           // var upldDocInfo = (UploadDocDetails)stepContext.Values[uploadDocInfo];
            //await stepContext.Context.SendActivityAsync(MessageFactory.Text("Document upload to BLOB initiated!"), cancellationToken);
            if (stepContext.Context.Activity.Text is "Yes")
            {
                string projid = ((UploadDocDetails)stepContext.Values[uploadDocInfo]).ProjectId;
                string doctype = ((UploadDocDetails)stepContext.Values[uploadDocInfo]).DocType;
                string filepath = ((UploadDocDetails)stepContext.Values[uploadDocInfo]).FilePath;
                string filename = $"{projid}-{doctype}.txt";
                await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Document upload to BLOB in-progress!; {filename}"), cancellationToken);
                string responseResult = await UploadDocToBlob.InvokeUploadDocToBlobFunction(filename, filepath);
                await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Function response: {responseResult}"), cancellationToken);
                if (responseResult.Contains("Status: OK - Successfully added to BLOB."))
                {
                    await stepContext.Context.SendActivityAsync(MessageFactory.Text("Document uploaded to Blob successfull!"), cancellationToken);
                    return await stepContext.EndDialogAsync();
                }
                else
                {
                    await stepContext.Context.SendActivityAsync(MessageFactory.Text(responseResult), cancellationToken);
                    return await stepContext.EndDialogAsync();
                }


            }
            //else if (stepContext.Context.Activity.Text is "No")
            //{            
            //    return await stepContext.PromptAsync(nameof(ConfirmPrompt), new PromptOptions
            //    {
            //        Prompt = MessageFactory.Text("Are you sure you want to abort file upload?"),
            //        Choices = new List<Choice>
            //    {
            //        new Choice { Value = "Yes" },
            //        new Choice { Value = "No" }
            //    }
            //    }, cancellationToken);
                
            //}
            else
            {
                return await stepContext.EndDialogAsync();
            }
            
        }

        //private async Task<DialogTurnResult> GetDocDetailStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        //{
        //   //return await stepContext.PromptAsync(nameof(AdaptiveCardPrompt), new PromptOptions());
        //}
        //private async Task<DialogTurnResult> UploadDocStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        //{
        //    var uploadDocInfoResult = stepContext.Result as JObject;
        //    var resultArray = uploadDocInfoResult.Properties().Select(p => $"{p.Name} : {p.Value}");
        //    var resultString = string.Join("\n", resultArray);
        //    await stepContext.Context.SendActivityAsync(MessageFactory.Text(resultString), cancellationToken);
        //    return await stepContext.EndDialogAsync();

        //}
    }

    public class UploadDocDetails
    {
        public string ProjectId { get; set; }
        public string DocType { get; set; }
        public string FilePath { get; set; }
    }
}
