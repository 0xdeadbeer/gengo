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

        public void updateWordTexts() {
            if (translationsLine.Count <= 0 || fakesLine.Count <= 0)
                return;

            // randomly (seed from beatmap) decide whether the left or right word will be the bait/correct translation of the current hitobject
            if (leftRightOrderRandom.NextDouble() > 0.5) {
                leftWordText.Text = translationsLine[0].translatedText;
                rightWordText.Text = fakesLine[0].translatedText;
            } else {
                leftWordText.Text = fakesLine[0].translatedText;
                rightWordText.Text = translationsLine[0].translatedText;
            }
        }
        public void addCard(Card translationCard, Card fakeCard) {
            // translationsLine.Insert(0, translationCard);
            // fakesLine.Insert(0, fakeCard);
            translationsLine.Add(translationCard);
            fakesLine.Add(fakeCard);

            if (translationsLine.Count == 1)
                updateWordTexts();
        }
        public void removeCard() {
            if (translationsLine.Count <= 0)
                return;

            translationsLine.RemoveAt(0);
            fakesLine.RemoveAt(0);

            updateWordTexts();
        }

        public TranslationContainer() {
        }

        [BackgroundDependencyLoader]
        public void load() {
            // convert from string -> bytes -> int32
            int beatmapHash = BitConverter.ToInt32(Encoding.UTF8.GetBytes(beatmap.BeatmapInfo.Hash), 0); 
            leftRightOrderRandom = new Random(beatmapHash);

            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;
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