namespace AdventOfCode2019.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using AdventOfCode2019;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using static AdventOfCode2019.IntcodeRunner;

    [TestClass]
    public class AmplifierTests
    {
        private const string InputFile1 = @".\input\Day7.txt";

        [DataTestMethod]
        [DataRow(0, 0, new int[] { 3, 15, 3, 16, 1002, 16, 10, 16, 1, 16, 15, 15, 4, 15, 99, 0, 0 }, 0)]
        [DataRow(1, 0, new int[] { 3, 15, 3, 16, 1002, 16, 10, 16, 1, 16, 15, 15, 4, 15, 99, 0, 0 }, 1)]
        [DataRow(2, 0, new int[] { 3, 15, 3, 16, 1002, 16, 10, 16, 1, 16, 15, 15, 4, 15, 99, 0, 0 }, 2)]
        [DataRow(3, 0, new int[] { 3, 15, 3, 16, 1002, 16, 10, 16, 1, 16, 15, 15, 4, 15, 99, 0, 0 }, 3)]
        [DataRow(4, 0, new int[] { 3, 15, 3, 16, 1002, 16, 10, 16, 1, 16, 15, 15, 4, 15, 99, 0, 0 }, 4)]
        [DataRow(0, 4, new int[] { 3, 15, 3, 16, 1002, 16, 10, 16, 1, 16, 15, 15, 4, 15, 99, 0, 0 }, 40)]
        [DataRow(1, 4, new int[] { 3, 15, 3, 16, 1002, 16, 10, 16, 1, 16, 15, 15, 4, 15, 99, 0, 0 }, 41)]
        [DataRow(2, 4, new int[] { 3, 15, 3, 16, 1002, 16, 10, 16, 1, 16, 15, 15, 4, 15, 99, 0, 0 }, 42)]
        [DataRow(3, 4, new int[] { 3, 15, 3, 16, 1002, 16, 10, 16, 1, 16, 15, 15, 4, 15, 99, 0, 0 }, 43)]
        [DataRow(0, 43, new int[] { 3, 15, 3, 16, 1002, 16, 10, 16, 1, 16, 15, 15, 4, 15, 99, 0, 0 }, 430)]
        public void TestExecute(int phaseSetting, int ampInput, int[] instructions, int expected)
        {
            var amplifier = new Amplifier(phaseSetting, ampInput, instructions);

            var ampResults = amplifier.Execute();
            ampResults.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(new int[] { 4, 3, 2, 1, 0 }, new int[] { 3, 15, 3, 16, 1002, 16, 10, 16, 1, 16, 15, 15, 4, 15, 99, 0, 0 }, 43210)]
        [DataRow(new int[] { 0, 1, 2, 3, 4 }, new int[] { 3, 23, 3, 24, 1002, 24, 10, 24, 1002, 23, -1, 23, 101, 5, 23, 23, 1, 24, 23, 23, 4, 23, 99, 0, 0 }, 54321)]
        [DataRow(new int[] { 1, 0, 4, 3, 2 }, new int[] { 3, 31, 3, 32, 1002, 32, 10, 32, 1001, 31, -2, 31, 1007, 31, 0, 33, 1002, 33, 7, 33, 1, 33, 31, 31, 1, 32, 31, 31, 4, 31, 99, 0, 0, 0 }, 65210)]
        public void TestAmpChain(int[] phaseSequence, int[] instructions, int expected)
        {
            var amp = new Amplifier(0, 0, instructions);
            var ampResult = amp.ExecuteChainSequence(phaseSequence);

            ampResult.Should().Be(expected);
        }

        //[DataTestMethod]
        //[DataRow(new int[] { 9, 8, 7, 6, 5 }, new int[] { 3, 26, 1001, 26, -4, 26, 3, 27, 1002, 27, 2, 27, 1, 27, 26, 27, 4, 27, 1001, 28, -1, 28, 1005, 28, 6, 99, 0, 0, 5 }, 139629729)]
        //public void TestFeedbackAmpChain(int[] phaseSequence, int[] instructions, int expected)
        //{
        //    var amp = new Amplifier(0, 0, instructions, Mode.feedback);
            
        //    var ampResult = amp.ExecuteChainSequence(phaseSequence);

        //    ampResult.Should().Be(expected);
        //}

        [TestMethod]
        public void Day7_Part1Tests_File()
        {
            var instructions = File.ReadAllText(InputFile1)
                .Split(",")
                .Select(x => int.Parse(x))
                .ToArray();

            var amp = new Amplifier(0, 0, instructions);
            var maxOutput = amp.GetMaxAmpChainOutput(5, Mode.normal);

            Console.WriteLine($"Max Output: {maxOutput}");
        }

        [TestMethod]
        public void Day7_Part2Tests_File()
        {
            var instructions = File.ReadAllText(InputFile1)
                .Split(",")
                .Select(x => int.Parse(x))
                .ToArray();

            var amp = new Amplifier(0, 0, instructions);
            var maxOutput = amp.GetMaxAmpChainOutput(5, Mode.normal);

            Console.WriteLine($"Max Output: {maxOutput}");
        }
    }
}
