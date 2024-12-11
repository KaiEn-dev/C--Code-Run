using System;
using System.Reflection.PortableExecutable;

public abstract class ChallengeDay
{
    public int Day { get; set; }
    public string Problem { get; set; }
    public string Instruction { get; set; }

    public int? CompletionTime { get; set; }

    public ChallengeDay(int day, string problem, string instruction, int? completionTime = null)
    {
        this.Day = day;
        this.Problem = problem;
        this.Instruction = instruction;
        this.CompletionTime = completionTime;
    }

    public override string ToString()
    {
        Console.WriteLine(
            $@"
-----------------
Challenge Day {Day}
-----------------
Problem:
{Problem}

Instruction:
{Instruction}

Completion Time:
{CompletionTime ?? 0}

"
        );
        InputInterface();
        return base.ToString() ?? "-";
    }

    public abstract void InputInterface();
}
