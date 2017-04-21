using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EvaluationBot.Models
{
    public class FeedbackEntity : TableEntity
    {

        public FeedbackEntity(string objPartitionKey, string objRowKey)
        {
            this.PartitionKey = objPartitionKey;
            this.RowKey = objRowKey;
        }

        public string HowInterestingContent { get; set; }

        public string SpaceForImplementing { get; set; }

        public string ExplainTopic { get; set; }

        public string PresentingSkills { get; set; }

        public string PresentationGeneral { get; set; }

        public string MostInteresting { get; set; }


        public string SpaceForImpr { get; set; }

    }

    //Event general
    //public class FeedbackEntity : TableEntity
    //{

    //    public FeedbackEntity(string objPartitionKey, string objRowKey)
    //    {
    //        this.PartitionKey = objPartitionKey;
    //        this.RowKey = objRowKey;
    //    }

    //    public string HowInterestingContent { get; set; }

    //    public string Installation { get; set; }

    //    public string ExplainTopic { get; set; }

    //    public string EventGeneral { get; set; }

    //    public string MostInteresting { get; set; }


    //    public string SpaceForImpr { get; set; }

    //}
}