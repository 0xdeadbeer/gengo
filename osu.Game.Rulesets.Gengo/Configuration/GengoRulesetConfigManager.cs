using osu.Game.Configuration; 
using osu.Game.Rulesets.Configuration;
using osu.Game.Rulesets.UI; 

namespace osu.Game.Rulesets.Gengo.Configuration 
{
    public class GengoRulesetConfigManager : RulesetConfigManager<GengoRulesetSetting> {
        public GengoRulesetConfigManager(SettingsStore? settings, RulesetInfo ruleset, int? variant = null)
            : base(settings, ruleset, variant) 
        {
        }

        protected override void InitialiseDefaults()
        {
            base.InitialiseDefaults();
            SetDefault(GengoRulesetSetting.AnkiURL, "http://localhost:8766");
            SetDefault(GengoRulesetSetting.DeckName, "");
            SetDefault(GengoRulesetSetting.ForeignWord, "ForeignWord");
            SetDefault(GengoRulesetSetting.TranslatedWord, "TranslatedWord");
        }
    }
    
    public enum GengoRulesetSetting {
        AnkiURL, 
        DeckName,
        ForeignWord, 
        TranslatedWord,
    }
}