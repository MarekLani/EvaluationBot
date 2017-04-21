using EvaluationBot.Forms;
using EvaluationBot.Services;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace EvaluationBot.Dialogs
{
    [Serializable]
    public class MainDialog : IDialog<object>
    {
        public string Code;

        private static List<string> FunFacts = new List<string>() { "Human saliva has a boiling point three times that of regular water.", "When hippos are upset, their sweat turns red.", "You cannot snore and dream at the same time.", "Nearly three percent of the ice in Antarctic glaciers is penguin urine.", "The Pokémon Hitmonlee and Hitmonchan are based off of Bruce Lee and Jackie Chan." };

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait<Activity>(MessageReceivedAsync);

        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<Activity> message)
        {

            //botStateClient = activity.GetStateClient();
            //botData = await botStateClient.BotState.GetUserDataAsync(activity.ChannelId, activity.From.Id);
            await sessionEntered(context, null);

        }

        private async Task sessionEntered(IDialogContext context, IAwaitable<string> result)
        {
 
            bool formCompleted;
            if (!context.ConversationData.TryGetValue("FormCompleted", out formCompleted))
            {
                var questionFormDialog = Chain.From(() => FormDialog.FromForm(FeedbackForm.BuildForm, FormOptions.PromptInStart));
                context.Call(questionFormDialog, afterForm);
            }
            else
            {
                await ConfirmReceivedAsync(context, null);
            }
        }

        private async Task afterForm(IDialogContext context, IAwaitable<FeedbackForm> result)
        {
            await context.PostAsync("Thank you for your time. To find out more about this technology visit this website: https://dev.botframework.com/ or github page for code samples: https://github.com/Microsoft/BotBuilder");

            context.ConversationData.SetValue("FormCompleted", true);
            context.ConversationData.SetValue("FunFact", 1);

            var res = await result;

            var ats = new AzureTableService();
            ats.SaveAnswers(res);
            Debug.WriteLine("Answers sent to Azure Table.");

            await ConfirmReceivedAsync(context, null);
        }

        public virtual async Task ConfirmReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {

            int n;
            context.ConversationData.TryGetValue("FunFact", out n);

            string dialog = "";
            if (n > 1)
            {

                if (n < 5)
                {
                    PromptDialog.Confirm(
                       context,
                       FunFactsReqRecieved,
                       "Do you want another *fun fact*?",
                       "Didn't get that! Do you want fun fact?",
                       promptStyle: PromptStyle.Auto
                    );
                }
                else
                {
                    await context.PostAsync("Sorry, we run out of fun facts for now. Thanks again for the feedback and have a great rest of the day!");
                    context.Wait(ConfirmReceivedAsync);
                }
            }
            else
            {
                await context.PostAsync("Thank you for filling up the feedback form. As reward I have few *fun facts* for you:");
                await context.PostAsync(FunFacts[0]);
                context.ConversationData.SetValue("FunFact", 1);
                PromptDialog.Confirm(
                       context,
                       FunFactsReqRecieved,
                       "Do you want another *fun fact*?",
                       "Didn't get that! Do you want fun fact?",
                       promptStyle: PromptStyle.Auto
                    );

            }
        }

        public virtual async Task WaittingForFFReq(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
            int n;
            context.ConversationData.TryGetValue("FunFact", out n);

            if (n <= 5)
            {
                if (message.Text.ToLower().Contains("fun fact") || message.Text.ToLower().Contains("funfact"))
                    await ConfirmReceivedAsync(context, null);
                else
                {
                    await context.PostAsync("Sorry I did not get that. If you want fun fact, just type 'Fun fact'.");
                    context.Wait(WaittingForFFReq);
                }
            }
        }

        private async Task FunFactsReqRecieved(IDialogContext context, IAwaitable<bool> result)
        {
            var confirm = await result;

            int n;
            context.ConversationData.TryGetValue("FunFact", out n);

            if (n < 5 && confirm)
            {

                await context.PostAsync("Fun Fact for you:");
                await context.PostAsync(FunFacts[n]);
                n++;
                context.ConversationData.SetValue("FunFact", n);
                await ConfirmReceivedAsync(context, null);

            }
            else if (!confirm)
            {
                await context.PostAsync("If you will want fun fact, just type 'Fun fact'.");
                context.Wait(WaittingForFFReq);
            }
        }
    }
}