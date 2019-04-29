using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Quaver.Shared.Assets;
using Quaver.Shared.Graphics.Containers;
using Quaver.Shared.Screens;
using Wobble.Assets;
using Wobble.Graphics;
using Wobble.Graphics.Sprites;
using Wobble.Graphics.UI.Buttons;
using Wobble.Screens;
using Wobble.Window;

namespace Quaver.Tests.Desktop.Screens.Testing.UI
{
    public class TestingBrowser : ScrollContainer
    {
        private Dictionary<string, Func<Screen>> Screens { get; }

        /// <summary>
        /// </summary>
        private List<ImageButton> Buttons { get; } = new List<ImageButton>();

        /// <summary>
        /// </summary>
        private TestingScreenView ScreenView { get; }

        /// <summary>
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="screenview"></param>
        /// <param name="screens"></param>
        public TestingBrowser(TestingScreenView screenview, Dictionary<string, Func<Screen>> screens)
            : base(new ScalableVector2(250, WindowManager.Height), new ScalableVector2(250, WindowManager.Height))
        {
            ScreenView = screenview;
            Screens = screens;
            Tint = Color.Black;
            Alpha = 0.85f;
            CreateButtons();
        }

        /// <summary>
        /// </summary>
        private void CreateButtons()
        {
            foreach (var test in Screens)
                AddButton(test.Key);
        }

        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        private void AddButton(string name)
        {
            var btn = new ImageButton(UserInterface.BlankBox, (sender, args) =>
            {
                ScreenView.CurrentTestingScreen?.Destroy();
                ScreenView.CurrentTestingScreen = Screens[name]();
            })
            {
                Size = new ScalableVector2(Width - 10, 30),
                Alignment = Alignment.TopCenter,
                Tint = new Color(60, 60, 60),
                Y = Buttons.Count * 30 + 10 * (Buttons.Count)
            };

            // ReSharper disable once ObjectCreationAsStatement
            new SpriteTextBitmap(FontsBitmap.GothamRegular, name)
            {
                Parent = btn,
                UsePreviousSpriteBatchOptions = true,
                FontSize = 16,
                Alignment = Alignment.MidCenter,
            };

            Buttons.Add(btn);

            var totalHeight =  Buttons.Count * 30 + 10 * (Buttons.Count - 1);

            if (totalHeight > Height)
                ContentContainer.Height = totalHeight;
            else
                ContentContainer.Height = Height;

            AddContainedDrawable(btn);
        }
    }
}