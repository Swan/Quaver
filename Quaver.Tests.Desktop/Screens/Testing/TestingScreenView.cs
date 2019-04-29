using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Quaver.Shared.Screens;
using Quaver.Shared.Screens.Alpha;
using Quaver.Shared.Screens.Menu;
using Quaver.Tests.Desktop.Screens.Testing.UI;
using Quaver.Tests.Visual.Box;
using Wobble;
using Wobble.Graphics.Animations;
using Wobble.Input;
using Wobble.Logging;
using Wobble.Screens;
using Wobble.Window;

namespace Quaver.Tests.Desktop.Screens.Testing
{
    public class TestingScreenView : ScreenView
    {
        public TestingBrowser Browser { get; }

        public Screen CurrentTestingScreen { get; set; }

        private bool BrowserActive { get; set; } = true;

        public TestingScreenView(Screen screen) : base(screen)
        {
            Logger.Important($"Test Screen Created", LogType.Runtime);

            Browser = new TestingBrowser(this, new Dictionary<string, Func<Screen>>
            {
                {"Alpha Testing", () => new AlphaScreen()},
                {"Box", () =>  new TestBoxScreen()},
                {"Main Menu", () => new  MenuScreen()},
            })
            {
                Parent = Container
            };
        }

        public override void Update(GameTime gameTime)
        {
            Container?.Update(gameTime);
            CurrentTestingScreen?.Update(gameTime);

            if (KeyboardManager.IsUniqueKeyPress(Keys.OemTilde))
            {
                BrowserActive = !BrowserActive;
                Browser?.ClearAnimations();
                Browser?.MoveToX(BrowserActive ? 0 : -Browser.Width - 10, Easing.OutQuint, 400);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            GameBase.Game.GraphicsDevice.Clear(Color.CornflowerBlue);
            CurrentTestingScreen?.Draw(gameTime);
            Container?.Draw(gameTime);
            GameBase.Game.SpriteBatch.End();
        }

        public override void Destroy() => Container?.Destroy();
    }
}