using EvaluationBot.Forms;
using EvaluationBot.Models;
using Microsoft.IdentityModel.Protocols;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace EvaluationBot.Services
{
    public class AzureTableService
    {
        private CloudStorageAccount storageAccount;
        private CloudTableClient tableClient;
        private CloudTable feedbackTable;

        public AzureTableService()
        {
            storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["TableStorage"].ConnectionString);
            tableClient = storageAccount.CreateCloudTableClient();
            feedbackTable = tableClient.GetTableReference("feedback");
        }

        public void SaveAnswers(FeedbackForm form)
        {
            FeedbackEntity fe = new FeedbackEntity("f", Guid.NewGuid().ToString())
            {


                //General event
                //HowInterestingContent = form.HowInterestingContent.ToString(),
                //Installation = form.Installation.ToString(),
                //ExplainTopic = form.ExplainTopic.ToString(),
                //EventGeneral = form.EventGeneral.ToString(),
                //MostInteresting = form.MostInteresting.ToString(),
                //SpaceForImpr = form.SpaceForImpr.ToString()

                //Bot presentation
                HowInterestingContent = form.HowInterestingContent.ToString(),
                ExplainTopic = form.ExplainTopic.ToString(),
                SpaceForImplementing = form.SpaceForImplementing.ToString(),
                PresentingSkills = form.PresentingSkills.ToString(),
                MostInteresting = form.MostInteresting.ToString(),
                SpaceForImpr = form.SpaceForImpr.ToString(),
                PresentationGeneral = form.PresentationGeneral.ToString()

            };

            TableOperation insert = TableOperation.Insert(fe);
            var result = feedbackTable.Execute(insert);
        }
    }     
}