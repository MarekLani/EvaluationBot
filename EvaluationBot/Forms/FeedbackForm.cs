using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace EvaluationBot.Forms
{
    public enum YesNo
    {
        //[Describe ("Yes")]
        //Yes = 1,
        //[Describe ("No")]
        //No
        [Describe("Áno")]
        Yes = 1,
        [Describe("Nie")]
        No
    }

    public enum InstallAnswer
    {
        [Describe("Yes")]
        Yes = 1,
        [Describe("No")]
        No,
        [Describe("Already installed")]
        AlreadyInstalled
    }

    public enum Satisfaction
    {
        //[Describe("Poor")]
        //Poor = 1,
        //[Describe("Needs Improvement")]
        //NeedsImprovement,
        //[Describe("Satisfactory")]
        //Satisfactory,
        //[Describe("Good")]
        //VeryGood,
        //[Describe("Very good")]
        //Exceptional
        [Describe("Slabé")]
        Poor = 1,
        [Describe("Potrebuje zlepšenie")]
        NeedsImprovement,
        [Describe("Uspokojivé")]
        Satisfactory,
        [Describe("Dobré")]
        VeryGood,
        [Describe("Výborné")]
        Exceptional
    }


    [Serializable]
    [Template(TemplateUsage.EnumSelectOne, "{&} {||}", FieldCase = CaseNormalization.None, ChoiceStyle = ChoiceStyleOptions.Default)]
    public class FeedbackForm
    {

        // [Describe("How interesting was the presented content for you?")]
        [Describe("Nakoľko bol pre vás prezentovaný obsah zaujímavý?")]
        [Template(TemplateUsage.NotUnderstood, "Odpovedi \"{0}\" nerozumiem.")]
        public Satisfaction HowInterestingContent;

        //[Describe("Do you see real space and scenarios for implementation of bots within market you operate in?")]
        [Describe("Vidíte priestor na implementáciu botov?")]
        [Template(TemplateUsage.NotUnderstood, "Odpovedi \"{0}\" nerozumiem.")]
        public YesNo SpaceForImplementing;

        //[Describe("How do you rate presenters abillity to explain the topic?")]
        [Describe("Ako hodnotíte schopnosť prednášajúeho vysvetliť tému?")]
        [Template(TemplateUsage.NotUnderstood, "Odpovedi \"{0}\" nerozumiem.")]
        public Satisfaction ExplainTopic;

        //[Describe("How do you rate presenters presentation skills?")]
        [Describe("Ako hodnotíte prezentačné zručnosti prednášajúceho?")]
        [Template(TemplateUsage.NotUnderstood, "Odpovedi \"{0}\" nerozumiem.")]
        public Satisfaction PresentingSkills;

        //[Describe("How do you rate the presentation in general?")]
        [Describe("Ako hodnotíte prezentáciu vo všeobecnosti")]
        [Template(TemplateUsage.NotUnderstood, "Odpovedi \"{0}\" nerozumiem.")]
        public Satisfaction PresentationGeneral;

        //[Prompt("What was the most interesting part of the presentation for you ? "), Optional]
        [Prompt("Čo bola pre vás najzaujímvajśia časť prezentácie ? "), Optional]
        [Template(TemplateUsage.NotUnderstood, "Odpovedi \"{0}\" nerozumiem.")]
        public string MostInteresting;

        //[Prompt("Where do you see space for improvement (both content and presentation skills)?"), Optional]
        [Prompt("Kde vidíte priestor na zlepšenie"), Optional]
        [Template(TemplateUsage.NotUnderstood, "Odpovedi \"{0}\" nerozumiem.")]
        public string SpaceForImpr;

        public static IForm<FeedbackForm> BuildForm()
        {
            return new FormBuilder<FeedbackForm>()
                //.Message("Hello, Thank you for attending presentation about bots. I would like to ask you for your feedback on the presentation of my creator. (If you will want to go back to previous question, just type 'back')")
                .Message("Dobrý deň, ďakujeme za vašu účasť na prezentácii. Chcel by som Vás poprosiť o spätnú väzbu na prezentáciu môjho stvoriteľa.")
                .Field(nameof(HowInterestingContent))
                .Field(nameof(SpaceForImplementing))
                //.Message("Amazing! Seems like in the future there will be more of us.", (state) => state.SpaceForImplementing == YesNo.Yes)
                .Message("Výborné! Vyzerá to tak, že nás bude v budúcnosti viac.", (state) => state.SpaceForImplementing == YesNo.Yes)
                .Field(nameof(ExplainTopic))
                //.Message("Wow, that's bad. Later I will share a link with you where you can find some more information.", (state) => state.ExplainTopic < Satisfaction.Satisfactory)
                .Message("To je zlé. Na záver vám poskytneme bližšie informácie k bot frameworku", (state) => state.ExplainTopic < Satisfaction.Satisfactory)
                .Field(nameof(PresentingSkills))
                 //.Message("Please, in later step provide feedback, what should be improved.", (state) => state.PresentingSkills <= Satisfaction.Satisfactory)
                 .Message("Prosím v ďalšom kroku uveďte, kde vidíte priestor na zlepšenie", (state) => state.PresentingSkills <= Satisfaction.Satisfactory)
                .Field(nameof(PresentationGeneral))
                .Field(nameof(MostInteresting))
                .Field(nameof(SpaceForImpr))
                .OnCompletion(formComplete)
                .Build();
        }


        //[Describe("How interesting was the presented content for you?")]
        //[Template(TemplateUsage.NotUnderstood, "Odpovedi \"{0}\" nerozumiem.")]
        //public Satisfaction HowInterestingContent;

        //[Describe("Do you plan to install Visual Studio 2017 in near future?")]
        //public InstallAnswer Installation;

        //[Describe("How do you rate presenters' abillity to explain topics?")]

        //public Satisfaction ExplainTopic;

        ////[Describe("How do you rate presenters presenting skills?")]
        ////public Satisfaction PresentingSkills;

        //[Describe("How do you rate Visual Studio 2017 Launch event?")]
        //public Satisfaction EventGeneral;

        //[Prompt("What was the most interesting part of today's event for you ? "), Optional]
        //public string MostInteresting;

        //[Prompt("Where do you see space for improvement?"), Optional]
        //public string SpaceForImpr;

        //public static IForm<FeedbackForm> BuildForm()
        //{
        //    return new FormBuilder<FeedbackForm>()
        //        .Message("Hello, Thank you for attending Visual Studio 2017 Launch. I would like to ask you for your feedback on this event. (If you will want to go back to previous question, just type 'back')")
        //        .Field(nameof(HowInterestingContent))
        //        //.Message("I'm glad to hear that!", (state) => state.HowInterestingContent == true)
        //        //.Field(nameof(WhyNotEnjoy), (state) => state.DidYouEnjoy == false)
        //        //.Message("Feedback taken, we will try and make the experience better.", (state) =>  state.DidYouEnjoy == false && state.WhyNotEnjoy != string.Empty)
        //        .Field(nameof(Installation))
        //        .Message("Amazing! We wish you many succsesfull apps developed with VS 2017.", (state) => state.Installation != InstallAnswer.No)
        //        .Field(nameof(ExplainTopic))
        //        .Field(nameof(EventGeneral))
        //        .Message("Wow, that's bad. Later I will share a link with you where you can find some more information.", (state) => state.ExplainTopic < Satisfaction.Satisfactory)
        //        .Field(nameof(MostInteresting))
        //        .Field(nameof(SpaceForImpr))
        //        .OnCompletion(formComplete)
        //        .Build();
        //}


        private async static Task formComplete(IDialogContext context, FeedbackForm state)
        {
            context.Done(state);
        }
    }
}