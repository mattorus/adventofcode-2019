using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using static AdventOfCode2019.IntcodeRunner;

namespace AdventOfCode2019
{
    public class Amplifier
    {
        public int PhaseSetting { get; private set; }
        public int ThrusterOutput { get; set; }

        private readonly IntcodeRunner _intcodeRunner;

        public Amplifier(int phaseSetting, int[] instructions, Mode inputMode = Mode.normal)
        {
            this.PhaseSetting = phaseSetting;

            _intcodeRunner = new IntcodeRunner(instructions)
            {
                InputMode = inputMode
            };
        }

        public void UpdateAndReset(int phaseSetting, int ampInput, int[] instructions = null)
        {
            this.PhaseSetting = phaseSetting;

            _intcodeRunner.UpdateAndReset(instructions, new int[] { PhaseSetting });
        }

        public void UpdateInput(int[] ampInput)
        {
            foreach (int i in ampInput)
            {
                _intcodeRunner.InputQueue.Enqueue(i);
            }
        }

        public void UpdateInput(int ampInput)
        {
            _intcodeRunner.InputQueue.Enqueue(ampInput);
        }

        public int Execute()
        {
            _intcodeRunner.Execute();
            ThrusterOutput = _intcodeRunner.GetLastOutput();

            return ThrusterOutput;
        }

        public void GetInputQueue(ref ConcurrentQueue<int> inputQueue)
        {
            inputQueue = _intcodeRunner.InputQueue;
        }

        public void RegisterOutputQueue(ref ConcurrentQueue<int> outputQueue)
        {
            _intcodeRunner.OutputQueue = outputQueue;
        }

        public List<int> GetOutputBuffer()
        {
            return _intcodeRunner.GetOutputBuffer();
        }

    }
}
