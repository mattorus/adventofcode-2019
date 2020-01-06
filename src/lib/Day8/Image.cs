using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode2019.Day8
{
    public class Image
    {
        public List<ImageLayer> ImageLayers { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Image()
        {
            ImageLayers = new List<ImageLayer>();
        }

        public Image(string path, int width, int height)
        {
            var encoded = File.ReadAllText(path);

            Width = width;
            Height = height;
            ImageLayers = new List<ImageLayer>();

            DecodeImage(encoded);
        }

        private void DecodeImage(string encoded)
        {
            int encodedPos = 0;

            while (encodedPos < encoded.Length)
            {
                List<int[]> image = new List<int[]>();

                for (int i = 0; i < Height; i++)
                {
                    List<int> row = new List<int>();

                    for (int j = 0; j < Width; j++)
                    {
                        var cur = "" + encoded[encodedPos++];

                        if (int.TryParse(cur, out int result))
                        {
                            row.Add(result);
                        }
                    }

                    image.Add(row.ToArray());
                }

                ImageLayers.Add(new ImageLayer(Width, Height, image.ToArray()));
            }
        }

    }
}
