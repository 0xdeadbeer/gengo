using System;
using osu.Framework.Allocation;
using osu.Framework.Screens;
using osu.Game.Overlays;
using osu.Game.Overlays.Dialog;
using osu.Game.Screens;
using osu.Game.Screens.Select;
using osu.Game.Screens.Play;

namespace osu.Game.Rulesets.Gengo.UI {
    public partial class AnkiConfigurationDialog : PopupDialog {

        [Resolved]
        protected IPerformFromScreenRunner? screen { get; set; }

        public AnkiConfigurationDialog(string bodyText, string cancelText) {
            HeaderText = "Whoops..";
            BodyText = bodyText;
            
            Buttons = new PopupDialogButton[] {
                new PopupDialogOkButton {
                    Text = cancelText,
                    Action = () => { 
                        screen?.PerformFromScreen(game => game.Exit(), new [] { 
                            typeof(PlayerLoader)
                        });
                    }
                }
            };
        }
    }
}