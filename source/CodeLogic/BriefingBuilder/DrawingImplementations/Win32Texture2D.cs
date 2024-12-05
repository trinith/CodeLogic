using ArbitraryPixel.Common;
using ArbitraryPixel.Common.Graphics;
using System;
using System.Drawing;
using System.IO;

namespace BriefingBuilder.DrawingImplementations
{
    using MGDrawing = Microsoft.Xna.Framework;

    public class Win32Texture2D : WrapperBase<Bitmap>, ITexture2D
    {
        #region Static Methods
        public static Win32Texture2D FromFile(string contentPath, string assetName)
        {
            string file = Path.Combine(contentPath, assetName);
            Bitmap internalImage = (Bitmap)Image.FromFile(file);

            return new Win32Texture2D(internalImage) { Texture2DAsset = Path.GetFileNameWithoutExtension(assetName) };
        }
        #endregion

        public string Texture2DAsset { get; set; }

        public Win32Texture2D(Bitmap internalImage)
            : base(internalImage)
        {
        }

        #region ITexture2D Implementation
        public int Width => this.WrappedObject.Width;
        public int Height => this.WrappedObject.Height;

        public void SetData<T>(T[] data) where T : struct
        {
            if (typeof(T) != typeof(MGDrawing.Color))
                throw new NotImplementedException("Types other than Color are not currently supported.");

            MGDrawing.Color[] mgData = (MGDrawing.Color[])Convert.ChangeType(data, typeof(MGDrawing.Color[]));

            for (int x = 0; x < this.WrappedObject.Width; x++)
            {
                for (int y = 0; y < this.WrappedObject.Height; y++)
                {
                    int index = this.WrappedObject.Width * y + x;
                    if (index >= data.Length)
                        return;

                    Color c = Color.FromArgb(mgData[index].A, mgData[index].R, mgData[index].G, mgData[index].B);

                    this.WrappedObject.SetPixel(x, y, c);
                }
            }
        }

        public void SetData<T>(T data) where T : struct
        {
            if (typeof(T) != typeof(MGDrawing.Color))
                throw new NotImplementedException("Types other than Color are not currently supported.");

            MGDrawing.Color mgColour = (MGDrawing.Color)Convert.ChangeType(data, typeof(MGDrawing.Color));
            Color c = Color.FromArgb(mgColour.A, mgColour.R, mgColour.G, mgColour.B);

            for (int x = 0; x < this.WrappedObject.Width; x++)
            {
                for (int y = 0; y < this.WrappedObject.Height; y++)
                {
                    this.WrappedObject.SetPixel(x, y, c);
                }
            }
        }
        #endregion
    }
}
