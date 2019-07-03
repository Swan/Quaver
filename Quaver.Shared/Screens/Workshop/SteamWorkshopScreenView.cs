using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Quaver.Shared.Assets;
using Quaver.Shared.Graphics;
using Quaver.Shared.Graphics.Menu;
using Quaver.Shared.Screens.Menu;
using Wobble;
using Wobble.Graphics;
using Wobble.Graphics.UI;
using Wobble.Screens;

namespace Quaver.Shared.Screens.Workshop
{
    public class SteamWorkshopScreenView : ScreenView
    {
        /// <summary>
        /// </summary>
        private MenuHeader Header { get; set; }

        /// <summary>
        /// </summary>
        private MenuFooter Footer { get; set; }

        /// <summary>
        /// </summary>
        private BackgroundImage Background { get; }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="screen"></param>
        public SteamWorkshopScreenView(Screen screen) : base(screen)
        {
            Background = new BackgroundImage(UserInterface.MenuBackgroundRaw, 30) {Parent = Container};
            CreateHeader();
            CreateFooter();
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            Container?.Update(gameTime);
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            GameBase.Game.GraphicsDevice.Clear(Color.Black);
            Container?.Draw(gameTime);
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        public override void Destroy() => Container?.Destroy();

        /// <summary>
        /// </summary>
        private void CreateHeader()
        {
            Header = new MenuHeader(FontAwesome.Get(FontAwesomeIcon.fa_pencil), "Steam", "Workshop",
                "Find or share skins on the Steam workshop", Colors.MainAccent)
            {
                Parent = Container,
                Alignment = Alignment.TopLeft
            };
        }

        /// <summary>
        /// </summary>
        private void CreateFooter()
        {
            Footer = new MenuFooter(new List<ButtonText>()
            {
                new ButtonText(FontsBitmap.GothamRegular, "Back", 14, (sender, args) =>
                {
                    var screen = (QuaverScreen) Screen;
                    screen.Exit(() => new MenuScreen());
                })
            }, new List<ButtonText>(), Colors.MainAccent)
            {
                Parent = Container,
                Alignment = Alignment.BotLeft
            };
        }
    }
}