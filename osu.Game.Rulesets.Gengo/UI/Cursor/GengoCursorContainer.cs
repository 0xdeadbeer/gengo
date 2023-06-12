// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Textures;
using osu.Framework.Bindables;
using osu.Framework.Logging;
using osu.Game.Rulesets.UI;
using osu.Game.Configuration;
using osu.Game.Screens.Play;
using osu.Game.Beatmaps;
using osuTK;

namespace osu.Game.Rulesets.Gengo.UI.Cursor
{
    public partial class GengoCursorContainer : GameplayCursorContainer
    {
        public IBindable<float> CursorScale => cursorScale;
        private readonly Bindable<float> cursorScale = new BindableFloat(1);
        private Bindable<float> userCursorScale;
        private Bindable<bool> autoCursorScale;

        [Resolved(canBeNull: true)]
        private GameplayState state { get; set; }

        [Resolved]
        private OsuConfigManager config { get; set; }
        protected override Drawable CreateCursor() => new GengoCursor();

        [BackgroundDependencyLoader]
        private void load(TextureStore textures) {

        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            userCursorScale = config.GetBindable<float>(OsuSetting.GameplayCursorSize);
            userCursorScale.ValueChanged += _ => calculateScale();

            autoCursorScale = config.GetBindable<bool>(OsuSetting.AutoCursorSize);
            autoCursorScale.ValueChanged += _ => calculateScale();

            CursorScale.BindValueChanged(e => {
                var newScale = new Vector2(e.NewValue);
                ActiveCursor.Scale = newScale;
            }, true);
        
            calculateScale();
        }

        /// <summary>
        /// Get the scale applicable to the ActiveCursor based on a beatmap's circle size.
        /// </summary>
        public static float GetScaleForCircleSize(float circleSize) =>
            1f - 0.7f * (1f + circleSize - BeatmapDifficulty.DEFAULT_DIFFICULTY) / BeatmapDifficulty.DEFAULT_DIFFICULTY;

        private void calculateScale() {
            float scale = userCursorScale.Value;

            if (autoCursorScale.Value && state != null) { 
                // if we have a beatmap available, let's get its circle size to figure out an automatic cursor scale modifier.
                scale *= GetScaleForCircleSize(state.Beatmap.Difficulty.CircleSize);
            }

            cursorScale.Value = scale;

            var newScale = new Vector2(scale);

            ActiveCursor.ScaleTo(newScale, 400, Easing.OutQuint);
        }
    }
}
