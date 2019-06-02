using System.Collections.Generic;
using Quaver.API.Enums;
using Quaver.API.Replays;
using Quaver.Server.Client.Structures;
using Quaver.Server.Common.Objects;
using Quaver.Shared.Database.Maps;

namespace Quaver.Shared.Online
{
    public class SpectatorClient
    {
        /// <summary>
        ///     The player that is currently being spectated
        /// </summary>
        public User Player { get; }

        /// <summary>
        ///     The user's current replay that is being received.
        /// </summary>
        public Replay Replay { get; private set; }

        /// <summary>
        ///     The map that the user is currently playing
        /// </summary>
        public Map Map { get; private set; }

        /// <summary>
        /// </summary>
        /// <param name="player"></param>
        public SpectatorClient(User player) => Player = player;

        /// <summary>
        ///     Handles when the client is beginning to play a new map
        /// </summary>
        /// <param name="map"></param>
        /// <param name="mods"></param>
        public void PlayNewMap(Map map, ModIdentifier mods)
        {
            Map = map;
            Replay = new Replay(Map.Mode, Player.OnlineUser.Username, mods, Map.Md5Checksum);
        }

        /// <summary>
        ///     Adds a single replay frame to the spectating replay
        /// </summary>
        /// <param name="f"></param>
        public void AddFrame(ReplayFrame f) => Replay.Frames.Add(f);

        /// <summary>
        ///     Adds a bundle of replay frames to the spectating replay
        /// </summary>
        /// <param name="frames"></param>
        public void AddFrames(List<ReplayFrame> frames) => frames.ForEach(AddFrame);
    }
}