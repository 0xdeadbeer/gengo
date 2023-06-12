#nullable disable

using System;
using System.Text;
using System.Collections.Generic;
using System.Net.Http;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Extensions;
using osu.Framework.Logging;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Gengo.Cards;
using osu.Game.Rulesets.Gengo.Configuration;
using Newtonsoft.Json;

namespace osu.Game.Rulesets.Gengo.Anki 
{
    /// <summary>
    /// Class for connecting to the anki API. 
    /// </summary>
    public partial class AnkiAPI : Component {
        public string URL { get; set; } = "";
        public string ankiDeck{ get; set; } = "";
        public string foreignWordField { get; set; } = "";
        public string translatedWordField { get; set; } = "";
        private List<Card> dueCards = new List<Card>();
        private HttpClient httpClient;
        
        [Resolved]
        protected GengoRulesetConfigManager config { get; set; }
        [Resolved]
        protected IBeatmap beatmap { get; set; }

        private Random hitObjectRandom;
        public AnkiAPI() {
        }

        [BackgroundDependencyLoader]
        public void load() {
            URL = (config?.GetBindable<string>(GengoRulesetSetting.AnkiURL).Value ?? "");
            ankiDeck = (config?.GetBindable<string>(GengoRulesetSetting.DeckName).Value ?? "");
            foreignWordField = (config?.GetBindable<string>(GengoRulesetSetting.ForeignWord).Value ?? "");
            translatedWordField = (config?.GetBindable<string>(GengoRulesetSetting.TranslatedWord).Value ?? "");
            httpClient = new HttpClient();

            // convert from string -> bytes -> int32
            int beatmapHash = BitConverter.ToInt32(Encoding.UTF8.GetBytes(beatmap.BeatmapInfo.Hash), 0); 
            hitObjectRandom = new Random(beatmapHash);
            
            GetDueCardsFull();
        }

        public void GetDueCardsFull() {
            // IDEA: Make the query customizable in the future (i.e. add a settings option for it)
            var requestData = new {
                action = "findCardsFull",
                version = 6,
                parameters = new {
                    query = $"deck:\"{ankiDeck}\" is:due",
                },
            };

            var jsonRequestData = new StringContent(JsonConvert.SerializeObject(requestData));

            var response = httpClient.PostAsync(URL, jsonRequestData).GetResultSafely();
            var responseString = response.Content.ReadAsStringAsync().GetResultSafely();

            dynamic deserializedResponse = JsonConvert.DeserializeObject<dynamic>(responseString);
            foreach (var id in deserializedResponse.result) {
                string foreignWord = id.fields[foreignWordField].value;
                string translatedWord = id.fields[translatedWordField].value;
                string cardId = id.cardId;

                var newCard = new Card(foreignWord, translatedWord, cardId.ToString());

                dueCards.Add(newCard);
            }

            for (int i = 0; i < 10; i++) 
                this.FetchRandomCard();
        }

        public Card FetchRandomCard() {
            int randomIndex = hitObjectRandom.Next(0, dueCards.Count);
            return dueCards[randomIndex];
        }
    }
}