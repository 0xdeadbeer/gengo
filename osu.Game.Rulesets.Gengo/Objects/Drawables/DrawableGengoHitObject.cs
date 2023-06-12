// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osu.Game.Audio;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Gengo.UI.Translation;
using osu.Game.Rulesets.Gengo.Anki;
using osu.Game.Rulesets.Gengo.Cards;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Gengo.Objects.Drawables
{
    public partial class DrawableGengoHitObject : DrawableHitObject<GengoHitObject>, IKeyBindingHandler<GengoAction>
    {
        private const double time_preempt = 600;
        private const double time_fadein = 400;
        public override bool HandlePositionalInput => true;
        public DrawableGengoHitObject(GengoHitObject hitObject)
            : base(hitObject)
        {
            Size = new Vector2(80);
            Origin = Anchor.Centre;
            Position = hitObject.Position;
        }

        [Resolved]
        protected TranslationContainer translationContainer { get; set; }
        [Resolved]
        protected AnkiAPI anki { get; set; }

        private Card assignedCard;
        private Card baitCard;
        
        private Box cardDesign;
        private OsuSpriteText cardText;

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            assignedCard = anki.FetchRandomCard();
            baitCard = anki.FetchRandomCard();

            translationContainer.addCard(assignedCard, baitCard);

            AddInternal(new CircularContainer {
                // RelativeSizeAxes = Axes.Both,
                AutoSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Masking = true,
                CornerRadius = 15f,
                Children = new Drawable[] {
                    cardDesign = new Box {
                        RelativeSizeAxes = Axes.Both,
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Colour = Color4.Black,
                    }, 
                    cardText = new OsuSpriteText {
                        Text = assignedCard.foreignText,
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Colour = Color4.Red,
                        Font = new FontUsage(size: 35f),
                        Margin = new MarginPadding(8f),
                    }
                }
            });
        }

        public override IEnumerable<HitSampleInfo> GetSamples() => new[]
        {
            new HitSampleInfo(HitSampleInfo.HIT_NORMAL)
        };
        protected void ApplyResult(HitResult result) {
            void resultApplication(JudgementResult r) => r.Type = result; 
            ApplyResult(resultApplication);
        }
        GengoAction pressedAction;

        /// <summary>
        /// Checks whether or not the pressed button/action for the current HitObject was correct for (matching to) the assigned card.
        /// </summary>
        bool CorrectActionCheck() {
            if (pressedAction == GengoAction.LeftButton) 
                return translationContainer.leftWordText.Text == assignedCard.translatedText;

            else if (pressedAction == GengoAction.RightButton) 
                return translationContainer.rightWordText.Text == assignedCard.translatedText;

            return false;
        }
        protected override void CheckForResult(bool userTriggered, double timeOffset)
        {
            if (!userTriggered)
            {
                if (!HitObject.HitWindows.CanBeHit(timeOffset)) {
                    translationContainer.removeCard();
                    ApplyResult(r => r.Type = r.Judgement.MinResult);
                }
                return;
            }

            var result = HitObject.HitWindows.ResultFor(timeOffset);
            if (result == HitResult.None)
                return;

            if (!CorrectActionCheck()) {
                translationContainer.removeCard();
                ApplyResult(HitResult.Miss);
                return;
            }

            translationContainer.removeCard();
            ApplyResult(r => r.Type = result);
        }

        protected override double InitialLifetimeOffset => time_preempt;

        protected override void UpdateHitStateTransforms(ArmedState state)
        {
            switch (state)
            {
                case ArmedState.Hit:
                    cardText.FadeColour(Color4.White, 200, Easing.OutQuint);
                    cardDesign.FadeColour(Color4.YellowGreen, 200, Easing.OutQuint);
                    this.ScaleTo(2, 500, Easing.OutQuint).Expire();
                    break;

                default:
                    this.ScaleTo(0.8f, 200, Easing.OutQuint);
                    cardText.FadeColour(Color4.Black, 200, Easing.OutQuint);
                    cardDesign.FadeColour(Color4.Red, 200, Easing.OutQuint);
                    this.FadeOut(500, Easing.InQuint).Expire();
                    break;
            }
        }
        public bool OnPressed(KeyBindingPressEvent<GengoAction> e) {
            if (e.Action != GengoAction.LeftButton && e.Action != GengoAction.RightButton)
                return false; 

            pressedAction = e.Action; 

            return UpdateResult(true);
        }

        public void OnReleased(KeyBindingReleaseEvent<GengoAction> e) {
        }
   }
}
