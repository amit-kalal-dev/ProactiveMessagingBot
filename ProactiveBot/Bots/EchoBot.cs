// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.11.1

using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ProactiveBot.Bots
{
    public class EchoBot : ActivityHandler
    {
        private ConcurrentDictionary<string, ConversationReference> _userConversationReferences;

        public EchoBot(ConcurrentDictionary<string, ConversationReference> userConversationReferences)
        {
            _userConversationReferences = userConversationReferences;
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            AddConversationReference(turnContext.Activity as Activity);
            var replyText = $"Echo: {turnContext.Activity.Text}";
            await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);

        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var welcomeText = "Hello and welcome!";
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
                }
            }
        }
        //protected override Task OnConversationUpdateActivityAsync(ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        //{
        //    if (turnContext.Activity is Activity activity)
        //    {
        //        var conReference = activity.GetConversationReference();

        //        _userConversationReferences.AddOrUpdate(conReference.User.Id, conReference,
        //            (key, newValue) => conReference);
        //    }
        //    return base.OnConversationUpdateActivityAsync(turnContext, cancellationToken);
        //}
        private void AddConversationReference(Activity activity)
        {
            var conversationReference = activity.GetConversationReference();
            _userConversationReferences.AddOrUpdate(conversationReference.User.Id, conversationReference, (key, newValue) => conversationReference);
        }
    }
}
