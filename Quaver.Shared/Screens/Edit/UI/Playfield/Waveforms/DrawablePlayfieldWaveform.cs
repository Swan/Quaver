using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Quaver.Shared.Audio;
using Wobble.Audio.Tracks;
using Wobble.Bindables;
using Wobble.Graphics;
using Wobble.Graphics.Sprites;

namespace Quaver.Shared.Screens.Edit.UI.Playfield.Waveforms
{
    public class DrawablePlayfieldWaveform : Sprite
    {
        /// <summary>
        /// </summary>
        private EditorPlayfield Playfield { get; }

        /// <summary>
        /// </summary>
        private Waveform Waveform { get; }

        /// <summary>
        /// </summary>
        private List<Sprite> WaveformSprites { get; } = new List<Sprite>();

        /// <summary>
        /// </summary>
        /// <param name="playfield"></param>
        public DrawablePlayfieldWaveform(EditorPlayfield playfield)
        {
            Playfield = playfield;

            if (!(Playfield.Track is AudioTrack track))
                return;

            Waveform = new Waveform(track);

            Waveform.Textures.MultipleItemsAdded += OnWaveformGenerated;
            Waveform.GenerateTextures(1920, 120, Color.Crimson);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (var tex in WaveformSprites)
                tex.Draw(gameTime);

            base.Draw(gameTime);
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        public override void Destroy()
        {
            // ReSharper disable once DelegateSubtraction
            Waveform.Textures.MultipleItemsAdded -= OnWaveformGenerated;

            Waveform.Dispose();
            base.Destroy();
        }

        private void OnWaveformGenerated(object sender, BindableListMultipleItemsAddedEventArgs<Texture2D> e)
        {
            WaveformSprites.ForEach(x => x.Destroy());
            WaveformSprites.Clear();

            for (var i = 0; i < e.Items.Count; i++)
            {
                var tex = e.Items[i];
                var stream = File.OpenWrite($@"C:\users\swan\desktop\lol\{i}.png");
                tex.SaveAsPng(stream, tex.Width, tex.Height);
                stream.Dispose();

                var sprite = new Sprite()
                {
                    Size = new ScalableVector2(tex.Width, tex.Height),
                    Y = i == 0 ? 0 : WaveformSprites[i - 1].Y + WaveformSprites[i - 1].Height,
                    Image = tex,
                };

                // ReSharper disable once ObjectCreationAsStatement
                WaveformSprites.Add(sprite);
            }
        }
    }
}