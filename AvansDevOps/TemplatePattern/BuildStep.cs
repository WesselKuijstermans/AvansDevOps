using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.TemplatePattern
{
    public class BuildStep: IPipelineStep
    {
        public void Execute()
        {
            Console.WriteLine("Building the project...");

        }
    }
}
