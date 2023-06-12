using System;

namespace osu.Game.Rulesets.Gengo.Cards 
{
    public class Card : IEquatable<Card> {
        public string foreignText { get; set; }
        public string translatedText { get; set; }
        public string cardID { get; set; }
        public Card(string foreignText, string translatedText, string cardID) {
            this.foreignText = foreignText;
            this.translatedText = translatedText;
            this.cardID = cardID;
        }

        public override bool Equals(object? obj)
        {
            return this.Equals(obj as Card);
        }

        public override int GetHashCode()
        {
            int hash = 0; 
            hash += 31 * foreignText?.GetHashCode() ?? 0;
            hash += 17 * translatedText?.GetHashCode() ?? 0;
            hash += 11 * cardID?.GetHashCode() ?? 0;
            return hash;
        }

        public bool Equals(Card? other) {
            if (other is null)
                return false;

            return (other.foreignText == this.foreignText) && 
                   (other.translatedText == this.translatedText) && 
                   (other.cardID == this.cardID);
        }
    }
}