using Quaver.Server.Common.Objects;

namespace Quaver.Shared.Screens.Workshop
{
    public sealed class SteamWorkshopScreen : QuaverScreen
    {
        /// <inheritdoc />
        /// <summary>
        /// </summary>
        public override QuaverScreenType Type { get; } = QuaverScreenType.Workshop;

        /// <summary>
        /// </summary>
        public SteamWorkshopScreen() => View = new SteamWorkshopScreenView(this);

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <returns></returns>
        public override UserClientStatus GetClientStatus() => null;
    }
}