using System;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Quaver.Shared;
using Quaver.Shared.Assets;
using Quaver.Shared.Graphics.Backgrounds;
using Quaver.Shared.Graphics.Overlays.Volume;
using Quaver.Shared.Graphics.Transitions;
using Quaver.Shared.Online;
using Quaver.Shared.Screens;
using Quaver.Shared.Skinning;
using Quaver.Tests.Desktop.Screens;
using Quaver.Tests.Desktop.Screens.Testing;
using Steamworks;
using Wobble;
using Wobble.IO;
using Wobble.Window;

namespace Quaver.Tests.Desktop
{
    public class QuaverTestsGame : QuaverGame
    {
        protected override bool IsReadyToUpdate { get; set; }

        public QuaverTestsGame() => IsMouseVisible = true;

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            if (CurrentScreen != null && CurrentScreen.Type == QuaverScreenType.Alpha && !CurrentScreen.Exiting)
                CurrentScreen.Exit(() => new TestingScreen());

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}