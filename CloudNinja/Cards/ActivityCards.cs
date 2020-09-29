using System.Collections.Generic;
using System.IO;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CloudNinja.Cards
{
    public class ActivityCards
    {
        public static Attachment WelcomeCard()
        {
            var welcomeCard = new HeroCard
            {
                Title = "Cloud Ninja",
                Subtitle = "Cloud Service Automation",
                Text = "Hi there! I'm Cloud Ninja and I provide services related to Azure Cloud platform.",

                Images = new List<CardImage>
                {
                    new CardImage("https://ciklopea.com/wp-content/uploads/2017/07/translator.jpg"),
                }
            };

            return welcomeCard.ToAttachment();
        }

        public static IList<CardAction> InitialOptions()
        {
            var initialOptions = new List<CardAction> 
            {
                    new CardAction{Title="New Project",Type=ActionTypes.PostBack,Value="New Project"},
                    new CardAction{Title="Project Status",Type=ActionTypes.PostBack,Value="Project Status"},
                    new CardAction{Title="Schedule Meeting",Type=ActionTypes.PostBack,Value="Schedule Meeting"},
                    new CardAction{Title="Upload Docs",Type=ActionTypes.PostBack,Value="Upload Docs"}             
            };
            return initialOptions;

        }

        public static Attachment ProjInfoConfirmationCard(string projName, string description, string ownerEmail, string azureServices, string clarityId, string eHLCCD, string costCenter, string billingContact)
        {
            var paths = new[] { ".", "Resources", "ProjInfoConfirmationCard.json" };
            var jsonString = File.ReadAllText(Path.Combine(paths));
            var cardJson = JObject.Parse(jsonString);
            //Below lines of code read the json file and modify its content then present it to the user as a card
            JArray body = (JArray)cardJson["body"];
            JArray projectFactSet = (JArray)(body[1]["facts"]);
            ((JObject)projectFactSet[0])["value"] = projName;
            ((JObject)projectFactSet[1])["value"] = description;
            ((JObject)projectFactSet[2])["value"] = ownerEmail;
            ((JObject)projectFactSet[3])["value"] = azureServices;
            ((JObject)projectFactSet[4])["value"] = clarityId;
            ((JObject)projectFactSet[5])["value"] = eHLCCD;
            ((JObject)projectFactSet[6])["value"] = costCenter;
            ((JObject)projectFactSet[7])["value"] = billingContact;

            var translationResultCard = new Attachment()
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(cardJson.ToString()),
            };
            return translationResultCard;
        }

        public static Attachment CheckProjectStatusCard()
        {
            var paths = new[] { ".", "Resources", "CheckProjectStatusCard.json" };
            var jsonString = File.ReadAllText(Path.Combine(paths));
            var cardJson = JObject.Parse(jsonString);
            //Below lines of code read the json file and modify its content then present it to the user as a card


            var checkProjectStatusCard = new Attachment()
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(cardJson.ToString()),
            };
            return checkProjectStatusCard;
        }

        public static Attachment DisplayProjectStatusCard()
        {
            var paths = new[] { ".", "Resources", "DisplayProjectStatusCard.json" };
            var jsonString = File.ReadAllText(Path.Combine(paths));
            var cardJson = JObject.Parse(jsonString);
            //Below lines of code read the json file and modify its content then present it to the user as a card
           

            var displayProjectStatusCard = new Attachment()
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(cardJson.ToString()),
            };
            return displayProjectStatusCard;
        }

        public static Attachment UploadDocCard()
        {
            var paths = new[] { ".", "Resources", "UploadDocCard.json" };
            var jsonString = File.ReadAllText(Path.Combine(paths));
            var cardJson = JObject.Parse(jsonString);
            //Below lines of code read the json file and modify its content then present it to the user as a card


            var uploadDocCard = new Attachment()
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(cardJson.ToString()),
            };
            return uploadDocCard;
        }
    }
}
