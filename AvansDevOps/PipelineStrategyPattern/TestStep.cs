using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Spectre.Console;

namespace AvansDevOps.PipelineStrategyPattern
{
    public class TestStep: IPipelineStep
    {
        public void Execute()
        {
            int i = 1;
            while (i < 10)
            {
                // sleep for a tenth of a second
                Thread.Sleep(100);
                AnsiConsole.WriteLine("Running test " + i);
                // sleep for a random amount of time between 0 and 1 second
                Random random = new();
                Thread.Sleep(random.Next(0, 1000));
                AnsiConsole.WriteLine("Test " + i + " succeeded.");
                i++;
            }

        }
    }
}
