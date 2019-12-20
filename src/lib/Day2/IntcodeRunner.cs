namespace AdventOfCode2019.Day2
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class IntcodeRunner
    {
        private enum Opcode
        {
            Unknown = 0,
            Add = 1,
            Multiply = 2,
            Read = 3,
            Print = 4,
            JumpIfTrue = 5,
            JumpIfFalse = 6,
            LessThan = 7,
            Equals = 8,
            End = 99,
        }

        private readonly int[] instructions;
        private readonly int[] userInput;
        private int userInputPos;

        /// <summary>
        /// Initializes a new instance of the <see cref="IntcodeRunner"/> class.
        /// </summary>
        /// <param name="instructions">Program instructions.</param>
        /// <param name="userInput">Test hook - simulates command line input.</param>
        public IntcodeRunner(int[] instructions, int[] userInput = null)
        {
            this.instructions = instructions ?? throw new ArgumentNullException(nameof(instructions));
            this.userInput = userInput ?? new int[] { 1 };
            userInputPos = 0;
        }

        /// <summary>
        /// Executes the opcodes and returns the results.
        /// </summary>
        /// <returns>Intcode runner results.</returns>
        public int[] Execute()
        {
            return this.Execute(this.instructions[1], this.instructions[2]);
        }

        /// <summary>
        /// Executes the opcodes and returns the results.
        /// </summary>
        /// <param name="noun">Noun value.</param>
        /// <param name="verb">Verb value.</param>
        /// <returns>Intcode runner results.</returns>
        public int[] Execute(int noun, int verb)
        {
            var output = (int[])this.instructions.Clone();
            var outputIndex = -1;
            output[1] = noun;
            output[2] = verb;

            int index = 0, numParams = 0;            
            var (opcode, operModes) = ParseOpcode(output[index]);

            while (opcode != Opcode.End)
            {
                switch (opcode)
                {
                    case Opcode.Add:
                    case Opcode.Multiply:
                    case Opcode.LessThan:
                    case Opcode.Equals:
                        numParams = 2;
                        outputIndex = index + numParams + 1;
                        break;
                    case Opcode.JumpIfTrue:
                    case Opcode.JumpIfFalse:
                        numParams = 2;
                        outputIndex = index + numParams;
                        break;
                    case Opcode.Read:
                    case Opcode.Print:
                        numParams = 1;
                        outputIndex = index + numParams;
                        break;
                }

                int[] opcodeParams = new int[numParams];
                Array.Copy(output, index + 1, opcodeParams, 0, numParams);

                if (!this.Execute(opcode, operModes, ref index, opcodeParams, output, output[outputIndex]))
                {
                    index = outputIndex + 1;
                }

                (opcode, operModes) = ParseOpcode(output[index]);
            }

            return output;
        }

        /// <summary>
        /// Parses the given opcode into instruction opcode and operand modes
        /// </summary>
        /// <param name="opcode"></param>
        /// <returns>
        ///    opcode  - the two right-most digits of parameter
        ///    opModes - the remaining digits of parameter
        /// </returns>
        private (Opcode opcode, int opermodes) ParseOpcode(int opcode)
        {
            return ((Opcode)(opcode % 100), opcode / 100);
        }

        /// <summary>
        /// Executes a single instruction, returns bool indicating whether or not instruction pointer was updated
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="operModes"></param>
        /// <param name="index"></param>
        /// <param name="numParams"></param>
        /// <param name="instructions"></param>
        /// <returns>True if instruction pointer was updated, false otherwise</returns>
        private bool Execute(Opcode opcode, int operModes, ref int index, int[] opcodeParams, int[] instructions, int outputIndex)
        {
            bool indexUpdated = false;
            int i = 0;

            for (i = 0; i < opcodeParams.Length; i++)
            {
                if (operModes % 10 == 0)
                {
                    opcodeParams[i] = instructions[opcodeParams[i]];
                }

                operModes /= 10;
            }


            switch (opcode)
            {
                case Opcode.Add:
                    instructions[outputIndex] = opcodeParams.Aggregate(0, (x, y) => x + y);
                    break;

                case Opcode.Multiply:
                    instructions[outputIndex] = opcodeParams.Aggregate(1, (x, y) => x * y);
                    break;

                case Opcode.Read:
                    if (userInput == null)
                    {
                        throw new ArgumentOutOfRangeException(nameof(opcode), $"User input expected! -- {opcode}");
                    }

                    instructions[outputIndex] = userInput[userInputPos++];
                    break;

                case Opcode.Print:
                    Console.WriteLine(instructions[outputIndex]);
                    break;

                case Opcode.JumpIfTrue:
                    if (opcodeParams[0] != 0)
                    {
                        index = opcodeParams[^1];
                        indexUpdated = true;
                    }

                    break;

                case Opcode.JumpIfFalse:
                    if (opcodeParams[0] == 0)
                    {
                        index = opcodeParams[^1];
                        indexUpdated = true;
                    }

                    break;

                case Opcode.LessThan:
                    instructions[outputIndex] = (opcodeParams[0] < opcodeParams[1] ? 1 : 0);                    
                    break;

                case Opcode.Equals:
                    instructions[outputIndex] = (opcodeParams[0] == opcodeParams[1] ? 1 : 0);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(opcode), $"Invalid opcode specified! -- {opcode}");
            }

            return indexUpdated;
        }
    }
}
