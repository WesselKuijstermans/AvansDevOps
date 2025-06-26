using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;

namespace AvansDevOps.PipelineStrategyPattern
{
    public class BuildStep: IPipelineStep
    {
        public void Execute()
        {
            AnsiConsole.WriteLine("Building the project...");

        }
    }
}
