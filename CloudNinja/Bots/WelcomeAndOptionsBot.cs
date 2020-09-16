using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using CloudNinja.Cards;

namespace CloudNinja.Bots
{
    public class WelcomeAndOptionsBot<T> : CloudNinjaBot<T> where T: Dialog
    {
        public WelcomeAndOptionsBot(ConversationState conversationState, UserState userState, T dialog, ILogger<CloudNinjaBot<T>> logger)
            : base(conversationState, userState, dialog, logger)
        {

        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {           
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Attachment(ActivityCards.WelcomeCard()), cancellationToken);
                    await turnContext.SendActivityAsync (MessageFactory.SuggestedActions(new List<CardAction>
                    {
                        new CardAction{Title="New Project",Type=ActionTypes.PostBack,Value="New Project"},
                        new CardAction{Title="Existing Project",Type=ActionTypes.PostBack,Value="Existing Project"},
                        new CardAction{Title="Schedule Meeting",Type=ActionTypes.PostBack,Value="Schedule Meeting"}
                    }), cancellationToken);
                }
            }
        }
    }
}
