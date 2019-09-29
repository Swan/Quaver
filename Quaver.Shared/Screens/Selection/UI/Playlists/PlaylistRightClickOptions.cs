using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Quaver.Shared.Database.Maps;
using Quaver.Shared.Database.Playlists;
using Quaver.Shared.Graphics.Form.Dropdowns.RightClick;
using Quaver.Shared.Graphics.Notifications;
using Quaver.Shared.Screens.Selection.UI.Mapsets;
using Wobble;
using Wobble.Graphics;
using Wobble.Graphics.UI.Dialogs;
using ColorHelper = Quaver.Shared.Helpers.ColorHelper;

namespace Quaver.Shared.Screens.Selection.UI.Playlists
{
    public class PlaylistRightClickOptions : RightClickOptions
    {
        /// <summary>
        /// </summary>
        private DrawablePlaylist Drawable { get; }

        /// <summary>
        /// </summary>
        private Playlist Playlist { get; }

        private const string Play = "Play";

        private const string Delete = "Delete";

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="playlist"></param>
        public PlaylistRightClickOptions(DrawablePlaylist playlist) : base(GetOptions(), new ScalableVector2(200, 40), 22)
        {
            Drawable = playlist;
            Playlist = playlist.Item;

            ItemSelected += (sender, args) =>
            {
                var game = (QuaverGame) GameBase.Game;
                var selectScreen = game.CurrentScreen as SelectionScreen;

                switch (args.Text)
                {
                    case Play:
                        SelectPlaylist();

                        if (selectScreen != null)
                            selectScreen.ActiveScrollContainer.Value = SelectScrollContainerType.Mapsets;
                        break;
                    case Delete:
                        if (Playlist.PlaylistGame != MapGame.Quaver)
                        {
                            NotificationManager.Show(NotificationLevel.Error, "You cannot delete a playlist loaded from another game.");
                            return;
                        }

                        DialogManager.Show(new DeletePlaylistDialog(Playlist));
                        break;
                }
            };
        }

        /// <summary>
        /// </summary>
        private void SelectPlaylist()
        {
            PlaylistManager.Selected.Value = Playlist;

            var container = (PlaylistContainer) Drawable.Container;

            var index = container.AvailableItems.IndexOf(Playlist);

            if (index == -1)
                return;

            container.SelectedIndex.Value = index;
            container.ScrollToSelected();
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, Color> GetOptions() => new Dictionary<string, Color>()
        {
            {Play, Color.White},
            {Delete, ColorHelper.HexToColor($"#FF6868")}
        };
    }
}