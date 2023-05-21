using System;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace MriBase.App.Base.Services.Implementations
{
    public class BitmapCreationService
    {
        private const int headerSize = 54;
        private readonly byte[] buffer;
        private readonly int width;
        private readonly int height;

        public BitmapCreationService(int width, int height)
        {
            this.width = width;
            this.height = height;

            int numPixels = this.width * this.height;
            int numPixelBytes = 4 * numPixels;
            int fileSize = headerSize + numPixelBytes;
            buffer = new byte[fileSize];

            using (MemoryStream memoryStream = new MemoryStream(buffer))
            {
                using (BinaryWriter writer = new BinaryWriter(memoryStream, Encoding.UTF8))
                {
                    writer.Write(new char[] { 'B', 'M' });
                    writer.Write(fileSize);
                    writer.Write((short)0);
                    writer.Write((short)0);
                    writer.Write(headerSize);

                    writer.Write(40);
                    writer.Write(this.width);
                    writer.Write(this.height);
                    writer.Write((short)1);
                    writer.Write((short)32);
                    writer.Write(0);
                    writer.Write(numPixelBytes);
                    writer.Write(0);
                    writer.Write(0);
                    writer.Write(0);
                    writer.Write(0);
                }
            }
        }

        public void SetPixel(int row, int col, Color color)
        {
            SetPixel(row, col, Convert.ToInt32(255 * color.R),
                               Convert.ToInt32(255 * color.G),
                               Convert.ToInt32(255 * color.B),
                               Convert.ToInt32(255 * color.A));
        }

        public ImageSource Generate()
        {
            return ImageSource.FromStream(() => new MemoryStream(buffer));
        }

        private void SetPixel(int row, int col, int r, int g, int b, int a = 255)
        {
            int index = (((row * width) + col) * 4) + headerSize;
            buffer[index + 0] = (byte)b;
            buffer[index + 1] = (byte)g;
            buffer[index + 2] = (byte)r;
            buffer[index + 3] = (byte)a;
        }
    }
}
