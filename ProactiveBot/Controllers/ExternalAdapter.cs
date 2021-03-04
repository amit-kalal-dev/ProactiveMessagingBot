using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using BotWebApi.Dtos;
using AdaptiveCards;

namespace ProactiveBot
{
    [Route("api/notify")]
    [ApiController]
    public class ExternalAdapter : ControllerBase
    {
        private IBotFrameworkHttpAdapter _externAdapter;
        private ConcurrentDictionary<string, ConversationReference> _userReference;
        public ExternalAdapter(IBotFrameworkHttpAdapter adapter, ConcurrentDictionary<string, ConversationReference> conReferences)
        {
            _externAdapter = adapter;
            _userReference = conReferences;
        }

        public async Task<IActionResult> Get(StudentReadDto studentReadDto)
        {
            foreach (var item in _userReference.Values)
            {
                await ((BotAdapter)_externAdapter).ContinueConversationAsync("<BOT ID>", item,async
                    (context,token) => await ExternalCallback(context, token, studentReadDto), default);
                 
            }


            var result = new ContentResult();
            result.StatusCode = (int)HttpStatusCode.OK;
            return result;

        }

        private async Task ExternalCallback(ITurnContext turnContext, CancellationToken cancellationToken,StudentReadDto studentReadDto)
        {
            var name = studentReadDto.Name;
            await turnContext.SendActivityAsync(MessageFactory.Attachment(getCard(name)), cancellationToken);
        }
        private Attachment getCard(string name)
        {
            AdaptiveCard card = new AdaptiveCard("1.2")
            {
                Body = new List<AdaptiveElement>()
                {
                   new AdaptiveTextBlock
                   {
                       Text="Api Data",

                   },
                   new AdaptiveTextBlock
                   {
                       Text=name,
                       

                   },
                   
                }

            };
            Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };

            return attachment;
        }
    }
}

