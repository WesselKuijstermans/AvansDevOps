using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.TemplatePattern
{
    internal abstract class PipelineTemplate
    {
        private List<IPipelineStep> steps;
        private static readonly Random random = new Random();

        public PipelineTemplate()
        {
            this.steps = new List<IPipelineStep>();
        }

        public void AddStep(IPipelineStep step)
        {
            this.steps.Add(step);
        }

        public void RemoveStep(IPipelineStep step)
        {
            this.steps.Remove(step);
        }

        public List<IPipelineStep> GetSteps()
        {
            return this.steps;
        }

        public virtual bool RunPipeline()
        {
            if (this.steps.Count > 0)
            {
                foreach (IPipelineStep step in this.steps)
                {
                    step.Execute();
                }
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
