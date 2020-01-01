namespace AdventOfCode2019.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using AdventOfCode2019;
    using AdventOfCode2019.Day8;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using static AdventOfCode2019.IntcodeRunner;

    [TestClass]
    public class Day8Tests
    {
        private const string InputFile1 = @".\input\Day8.txt";

        [TestMethod]
        public void Part1Tests_File()
        {
            var image = new ImageReceiver(InputFile1, 25, 6);
            var layer = image.FindMinDigitLayer(0);
            var result = layer.DigitCounts[1] * layer.DigitCounts[2];

            Console.WriteLine(result);
            result.Should().Be(2460);
        }

        [TestMethod]
        public void Part2Tests_File()
        {
            var image = new ImageReceiver(InputFile1, 25, 6);

            image.DecodeImage();
            image.DecodedImage.Print();
            image.DecodedImage.ToString().Should().Be("100001110011110100101001010000100101000010100100101000010010111001100010010100001110010000101001001010000101001000010100100101111010010100001001001100");
        }
    }
}
