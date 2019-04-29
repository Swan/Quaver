using Wobble.Screens;

namespace Quaver.Tests.Visual.Box
{
    public class TestBoxScreen : Screen
    {
        public sealed override ScreenView View { get; protected set; }

        public TestBoxScreen() => View = new TestBoxScreenView(this);
    }
}