using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CloudNinja.Helpers;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;

namespace CloudNinja.Dialogs
{
    public class ExistingProjectDialog : ComponentDialog
    {
        public ExistingProjectDialog() :base(nameof(ExistingProjectDialog))
        {
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
           {
                SearchProjectAsync,
           }));

            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> SearchProjectAsync(DialogContext dialogContext, CancellationToken cancellationToken)
        {
            await dialogContext.Context.SendActivityAsync(MessageFactory.Text("This is the existing project dialog"),cancellationToken);
            return await dialogContext.EndDialogAsync();
        }
    }
}
