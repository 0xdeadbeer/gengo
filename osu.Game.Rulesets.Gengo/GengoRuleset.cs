// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Input.Bindings;
using osu.Game.Beatmaps;
using osu.Game.Configuration;
using osu.Game.Rulesets.Difficulty;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Gengo.Beatmaps;
using osu.Game.Rulesets.Gengo.Mods;
using osu.Game.Rulesets.Gengo.UI;
using osu.Game.Rulesets.UI;
using osu.Game.Rulesets.Gengo.Configuration;
using osu.Game.Rulesets.Configuration;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Gengo.Anki;

namespace osu.Game.Rulesets.Gengo
{
    public class GengoRuleset : Ruleset
    {
        public override string Description => "osu!gengo";

        public override DrawableRuleset CreateDrawableRulesetWith(IBeatmap beatmap, IReadOnlyList<Mod>? mods = null) =>
            new DrawableGengoRuleset(this, beatmap, mods);
        public override IBeatmapConverter CreateBeatmapConverter(IBeatmap beatmap) =>
            new GengoBeatmapConverter(beatmap, this);

        public override DifficultyCalculator CreateDifficultyCalculator(IWorkingBeatmap beatmap) =>
            new GengoDifficultyCalculator(RulesetInfo, beatmap);

        public override IEnumerable<Mod> GetModsFor(ModType type)
        {
            switch (type)
            {
                case ModType.Automation:
                    return new[] { new GengoModAutoplay() };

                default:
                    return Array.Empty<Mod>();
            }
        }

        public override string ShortName => "gengo";

        public override IEnumerable<KeyBinding> GetDefaultKeyBindings(int variant = 0) => new[]
        {
            new KeyBinding(InputKey.A, GengoAction.LeftButton),
            new KeyBinding(InputKey.S, GengoAction.RightButton),
        };

        public override Drawable CreateIcon() => new GengoRulesetIcon(this);
        public override RulesetSettingsSubsection CreateSettings() => new GengoSettingsSubsection(this);

        public override IRulesetConfigManager CreateConfig(SettingsStore? settings) => new GengoRulesetConfigManager(settings, RulesetInfo);

        // Leave this line intact. It will bake the correct version into the ruleset on each build/release.
        public override string RulesetAPIVersionSupported => CURRENT_RULESET_API_VERSION;
    }
}
