using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using ManagedBass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Quaver.Shared.Assets;
using Quaver.Shared.Graphics;
using Quaver.Shared.Graphics.Menu.Border;
using Quaver.Shared.Helpers;
using Wobble;
using Wobble.Assets;
using Wobble.Audio.Tracks;
using Wobble.Bindables;
using Wobble.Graphics;
using Wobble.Graphics.Sprites;
using Wobble.Logging;
using Wobble.Window;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Quaver.Shared.Audio
{
    public class Waveform : IDisposable
    {
        /// <summary>
        /// </summary>
        private AudioTrack Track { get; }

        /// <summary>
        /// </summary>
        public BindableList<Texture2D> Textures { get; } = new BindableList<Texture2D>(new List<Texture2D>());

        /// <summary>
        /// </summary>
        /// <param name="track"></param>
        public Waveform(AudioTrack track) => Track = track;

        /// <summary>
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="col"></param>
        public void GenerateTextures(int width, int height, Color col)
        {
            // Get rid of all previous textures if they exist
            if (Textures.Value != null)
            {
                Textures.Value.ForEach(x => x.Dispose());
                Textures.Clear();
            }

            var stream = 0;
            const BassFlags flags = BassFlags.Decode | BassFlags.Float;

            try
            {
                switch (Track.Type)
                {
                    case AudioTrackLoadType.FilePath:
                        stream = Bass.CreateStream(Track.OriginalFilePath, 0, 0, flags);
                        break;
                    case AudioTrackLoadType.ByteArray:
                        stream = Bass.CreateStream(Track.OriginalByteArray, 0, Track.OriginalByteArray.Length, flags);
                        break;
                    case AudioTrackLoadType.Uri:
                        stream = Bass.CreateStream(Track.OriginalUri.ToString(), 0, 0, flags);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                var length = Bass.ChannelGetLength(stream);

                const int readBytes = 2048;

                var fft = new float[readBytes];
                var waveform = new List<float>();

                while (length > 0)
                {
                    var bytesRead = Bass.ChannelGetData(stream, fft, (int) DataFlags.FFT2048);
                    length -= bytesRead;

                    var max = fft.Max();

                    for (var i = 0f; i < readBytes; i += 2048 / 128f)
                        waveform.Add(max);
                }

                GenerateWaveform(waveform, width, height, col);
            }
            catch (Exception e)
            {
                Logger.Error(e, LogType.Runtime);
            }

            Bass.StreamFree(stream);
        }

        /// <summary>
        ///     Generates the list of rendertargets to use as the waveform
        /// </summary>
        /// <param name="samples"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="color"></param>
        private void GenerateWaveform(IReadOnlyList<float> samples, int width, int height, Color color)
        {
            var waveform = new Dictionary<int, float>();
            var packSize = samples.Count / width + 1;

            var s = 0;

            for (var i = 0; i < samples.Count; i += packSize)
            {
                waveform[s] = Math.Abs(samples[i]);
                s++;
            }

            GameBase.Game.ScheduledRenderTargetDraws.Add(() =>
            {
                // Create a RenderTarget with mipmapping with the original texuture.
                var (pixelWidth, pixelHeight) = new Vector2(width, height) * WindowManager.ScreenScale;

                var rt = new RenderTarget2D(GameBase.Game.GraphicsDevice, (int) pixelWidth, (int) pixelHeight, false,
                    GameBase.Game.GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.None);

                GameBase.Game.GraphicsDevice.SetRenderTarget(rt);
                GameBase.Game.GraphicsDevice.Clear(Color.Transparent);

                var container = new Container { Size = new ScalableVector2(width, height) };

                for (var j = 0; j < waveform.Count; j++)
                {
                    var lineHeight = 1f;

                    if (waveform.ContainsKey(j))
                    {
                        var sample = waveform[j];
                        lineHeight = sample * (height * 0.75f);
                    }

                    lineHeight = MathHelper.Clamp(lineHeight * 2, 1, height * 0.75f);

                    var line = new Sprite
                    {
                        Parent = container,
                        Alignment = Alignment.MidLeft,
                        Image = UserInterface.BlankBox,
                        Size = new ScalableVector2(1, lineHeight),
                        Position = new ScalableVector2(j, 0),
                        Tint = new Color(color.R / 2, color.G / 2, color.B / 2)
                    };

                    // Line drawn in the middle to differentiate colors/aesthetics.
                    if (line.Height >= 4)
                    {
                        // ReSharper disable once ObjectCreationAsStatement
                        new Sprite
                        {
                            Parent = line,
                            Alignment = Alignment.MidCenter,
                            Image = UserInterface.BlankBox,
                            Size = new ScalableVector2(1, lineHeight / 2.5f),
                            Tint = color
                        };
                    }

                    container.Draw(new GameTime());
                }

                GameBase.Game.SpriteBatch.End();
                GameBase.Game.GraphicsDevice.SetRenderTarget(null);
                Textures.AddRange(new List<Texture2D> { rt } );
            });
        }

        public void Dispose() => Textures?.Dispose();
    }
}