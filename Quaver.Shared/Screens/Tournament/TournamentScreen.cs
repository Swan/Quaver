using Quaver.Server.Common.Objects;
using Wobble.Screens;

namespace Quaver.Shared.Screens.Tournament
{
    public sealed class TournamentScreen : QuaverScreen
    {
        public override QuaverScreenType Type { get; } = QuaverScreenType.Tournament;

        public TournamentScreen() => View = new TournamentScreenView(this); 
        public override UserClientStatus GetClientStatus() => null;
    }
}