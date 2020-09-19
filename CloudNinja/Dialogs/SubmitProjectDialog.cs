using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CloudNinja.Helpers;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;

namespace CloudNinja.Dialogs
{
    public class SubmitProjectDialog : ComponentDialog
    {
        public SubmitProjectDialog() : base(nameof(SubmitProjectDialog))
        {
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
         {
                SubmitProjectAsync,
         }));

            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> SubmitProjectAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync(MessageFactory.Text("Your Project details are being registered in CSPE database..."), cancellationToken);
            var result = (string)stepContext.Result;
            await stepContext.Context.SendActivityAsync(MessageFactory.Text(result));
            //await AddProjectDetails.InvokeAddProjectDetailsFunction();
            return await stepContext.EndDialogAsync();
        }
    }
}
