using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Connector;
using System.Diagnostics;
using Microsoft.Bot.Builder.Dialogs;
using EvaluationBot.Dialogs;

namespace EvaluationBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {


            if (activity.Type == ActivityTypes.Message)
            {
                //activity.Locale = "sk-SK";
                //activity.Locale = "cs-CZ";
                if (activity.Text == "resetr")
                {
                    //Debug.WriteLine(activity.GetStateClient().BotState.GetConversationData(activity.ChannelId, activity.Conversation.Id).Data);
                    //This is where the conversation gets reset!
                    //activity.GetStateClient().BotState.DeleteStateForUser(activity.ChannelId, activity.From.Id);
                    try
                    {

                        //CachingBotDataStoreConsistencyPolicy.LastWriteWins

                        activity.GetStateClient().BotState.SetConversationData(activity.ChannelId, activity.Conversation.Id, new BotData());
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);
                    }
                }
                else
                {

                    try
                    {
                        //Bot is typying
                        ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                        Activity reply = activity.CreateReply();
                        reply.Type = ActivityTypes.Typing;
                        reply.Text = null;
                        //reply.Locale = "sk-SK";
                        await connector.Conversations.ReplyToActivityAsync(reply);
                        await Conversation.SendAsync(activity, () => new MainDialog());
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);
                    }
                }
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }

    }
}