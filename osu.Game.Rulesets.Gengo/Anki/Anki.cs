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
using osu.Game.Overlays;
using osu.Game.Screens.Play;
using osu.Game.Rulesets.Gengo.Cards;
using osu.Game.Rulesets.Gengo.Configuration;
using osu.Game.Rulesets.Gengo.UI;
using Newtonsoft.Json;
using Microsoft.CSharp.RuntimeBinder;

namespace osu.Game.Rulesets.Gengo.Anki 
{
    /// <summary>
    /// Class for connecting to the anki API. 
    /// </summary>
    public partial class AnkiAPI : Component {
        public string URL { get; set; } 
        public string ankiDeck{ get; set; }
        public string foreignWordField { get; set; } 
        public string translatedWordField { get; set; }
        private List<Card> dueCards = new List<Card>();
        private HttpClient httpClient;
        
        [Resolved]
        protected GengoRulesetConfigManager config { get; set; }
        [Resolved]
        protected IBeatmap beatmap { get; set; }
        [Resolved] 
        protected IDialogOverlay dialogOverlay { get; set; }

        private Random hitObjectRandom;

        /// <summary>
        /// Function checks whether it's possible to send valid requests to the Anki API with current configurations
        /// </summary>
        bool CheckSettings() {
            var requestData = new {
                action = "findCards",
                version = 6,
                parameters = new {
                    query = $"deck:\"{ankiDeck}\" is:due"
                }
            };

            try {
                var jsonRequestData = new StringContent(JsonConvert.SerializeObject(requestData));

                var response = httpClient.PostAsync(URL, jsonRequestData).GetResultSafely();

                response.EnsureSuccessStatusCode();

                var responseString = response.Content.ReadAsStringAsync().GetResultSafely();

                var deserializedResponse = JsonConvert.DeserializeObject<dynamic>(responseString);

                return deserializedResponse.error == null;
            } catch {
                return false;
            }
       }

        [BackgroundDependencyLoader]
        public void load() {
            URL = config?.GetBindable<string>(GengoRulesetSetting.AnkiURL).Value;
            ankiDeck = config?.GetBindable<string>(GengoRulesetSetting.DeckName).Value;
            foreignWordField = config?.GetBindable<string>(GengoRulesetSetting.ForeignWord).Value;
            translatedWordField = config?.GetBindable<string>(GengoRulesetSetting.TranslatedWord).Value;
            httpClient = new HttpClient();

            // convert from string -> bytes -> int32
            int beatmapHash = BitConverter.ToInt32(Encoding.UTF8.GetBytes(beatmap.BeatmapInfo.Hash), 0); 
            hitObjectRandom = new Random(beatmapHash);
            
            if(!CheckSettings()) {
                dialogOverlay.Push(new AnkiConfigurationDialog("It seems like you've misconfigured osu!gengo's settings", "Back to the settings I go.."));
            } else {
                GetDueCardsFull();
            }
        }

        /// <summary>
        /// Function to fetch due cards from the Anki API
        /// </summary>
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

            // If there's an error with the Anki query, create an error dialog
            if (deserializedResponse.error != null) {
                dialogOverlay.Push(new AnkiConfigurationDialog($"Error retrieved from the Anki API: {deserializedResponse.error}", "Go back"));
                return;
            }

            // Try to fill the cards array. If you get a null-reference, it means that the field names configured by the user are wrong
            try {
                foreach (var id in deserializedResponse.result) {
                    string foreignWord = id.fields[foreignWordField].value;
                    string translatedWord = id.fields[translatedWordField].value;
                    string cardId = id.cardId;

                    var newCard = new Card(foreignWord, translatedWord, cardId.ToString());

                    dueCards.Add(newCard);
                }
            }
            catch (RuntimeBinderException) {
                dialogOverlay.Push(new AnkiConfigurationDialog($"Double check if the field names ('{foreignWordField}', '{translatedWordField}') are correct for the cards used in the deck '{ankiDeck}'", "Go back"));
                return;
            }

            // If there's no cards in the array, create an error dialog
            if (dueCards.Count == 0) {
                dialogOverlay.Push(new AnkiConfigurationDialog($"No due cards found in deck '{ankiDeck}' at '{URL}'", "Go back"));
                return;
            }
        }

        /// <summary>
        /// Return random card object from <see cref="dueCards"/>
        /// </summary>
        public Card FetchRandomCard() {
            if (dueCards.Count <= 0) {
                return new Card("NULL", "NULL", "NULL");
            }

            int randomIndex = hitObjectRandom.Next(0, dueCards.Count);
            return dueCards[randomIndex];
        }
    }
}