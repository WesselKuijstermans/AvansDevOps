using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.TemplatePattern
{
    internal class DeployPipeline : PipelineTemplate
    {


        public override void RunPipeline()
        {
            var steps = this.GetSteps();
            if (steps.Count > 0)
            {
                foreach (IPipelineStep step in steps)
                {
                    step.Execute();
                }
                new DeployStep().Execute();
            }
        }
    }
}
