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
    public class Day7Tests
    {
        private const string InputFile1 = @".\input\Day7.txt";

        [DataTestMethod]
        [DataRow(0, 0, new long[] { 3, 15, 3, 16, 1002, 16, 10, 16, 1, 16, 15, 15, 4, 15, 99, 0, 0 }, 0)]
        [DataRow(1, 0, new long[] { 3, 15, 3, 16, 1002, 16, 10, 16, 1, 16, 15, 15, 4, 15, 99, 0, 0 }, 1)]
        [DataRow(2, 0, new long[] { 3, 15, 3, 16, 1002, 16, 10, 16, 1, 16, 15, 15, 4, 15, 99, 0, 0 }, 2)]
        [DataRow(3, 0, new long[] { 3, 15, 3, 16, 1002, 16, 10, 16, 1, 16, 15, 15, 4, 15, 99, 0, 0 }, 3)]
        [DataRow(4, 0, new long[] { 3, 15, 3, 16, 1002, 16, 10, 16, 1, 16, 15, 15, 4, 15, 99, 0, 0 }, 4)]
        [DataRow(0, 4, new long[] { 3, 15, 3, 16, 1002, 16, 10, 16, 1, 16, 15, 15, 4, 15, 99, 0, 0 }, 40)]
        [DataRow(1, 4, new long[] { 3, 15, 3, 16, 1002, 16, 10, 16, 1, 16, 15, 15, 4, 15, 99, 0, 0 }, 41)]
        [DataRow(2, 4, new long[] { 3, 15, 3, 16, 1002, 16, 10, 16, 1, 16, 15, 15, 4, 15, 99, 0, 0 }, 42)]
        [DataRow(3, 4, new long[] { 3, 15, 3, 16, 1002, 16, 10, 16, 1, 16, 15, 15, 4, 15, 99, 0, 0 }, 43)]
        [DataRow(0, 43, new long[] { 3, 15, 3, 16, 1002, 16, 10, 16, 1, 16, 15, 15, 4, 15, 99, 0, 0 }, 430)]
        public void TestExecute(int phaseSetting, int ampInput, long[] instructions, int expected)
        {
            var amplifier = new Amplifier(phaseSetting, instructions);
            var input = new long[] { phaseSetting, ampInput };

            amplifier.UpdateInput(input);

            var ampResults = amplifier.Execute();

            ampResults.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(new int[] { 4, 3, 2, 1, 0 }, new long[] { 3, 15, 3, 16, 1002, 16, 10, 16, 1, 16, 15, 15, 4, 15, 99, 0, 0 }, 43210)]
        [DataRow(new int[] { 0, 1, 2, 3, 4 }, new long[] { 3, 23, 3, 24, 1002, 24, 10, 24, 1002, 23, -1, 23, 101, 5, 23, 23, 1, 24, 23, 23, 4, 23, 99, 0, 0 }, 54321)]
        [DataRow(new int[] { 1, 0, 4, 3, 2 }, new long[] { 3, 31, 3, 32, 1002, 32, 10, 32, 1001, 31, -2, 31, 1007, 31, 0, 33, 1002, 33, 7, 33, 1, 33, 31, 31, 1, 32, 31, 31, 4, 31, 99, 0, 0, 0 }, 65210)]
        public void TestNormalAmpCircuit(int[] phaseSequence, long[] instructions, int expected)
        {
            var ampCircuit = new AmpCircuit(phaseSequence, instructions, Mode.normal);
            var ampResult = ampCircuit.Execute();

            ampResult.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(new int[] { 9, 8, 7, 6, 5 }, new long[] { 3, 26, 1001, 26, -4, 26, 3, 27, 1002, 27, 2, 27, 1, 27, 26, 27, 4, 27, 1001, 28, -1, 28, 1005, 28, 6, 99, 0, 0, 5 }, 139629729)]
        [DataRow(new int[] { 9, 7, 8, 5, 6 }, new long[] { 3, 52, 1001, 52, -5, 52, 3, 53, 1, 52, 56, 54, 1007, 54, 5, 55, 1005, 55, 26, 1001, 54, -5, 54, 1105, 1, 12, 1, 53, 54, 53, 1008, 54, 0, 55, 1001, 55, 1, 55, 2, 53, 55, 53, 4, 53, 1001, 56, -1, 56, 1005, 56, 6, 99, 0, 0, 0, 0, 10 }, 18216)]
        public void TestFeedbackAmpCircuit(int[] phaseSequence, long[] instructions, int expected)
        {
            var ampCircuit = new AmpCircuit(phaseSequence, instructions, Mode.feedback);
            var ampResult = ampCircuit.Execute();

            ampResult.Should().Be(expected);
        }

        [TestMethod]
        public void Day7_Part1Tests_File()
        {
            var instructions = File.ReadAllText(InputFile1)
                .Split(",")
                .Select(x => long.Parse(x))
                .ToArray();

            var ampCircuit = new AmpCircuit(new int[] { 1, 2, 3, 4, 5 }, instructions, Mode.normal);
            var maxOutput = AmpCircuit.GetMaxAmpChainOutput(ampCircuit);
            
            maxOutput.Should().Be(95757);
        }

        [TestMethod]
        public void Day7_Part2Tests_File()
        {
            var instructions = File.ReadAllText(InputFile1)
                .Split(",")
                .Select(x => long.Parse(x))
                .ToArray();

            var ampCircuit = new AmpCircuit(new int[] { 1, 2, 3, 4, 5 }, instructions, Mode.feedback);
            var maxOutput = AmpCircuit.GetMaxAmpChainOutput(ampCircuit);

            maxOutput.Should().Be(4275738);
        }
    }
}
