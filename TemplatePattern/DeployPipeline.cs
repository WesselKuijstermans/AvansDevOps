using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.TemplatePattern
{
    public class DeployPipeline : PipelineTemplate
    {
		private static readonly Random random = new Random();

		public override bool RunPipeline()
        {
            var steps = this.GetSteps();
            if (steps.Count > 0)
            {
                foreach (IPipelineStep step in steps)
                {
                    step.Execute();
                }
                new DeployStep().Execute();
				if (random.Next(5) == 0) // 1 in 5 chance of failure
				{
					Console.WriteLine("Pipeline failed.");
					return false;
				}
				else
				{
					Console.WriteLine("Pipeline succeeded.");
					return true;
				}
			}
			else
			{
				Console.WriteLine("No steps in the pipeline.");
				return false;
			}
        }
    }
}
