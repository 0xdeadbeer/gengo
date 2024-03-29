#nullable disable

using System;
using System.Text;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Logging;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Gengo.Cards;
using osu.Game.Graphics.Sprites;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Gengo.UI.Translation 
{
    /// <summary>
    /// Container responsible for showing the two translation words
    /// </summary>
    public partial class TranslationContainer : GridContainer {
        private List<Card> translationsLine = new List<Card>(); 
        private List<Card> fakesLine = new List<Card>(); 
        public OsuSpriteText leftWordText;
        public OsuSpriteText rightWordText;
        [Resolved]
        protected IBeatmap beatmap { get; set; }

        private Random leftRightOrderRandom;

        /// <summary>
        /// Function to update the text of the two translation words (<see cref="leftWordText"/>, <see cref="rightWordText"/>)
        /// </summary>
        public void UpdateWordTexts() {
            if (translationsLine.Count <= 0 || fakesLine.Count <= 0)
                return;

            // Randomly (seeded by the hash of the beatmap) decide whether the left or right word will be the bait/correct translation of the current HitObject 
            if (leftRightOrderRandom.NextDouble() > 0.5) {
                leftWordText.Text = translationsLine[0].translatedText;
                rightWordText.Text = fakesLine[0].translatedText;
            } else {
                leftWordText.Text = fakesLine[0].translatedText;
                rightWordText.Text = translationsLine[0].translatedText;
            }
        }

        /// <summary>
        /// Function to add a new card to record. (function takes a fake card as well)
        /// </summary>
        public void AddCard(Card translationCard, Card fakeCard) {
            translationsLine.Add(translationCard);
            fakesLine.Add(fakeCard);

            if (translationsLine.Count == 1)
                UpdateWordTexts();
        }
        /// <summary>
        /// Function to remove the first card (translation + fake) from their lines
        /// </summary>
        public void RemoveCard() {
            if (translationsLine.Count <= 0)
                return;

            translationsLine.RemoveAt(0);
            fakesLine.RemoveAt(0);

            UpdateWordTexts();
        }

        [BackgroundDependencyLoader]
        public void load() {
            // convert from string -> bytes -> int32
            int beatmapHash = BitConverter.ToInt32(Encoding.UTF8.GetBytes(beatmap.BeatmapInfo.Hash), 0); 
            leftRightOrderRandom = new Random(beatmapHash);

            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;
            Anchor = Anchor.TopCentre; 
            Origin = Anchor.TopCentre;
            ColumnDimensions = new[] { 
                new Dimension(GridSizeMode.Distributed),
                new Dimension(GridSizeMode.Distributed),
            };
            RowDimensions = new[] { new Dimension(GridSizeMode.AutoSize) };

            Content = new[] {
                new[] {
                    new CircularContainer {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        AutoSizeAxes = Axes.Both,
                        Masking = true,
                        CornerRadius = Size.X / 2,
                        CornerExponent = 2,
                        BorderColour = Color4.Black,
                        BorderThickness = 4f,
                        Children = new Drawable[] {
                            new Box {
                                RelativeSizeAxes = Axes.Both,
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                Colour = Color4.Red,
                            },
                            leftWordText = new OsuSpriteText {
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                Colour = Color4.Black,
                                Text = "-",
                                Font = new FontUsage(size: 20f),
                                Margin = new MarginPadding(8f),
                            },
                        },
                    },
                    new CircularContainer {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        AutoSizeAxes = Axes.Both,
                        Masking = true,
                        CornerRadius = Size.X / 2,
                        CornerExponent = 2,
                        BorderColour = Color4.Black,
                        BorderThickness = 4f,
                        Children = new Drawable[] {
                            new Box {
                                RelativeSizeAxes = Axes.Both,
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                Colour = Color4.Red,
                            },
                            rightWordText = new OsuSpriteText {
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                Colour = Color4.Black,
                                Text = "-",
                                Font = new FontUsage(size: 20f),
                                Margin = new MarginPadding(8f),
                            },
                        },
                    }
                }
            };
        }
    }
}