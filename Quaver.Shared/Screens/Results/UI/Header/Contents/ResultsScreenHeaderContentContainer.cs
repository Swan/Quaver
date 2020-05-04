using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Quaver.API.Enums;
using Quaver.API.Helpers;
using Quaver.API.Maps.Processors.Scoring;
using Quaver.Shared.Assets;
using Quaver.Shared.Database.Maps;
using Quaver.Shared.Helpers;
using Quaver.Shared.Screens.Results.UI.Header.Contents.Tabs;
using Quaver.Shared.Screens.Selection.UI.FilterPanel.MapInformation.Metadata;
using Quaver.Shared.Skinning;
using Wobble;
using Wobble.Assets;
using Wobble.Bindables;
using Wobble.Graphics;
using Wobble.Graphics.Sprites;
using Wobble.Graphics.Sprites.Text;
using Wobble.Managers;
using ColorHelper = Quaver.Shared.Helpers.ColorHelper;

namespace Quaver.Shared.Screens.Results.UI.Header.Contents
{
    public class ResultsScreenHeaderContentContainer : Sprite
    {
        /// <summary>
        /// </summary>
        private Map Map { get; }

        /// <summary>
        /// </summary>
        private Bindable<ScoreProcessor> Processor { get; }

        /// <summary>
        /// </summary>
        private Bindable<ResultsScreenTabType> ActiveTab { get; }

        /// <summary>
        /// </summary>
        private ResultsScreenHeaderAvatar Avatar { get; set; }

        /// <summary>
        /// </summary>
        private ResultsTabSelector TabSelector { get; set; }

        /// <summary>
        /// </summary>
        private SpriteTextPlus SongTitle { get; set; }

        /// <summary>
        /// </summary>
        private SpriteTextPlus Difficulty { get; set; }

        /// <summary>
        /// </summary>
        private TextKeyValue Creator { get; set; }

        /// <summary>
        ///     The y spacing between each piece of text
        /// </summary>
        private const int TEXT_SPACING = 8;

        /// <summary>
        /// </summary>
        private Sprite GradeSprite { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="map"></param>
        /// <param name="processor"></param>
        /// <param name="activeTab"></param>
        /// <param name="height"></param>
        public ResultsScreenHeaderContentContainer(Map map, Bindable<ScoreProcessor> processor, Bindable<ResultsScreenTabType> activeTab,
            float height)
        {
            Map = map;
            Processor = processor;
            ActiveTab = activeTab;

            Size = new ScalableVector2(ResultsScreenView.CONTENT_WIDTH, height);
            Alpha = 0f;

            CreateAvatar();
            CreateTabSelector();
            Avatar.Parent = this;

            CreateSongTitle();
            CreateDifficulty();
            CreateCreator();
            CreateGrade();
        }

        /// <summary>
        /// </summary>
        private void CreateAvatar() => Avatar = new ResultsScreenHeaderAvatar(Height)
        {
            Parent = this,
            Y = 10
        };

        /// <summary>
        /// </summary>
        private void CreateTabSelector() => TabSelector = new ResultsTabSelector(ActiveTab, Processor,
            new ScalableVector2(Width - 18, 62))
        {
            Parent = this,
            Alignment = Alignment.BotRight,
        };

        /// <summary>
        /// </summary>
        private void CreateSongTitle() => SongTitle = new SpriteTextPlus(FontManager.GetWobbleFont(Fonts.LatoBlack),
            $"{Map.Artist} - {Map.Title}", 32)
        {
              Parent = this,
              X = Avatar.X + Avatar.Width + 32,
              Y = 20,
        };

        /// <summary>
        /// </summary>
        private void CreateDifficulty()
        {
            var difficulty = Map.DifficultyFromMods(Processor.Value.Mods);

            Difficulty = new SpriteTextPlus(FontManager.GetWobbleFont(Fonts.LatoBlack),
                $"[{Map.DifficultyName}] - {StringHelper.RatingToString(difficulty)}", 26)
            {
                Parent = this,
                X = SongTitle.X,
                Y = SongTitle.Y + SongTitle.Height + TEXT_SPACING,
                Tint = ColorHelper.DifficultyToColor((float) difficulty)
            };
        }

        /// <summary>
        /// </summary>
        private void CreateCreator() => Creator = new TextKeyValue("By: ", Map.Creator, 23,
            ColorHelper.HexToColor("#CACACA"))
        {
            Parent = this,
            X = SongTitle.X,
            Y = Difficulty.Y + Difficulty.Height + TEXT_SPACING,
            Value =
            {
                Tint = ColorHelper.HexToColor("#00D1FF")
            }
        };

        /// <summary>
        /// </summary>
        private void CreateGrade()
        {
            GradeSprite = new Sprite
            {
                Parent = this,
                Alignment = Alignment.BotRight,
                X = -38
            };

            UpdateGradeTextureAndSize();
        }

        /// <summary>
        /// </summary>
        private void UpdateGradeTextureAndSize()
        {
            var grade = Processor.Value.Failed ? Grade.F : GradeHelper.GetGradeFromAccuracy(Processor.Value.Accuracy);

            Texture2D tex;

            // For visual testing purposes
            if (SkinManager.Skin == null)
            {
                var resource = GameBase.Game.Resources.Get($@"Quaver.Resources/Textures/Skins/Shared/Grades/grade-small-{grade.ToString().ToLower()}.png");
                tex = AssetLoader.LoadTexture2D(resource);
            }
            else
            {
                tex = SkinManager.Skin.Grades[grade];
            }

            GradeSprite.Image = tex;

            const int width = 110;

            GradeSprite.Size = new ScalableVector2(width, tex.Height / tex.Width * width);
            GradeSprite.Y = -TabSelector.Height - 22;
        }
    }
}