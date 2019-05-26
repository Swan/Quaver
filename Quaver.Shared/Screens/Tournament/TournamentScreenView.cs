using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Quaver.API.Enums;
using Quaver.Shared.Database.Maps;
using Quaver.Shared.Database.Scores;
using Quaver.Shared.Modifiers;
using Quaver.Shared.Screens.Gameplay;
using Wobble;
using Wobble.Bindables;
using Wobble.Graphics;
using Wobble.Graphics.Animations;
using Wobble.Graphics.Sprites;
using Wobble.Input;
using Wobble.Screens;

namespace Quaver.Shared.Screens.Tournament
{
    public class TournamentScreenView : ScreenView
    {
        private List<GameplayScreen> screens = new List<GameplayScreen>();

        private List<RenderTarget2D> RenderTarget2Ds { get; }

        private List<Sprite> ScreenSprites { get; } = new List<Sprite>();

        public TournamentScreenView(Screen screen) : base(screen)
        {
            ModManager.AddMod(ModIdentifier.Autoplay);

            var map = MapManager.Selected.Value.LoadQua();
            MapManager.Selected.Value.Qua = map;
            MapManager.Selected.Value.Scores.Value = new List<Score>();

            screens = new List<GameplayScreen>()
            {
                new GameplayScreen(map, MapManager.Selected.Value.Md5Checksum, new List<Score>()),
                new GameplayScreen(map, MapManager.Selected.Value.Md5Checksum, new List<Score>()),
                new GameplayScreen(map, MapManager.Selected.Value.Md5Checksum, new List<Score>()),
                new GameplayScreen(map, MapManager.Selected.Value.Md5Checksum, new List<Score>()),
                new GameplayScreen(map, MapManager.Selected.Value.Md5Checksum, new List<Score>()),
                new GameplayScreen(map, MapManager.Selected.Value.Md5Checksum, new List<Score>()),
                new GameplayScreen(map, MapManager.Selected.Value.Md5Checksum, new List<Score>()),
                new GameplayScreen(map, MapManager.Selected.Value.Md5Checksum, new List<Score>()),
            };

            RenderTarget2Ds = new List<RenderTarget2D>()
            {
                new RenderTarget2D(GameBase.Game.GraphicsDevice, 1366, 768, false,
                    GameBase.Game.GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.None),
                new RenderTarget2D(GameBase.Game.GraphicsDevice, 1366, 768, false,
                    GameBase.Game.GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.None),
                new RenderTarget2D(GameBase.Game.GraphicsDevice, 1366, 768, false,
                    GameBase.Game.GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.None),
                new RenderTarget2D(GameBase.Game.GraphicsDevice, 1366, 768, false,
                    GameBase.Game.GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.None),
                new RenderTarget2D(GameBase.Game.GraphicsDevice, 1366, 768, false,
                    GameBase.Game.GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.None),
                new RenderTarget2D(GameBase.Game.GraphicsDevice, 1366, 768, false,
                    GameBase.Game.GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.None),
                new RenderTarget2D(GameBase.Game.GraphicsDevice, 1366, 768, false,
                    GameBase.Game.GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.None),
                new RenderTarget2D(GameBase.Game.GraphicsDevice, 1366, 768, false,
                    GameBase.Game.GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.None),
                new RenderTarget2D(GameBase.Game.GraphicsDevice, 1366, 768, false,
                    GameBase.Game.GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.None)
            };

            ScreenSprites.Add(new Sprite()
            {
                Parent = Container,
                Image = RenderTarget2Ds[0],
                Size = new ScalableVector2(624, 351),
                Alignment = Alignment.TopLeft
            });

            ScreenSprites.Add(new Sprite()
            {
                Parent = Container,
                Image = RenderTarget2Ds[1],
                Size = new ScalableVector2(624, 351),
                Alignment = Alignment.TopRight
            });

            ScreenSprites.Add(new Sprite()
            {
                Parent = Container,
                Image = RenderTarget2Ds[2],
                Size = new ScalableVector2(624, 351),
                Alignment = Alignment.BotLeft
            });

            ScreenSprites.Add(new Sprite()
            {
                Parent = Container,
                Image = RenderTarget2Ds[3],
                Size = new ScalableVector2(624, 351),
                Alignment = Alignment.BotRight
            });

            ScreenSprites.Add(new Sprite()
            {
                Parent = Container,
                Image = RenderTarget2Ds[4],
                Size = new ScalableVector2(624, 351),
                Alignment = Alignment.BotRight
            });

            ScreenSprites.Add(new Sprite()
            {
                Parent = Container,
                Image = RenderTarget2Ds[5],
                Size = new ScalableVector2(624, 351),
                Alignment = Alignment.BotRight
            });

            ScreenSprites.Add(new Sprite()
            {
                Parent = Container,
                Image = RenderTarget2Ds[6],
                Size = new ScalableVector2(624, 351),
                Alignment = Alignment.BotRight
            });

            ScreenSprites.Add(new Sprite()
            {
                Parent = Container,
                Image = RenderTarget2Ds[7],
                Size = new ScalableVector2(624, 351),
                Alignment = Alignment.BotRight
            });
        }

        public override void Update(GameTime gameTime)
        {
            screens.ForEach(x => x.Update(gameTime));

            if (KeyboardManager.IsUniqueKeyPress(Keys.Z))
            {
                for (var i = 0; i < screens.Count; i++)
                {
                    ScreenSprites[i].ClearAnimations();

                    if (i == 0)
                    {
                        ScreenSprites[i].Size = new ScalableVector2(1366, 768);
                        ScreenSprites[i].Visible = true;
                    }
                    else
                    {
                        ScreenSprites[i].Size = new ScalableVector2(624, 351);
                        ScreenSprites[i].Visible = false;
                    }
                }
            }

            if (KeyboardManager.IsUniqueKeyPress(Keys.X))
            {
                for (var i = 0; i < screens.Count; i++)
                {
                    ScreenSprites[i].ClearAnimations();

                    if (i == 1)
                    {
                        ScreenSprites[i].Size = new ScalableVector2(1366, 768);
                        ScreenSprites[i].Visible = true;
                    }
                    else
                    {
                        ScreenSprites[i].Size = new ScalableVector2(624, 351);
                        ScreenSprites[i].Visible = false;
                    }
                }
            }

            if (KeyboardManager.IsUniqueKeyPress(Keys.C))
            {
                for (var i = 0; i < screens.Count; i++)
                {
                    ScreenSprites[i].ClearAnimations();

                    if (i == 2)
                    {
                        ScreenSprites[i].Size = new ScalableVector2(1366, 768);
                        ScreenSprites[i].Visible = true;
                    }
                    else
                    {
                        ScreenSprites[i].Size = new ScalableVector2(624, 351);
                        ScreenSprites[i].Visible = false;
                    }
                }
            }

            if (KeyboardManager.IsUniqueKeyPress(Keys.V))
            {
                for (var i = 0; i < screens.Count; i++)
                {
                    ScreenSprites[i].ClearAnimations();

                    if (i == 3)
                    {
                        ScreenSprites[i].Size = new ScalableVector2(1366, 768);
                        ScreenSprites[i].Visible = true;
                    }
                    else
                    {
                        ScreenSprites[i].Size = new ScalableVector2(624, 351);
                        ScreenSprites[i].Visible = false;
                    }
                }
            }

            if (KeyboardManager.IsUniqueKeyPress(Keys.B))
                ScreenSprites.ForEach(x =>
                {
                    x.Size = new ScalableVector2(624, 351);
                    x.Visible = true;
                });
        }

        public override void Draw(GameTime gameTime)
        {
            var game = GameBase.Game as QuaverGame;

            game?.ScheduledRenderTargetDraws.Add(() =>
            {
                for (var i = 0; i < screens.Count; i++)
                {
                    GameBase.Game.GraphicsDevice.SetRenderTarget(RenderTarget2Ds[i]);
                    GameBase.Game.GraphicsDevice.Clear(Color.Transparent);
                    screens[i].Draw(gameTime);
                    GameBase.Game.SpriteBatch.End();
                    GameBase.Game.GraphicsDevice.SetRenderTarget(null);
                }

            });

            GameBase.Game.GraphicsDevice.Clear(Color.Black);
            Container?.Draw(gameTime);
        }

        public override void Destroy()
        {
        }
    }
}