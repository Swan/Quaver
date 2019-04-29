using Microsoft.Xna.Framework;
using Wobble;
using Wobble.Graphics;
using Wobble.Graphics.Animations;
using Wobble.Graphics.Sprites;
using Wobble.Logging;
using Wobble.Screens;

namespace Quaver.Tests.Visual.Box
{
    public class TestBoxScreenView : ScreenView
    {
        private Sprite Box { get; }

        public TestBoxScreenView(Screen screen) : base(screen)
        {
            Logger.Important($"Loaded Test box screen", LogType.Runtime);

            Box = new Sprite
            {
                Parent = Container,
                Size = new ScalableVector2(100, 100),
                Tint = Color.LimeGreen,
                Alignment = Alignment.MidCenter
            };
        }

        public override void Update(GameTime gameTime)
        {
            Container?.Update(gameTime);

            if (Box.Animations.Count == 0)
            {
                var rotation = MathHelper.ToDegrees(Box.Rotation);
                Box.Animations.Add(new Animation(AnimationProperty.Rotation, Easing.Linear, rotation, rotation + 360, 1000));
            }
        }

        public override void Draw(GameTime gameTime) => Container?.Draw(gameTime);

        public override void Destroy() => Container?.Destroy();
    }
}