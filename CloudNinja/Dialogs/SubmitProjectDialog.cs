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

        private async Task<DialogTurnResult> SubmitProjectAsync(DialogContext dialogContext, CancellationToken cancellationToken)
        {
            await dialogContext.Context.SendActivityAsync(MessageFactory.Text("Submit project dialog will submit project details to database"), cancellationToken);
            return await dialogContext.EndDialogAsync();
        }
    }
}
