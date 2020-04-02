using System;
using System.Collections.Generic;
using System.Linq;
using Quaver.Shared.Screens;
using Quaver.Shared.Screens.Downloading;
using Quaver.Shared.Screens.Main;
using Quaver.Shared.Screens.Multi;
using Quaver.Shared.Screens.MultiplayerLobby;
using Quaver.Shared.Screens.Music;
using Quaver.Shared.Screens.Selection;
using Quaver.Shared.Screens.Theater;
using Wobble;
using Wobble.Window;

namespace Quaver.Shared.Window
{
    public static class QuaverWindowManager
    {
        /// <summary>
        ///     Returns the aspect ratio of the window
        /// </summary>
        public static AspectRatio Ratio
        {
            get
            {
                var graphics = GameBase.Game.Graphics;

                var ratio = (float) graphics.PreferredBackBufferWidth / graphics.PreferredBackBufferHeight;

                var widescreen = Math.Abs(ratio - 16 / 9f);
                var ultrawide = Math.Abs(ratio - 21 / 9f);
                var sixteenBy10 = Math.Abs(ratio - 16 / 10f);
                var standard = Math.Abs(ratio - 4 / 3f);

                var vals = new List<float>()
                {
                    widescreen,
                    ultrawide,
                    sixteenBy10,
                    standard
                };

                var closestRatio = vals.Min();

                // ReSharper disable twice CompareOfFloatsByEqualityOperator
                if (closestRatio == widescreen)
                    return AspectRatio.Widescreen;
                if (closestRatio == ultrawide)
                    return AspectRatio.Ultrawide;
                if (closestRatio == sixteenBy10)
                    return AspectRatio.SixteenByTen;
                if (closestRatio == standard)
                    return AspectRatio.Standard;

                return AspectRatio.Widescreen;
            }
        }

        /// <summary>
        ///     Returns if the user can change the resolution on the current screen
        /// </summary>
        public static bool CanChangeResolutionOnScene
        {
            get
            {
                var game = GameBase.Game as QuaverGame;

                if (game?.CurrentScreen == null)
                    return true;

                switch (game.CurrentScreen.Type)
                {
                    case QuaverScreenType.Menu:
                    case QuaverScreenType.Select:
                    case QuaverScreenType.Download:
                    case QuaverScreenType.Lobby:
                    case QuaverScreenType.Multiplayer:
                    case QuaverScreenType.Music:
                    case QuaverScreenType.Theatre:
                        return true;
                }

                return false;
            }
        }

        /// <summary>
        ///     Returns if the screen resolution is widescreen
        /// </summary>
        public static bool IsWidescreen => Ratio != AspectRatio.Standard && Ratio != AspectRatio.SixteenByTen;
    }
}