// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.


#nullable disable
using osu.Framework.Allocation;
using osu.Framework.Logging;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Overlays;
using osu.Game.Rulesets.UI;
using osu.Game.Rulesets.UI.Scrolling;
using osu.Game.Rulesets.Gengo.UI.Cursor;
using osu.Game.Rulesets.Gengo.UI.Translation;
using osu.Game.Rulesets.Gengo.Configuration;
using osu.Game.Rulesets.Gengo.Anki;
using osuTK;

namespace osu.Game.Rulesets.Gengo.UI
{
    [Cached]
    public partial class GengoPlayfield : ScrollingPlayfield
    {
        protected override GameplayCursorContainer CreateCursor() => new GengoCursorContainer();
        public static readonly Vector2 BASE_SIZE = new Vector2(512, 384);

        private FillFlowContainer playfieldContainer = new FillFlowContainer {
            RelativeSizeAxes = Axes.Both,
            Direction = FillDirection.Vertical,
            Spacing = new Vector2(0f, 5f),
        };

        [Cached]
        protected readonly TranslationContainer translationContainer = new TranslationContainer();
        [Cached]
        protected readonly AnkiAPI anki = new AnkiAPI();

        [BackgroundDependencyLoader]
        private void load()
        {
            AddInternal(anki);
            AddInternal(playfieldContainer);

            HitObjectContainer.Anchor = Anchor.TopCentre;
            HitObjectContainer.Origin = Anchor.Centre;

            playfieldContainer.Add(translationContainer);
            playfieldContainer.Add(HitObjectContainer);
        }
    }
}
