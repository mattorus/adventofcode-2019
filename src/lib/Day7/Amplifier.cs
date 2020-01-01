using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using static AdventOfCode2019.IntcodeRunner;

namespace AdventOfCode2019
{
    public class Amplifier
    {
        public int PhaseSetting { get; private set; }
        public long ThrusterOutput { get; set; }

        private readonly IntcodeRunner _intcodeRunner;

        public Amplifier(int phaseSetting, long[] instructions, Mode inputMode = Mode.normal)
        {
            this.PhaseSetting = phaseSetting;

            _intcodeRunner = new IntcodeRunner(instructions)
            {
                InputMode = inputMode
            };
        }

        public void UpdateAndReset(int phaseSetting, int ampInput, long[] instructions = null)
        {
            this.PhaseSetting = phaseSetting;

            _intcodeRunner.UpdateAndReset(instructions, new long[] { PhaseSetting });
        }

        public void UpdateInput(long[] ampInput)
        {
            foreach (int i in ampInput)
            {
                _intcodeRunner.InputQueue.Enqueue(i);
            }
        }

        public void UpdateInput(long ampInput)
        {
            _intcodeRunner.InputQueue.Enqueue(ampInput);
        }

        public long Execute()
        {
            _intcodeRunner.Execute();
            ThrusterOutput = _intcodeRunner.GetLastOutput();

            return ThrusterOutput;
        }

        public void GetInputQueue(ref ConcurrentQueue<long> inputQueue)
        {
            inputQueue = _intcodeRunner.InputQueue;
        }

        public void RegisterOutputQueue(ref ConcurrentQueue<long> outputQueue)
        {
            _intcodeRunner.OutputQueue = outputQueue;
        }

        public List<long> GetOutputBuffer()
        {
            return _intcodeRunner.GetOutputBuffer();
        }

    }
}
