using osu.Framework.Allocation;
using osu.Framework.Localisation;
using osu.Framework.Graphics;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Gengo.Configuration;
using osu.Game.Rulesets.UI;

namespace osu.Game.Rulesets.Gengo.UI 
{
    public partial class GengoSettingsSubsection : RulesetSettingsSubsection {
        protected override LocalisableString Header => "osu!gengo";

        public GengoSettingsSubsection(Ruleset ruleset) 
            : base(ruleset) 
        {
        }

        [BackgroundDependencyLoader]
        private void load() {
            var config = (GengoRulesetConfigManager)Config; 

            Children = new Drawable[] {
                new SettingsTextBox {
                    LabelText = "Anki URL (API)",
                    Current = config.GetBindable<string>(GengoRulesetSetting.AnkiURL)
                }, 
                new SettingsTextBox {
                    LabelText = "Anki Deck",
                    Current = config.GetBindable<string>(GengoRulesetSetting.DeckName)
                }, 
                new SettingsTextBox {
                    LabelText = "Foreign Word Field",
                    Current = config.GetBindable<string>(GengoRulesetSetting.ForeignWord)
                }, 
                new SettingsTextBox {
                    LabelText = "Translated Word Field",
                    Current = config.GetBindable<string>(GengoRulesetSetting.TranslatedWord)
                }
            };
        }
    }
}