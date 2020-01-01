namespace AdventOfCode2019
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Linq;
    using System.Collections.Concurrent;

    public class IntcodeRunner
    {
        public enum Mode
        {
            normal = 0,
            feedback = 1,
        }

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
            RelativeBaseOffset = 9,
            End = 99,
        }

        private readonly List<long> _outputBuffer;
        private long[] _instructions;
        private int _maxBuffer = 50;
        private long _relativeBase = 0;

        /// <summary>
        /// InputMode
        /// </summary>
        public Mode InputMode { get; set; }

        /// <summary>
        /// Represents user input
        /// </summary>
        public ConcurrentQueue<long> InputQueue { get; set; }
        
        /// <summary>
        /// Intcode output, for consumption during feedback mode
        /// </summary>
        public ConcurrentQueue<long> OutputQueue { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="IntcodeRunner"/> class.
        /// </summary>
        /// <param name="instructions">Program instructions.</param>
        /// <param name="userInput">Test hook - simulates command line input.</param>
        public IntcodeRunner(long[] instructions)
        {
            _instructions = instructions ?? throw new ArgumentNullException(nameof(instructions));
            InputQueue = new ConcurrentQueue<long>();
            OutputQueue = null;
            _outputBuffer = new List<long>();
            InputMode = Mode.normal;
        }
        
        /// <summary>
        /// Returns the most recent output.
        /// </summary>
        /// <returns>int</returns>
        public long GetLastOutput()
        {
            _outputBuffer.TrimExcess();

            if (OutputQueue != null && !OutputQueue.IsEmpty)
            {
                OutputQueue.TryPeek(out long result);

                return result;
            }
            else
            {
                return _outputBuffer[^1];
            }
        }

        /// <summary>
        /// Returns output buffer.
        /// </summary>
        /// <returns>List</returns>
        public List<long> GetOutputBuffer()
        {
            _outputBuffer.TrimExcess();

            return _outputBuffer;
        }

        /// <summary>
        /// Updates stored instructions and input, if given, and resets user input pointer.
        /// </summary>
        /// <param name="instructions"></param>
        /// <param name="userInput"></param>
        public void UpdateAndReset(long[] instructions = null, long[] userInput = null)
        {
            if (instructions != null)
            {
                _instructions = instructions;
            }
            
            if (userInput != null)
            {
                foreach (int i in userInput)
                {
                    InputQueue.Enqueue(i);
                }
            }
        }

        /// <summary>
        /// Executes the opcodes and returns the results.
        /// </summary>
        /// <returns>Intcode runner results.</returns>
        public long[] Execute()
        {
            return this.Execute(_instructions[1], _instructions[2]);
        }

        /// <summary>
        /// Executes the opcodes and returns the results.
        /// </summary>
        /// <param name="noun">Noun value.</param>
        /// <param name="verb">Verb value.</param>
        /// <returns>Intcode runner results.</returns>
        public long[] Execute(long noun, long verb)
        {
            var output = (InputMode == Mode.feedback ? _instructions : (long[])_instructions.Clone());
            long[] opcodeParams;
            int index = 0, outputIndex = -1, outputIndexOffset = 0, numParams = 0;

            output[1] = noun;
            output[2] = verb;                        
            var (opcode, operModes) = ParseOpcode(output[index]);
            //_outputBuffer.Clear();

            while (opcode != Opcode.End)
            {
                numParams = 2;
                outputIndexOffset = 0;

                switch (opcode)
                {                    
                    case Opcode.Add:
                    case Opcode.Multiply:
                    case Opcode.LessThan:
                    case Opcode.Equals:
                        outputIndexOffset = 1;
                        break;
                    case Opcode.Read:
                    case Opcode.Print:
                    case Opcode.RelativeBaseOffset:
                        numParams = 1;
                        break;
                    case Opcode.JumpIfTrue:
                    case Opcode.JumpIfFalse:
                    default:
                        break;
                }

                outputIndex = index + numParams + outputIndexOffset;

                opcodeParams = new long[numParams];
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
        private (Opcode opcode, long opermodes) ParseOpcode(long opcode)
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
        private bool Execute(Opcode opcode, long operModes, ref int index, long[] opcodeParams, long[] instructions, long outputIndex)
        {
            bool indexUpdated = false;
            int i = 0;

            for (i = 0; i < opcodeParams.Length; i++)
            {
                switch (operModes % 10)
                {
                    case 0:
                        opcodeParams[i] = instructions[opcodeParams[i]];
                        break;
                    case 2:
                        opcodeParams[i] = instructions[opcodeParams[i] + _relativeBase];
                        break;
                    default:
                        break;
                }

                operModes /= 10;
            }

            switch (opcode)
            {
                case Opcode.Add:
                    instructions[outputIndex] = opcodeParams.Aggregate((long) 0, (x, y) => x + y);
                    break;

                case Opcode.Multiply:
                    instructions[outputIndex] = opcodeParams.Aggregate((long) 1, (x, y) => x * y);
                    break;

                case Opcode.Read:
                    long input = 0;

                    while (!InputQueue.TryDequeue(out input))
                    {
                        Thread.Sleep(1); // TODO: TOFIX: Should utilize wait instead
                    }

                    instructions[outputIndex] = input;

                    break;

                case Opcode.Print:
                    Print(instructions[outputIndex]);
                    break;

                case Opcode.JumpIfTrue:
                    if (opcodeParams[0] != 0)
                    {
                        index = (int) opcodeParams[^1];
                        indexUpdated = true;
                    }

                    break;

                case Opcode.JumpIfFalse:
                    if (opcodeParams[0] == 0)
                    {
                        index = (int) opcodeParams[^1];
                        indexUpdated = true;
                    }

                    break;

                case Opcode.LessThan:
                    instructions[outputIndex] = (int) (opcodeParams[0] < opcodeParams[1] ? 1 : 0);                    
                    break;

                case Opcode.Equals:
                    instructions[outputIndex] = (int) (opcodeParams[0] == opcodeParams[1] ? 1 : 0);
                    break;

                case Opcode.RelativeBaseOffset:
                    _relativeBase += opcodeParams[0];
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(opcode), $"Invalid opcode specified! -- {opcode}");
            }

            return indexUpdated;
        }

        /// <summary>
        /// Handles the output functionality.
        /// </summary>
        /// <param name="value"></param>
        private void Print(long value)
        {
            _outputBuffer.Add(value);

            if (InputMode == Mode.feedback)
            {
                OutputQueue.Enqueue(value);
            }

            while (_outputBuffer.Count > _maxBuffer)
            {
                _outputBuffer.RemoveAt(0);
            }

            Console.WriteLine(value);
        }
    }
}
