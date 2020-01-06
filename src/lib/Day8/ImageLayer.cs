using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2019.Day8
{
    public class ImageLayer
    {
        public Dictionary<int, int> DigitCounts { get; set; }
        public int[][] Pixels { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public ImageLayer(int width, int height, int[][] image)
        {
            this.Width = width;
            this.Height = height;
            Pixels = image;

            DigitCounts = new Dictionary<int, int>();

            InitializeImage();
        }

        public void Print()
        {
            foreach (int[] row in Pixels)
            {
                foreach (int i in row)
                {
                    Console.Write(i == 1 ? "0" : "  ");
                }
                Console.WriteLine();
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (int[] row in Pixels)
            {
                sb.Append(String.Join("", row));
            }

            return sb.ToString();
        }

        private void InitializeImage()
        {
            for (int i = 0; i < Pixels.Length; i++)
            {
                for (int j = 0; j < Pixels[i].Length; j++)
                {
                    if (!DigitCounts.ContainsKey(Pixels[i][j]))
                    {
                        DigitCounts[Pixels[i][j]] = 0;
                    }

                    DigitCounts[Pixels[i][j]]++;
                }
            }
        }
    }
}
