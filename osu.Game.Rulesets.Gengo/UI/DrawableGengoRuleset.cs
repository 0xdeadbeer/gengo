// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Input;
using osu.Game.Beatmaps;
using osu.Game.Input.Handlers;
using osu.Game.Replays;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Gengo.Objects;
using osu.Game.Rulesets.Gengo.Objects.Drawables;
using osu.Game.Rulesets.Gengo.Replays;
using osu.Game.Rulesets.UI;
using osu.Game.Rulesets.UI.Scrolling;
using osu.Game.Rulesets.Gengo.Anki;
using osu.Game.Rulesets.Gengo.Configuration;

namespace osu.Game.Rulesets.Gengo.UI
{
    [Cached]
    public partial class DrawableGengoRuleset : DrawableScrollingRuleset<GengoHitObject>
    {
        public DrawableGengoRuleset(GengoRuleset ruleset, IBeatmap beatmap, IReadOnlyList<Mod>? mods = null)
            : base(ruleset, beatmap, mods)
        {
        }
        public override PlayfieldAdjustmentContainer CreatePlayfieldAdjustmentContainer() => new GengoPlayfieldAdjustmentContainer();

        protected override Playfield CreatePlayfield() => new GengoPlayfield();

        protected override ReplayInputHandler CreateReplayInputHandler(Replay replay) => new GengoFramedReplayInputHandler(replay);

        public override DrawableHitObject<GengoHitObject> CreateDrawableRepresentation(GengoHitObject h) => new DrawableGengoHitObject(h);

        protected override PassThroughInputManager CreateInputManager() => new GengoInputManager(Ruleset.RulesetInfo);
    }
}
