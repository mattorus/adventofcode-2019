using System.Collections.Generic;

namespace AdventOfCode2019.Day8
{
    public class ImageReceiver
    {
        public Image Image { get; set; }
        public ImageLayer DecodedImage { get; set; }

        public ImageReceiver(string path, int width, int height)
        {
            Image = new Image(path, width, height);
        }

        public ImageLayer FindMinDigitLayer(int minDigit)
        {
            ImageLayer minDigitLayer = null;
            int minDigitCount = int.MaxValue;

            foreach (ImageLayer layer in Image.ImageLayers)
            {
                if (layer.DigitCounts.ContainsKey(minDigit) && minDigitCount > layer.DigitCounts[minDigit])
                {
                    minDigitCount = layer.DigitCounts[minDigit];
                    minDigitLayer = layer;
                }
            }

            return minDigitLayer;
        }

        public void DecodeImage()
        {
            List<int[]> image = new List<int[]>();

            for (int i = 0; i < Image.Height; i++)            
            {
                List<int> row = new List<int>();

                for (int j = 0; j < Image.Width; j++)
                {
                    foreach(ImageLayer layer in Image.ImageLayers)
                    {
                        if (layer.Pixels[i][j] == 2)
                        {
                            continue;
                        }

                        row.Add(layer.Pixels[i][j]);

                        break;
                    }
                }

                image.Add(row.ToArray());
            }

            DecodedImage = new ImageLayer(Image.Width, Image.Height, image.ToArray());
        }
    }
}
