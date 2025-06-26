using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;

namespace AvansDevOps.PipelineStrategyPattern
{
    public class DeployStep: IPipelineStep
    {
        public void Execute()
        {
            AnsiConsole.WriteLine("Deploying to cloud...");
        }
    }
}
