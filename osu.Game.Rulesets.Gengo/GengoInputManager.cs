// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.ComponentModel;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.UI;

namespace osu.Game.Rulesets.Gengo
{
    public partial class GengoInputManager : RulesetInputManager<GengoAction>
    {
        public GengoInputManager(RulesetInfo ruleset)
            : base(ruleset, 0, SimultaneousBindingMode.Unique)
        {
        }
        protected override KeyBindingContainer<GengoAction> CreateKeyBindingContainer(RulesetInfo ruleset, int variant, SimultaneousBindingMode unique)
            => new GengoKeyBindingContainer(ruleset, variant, unique);
        private partial class GengoKeyBindingContainer : RulesetKeyBindingContainer {
            public bool AllowUserPresses = true;
            public GengoKeyBindingContainer(RulesetInfo ruleset, int variant, SimultaneousBindingMode unique) 
                : base(ruleset, variant, unique) 
            {
            }
            protected override bool Handle(UIEvent e)
            {
                return base.Handle(e);
            }
        }
    }
    
    public enum GengoAction
    {
        [Description("Left Button")]
        LeftButton,

        [Description("Right Button")]
        RightButton,
    }
}
