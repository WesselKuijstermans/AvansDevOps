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

		public virtual void RunPipeline()
		{
			if (this.steps.Count > 0)
			{
				foreach (IPipelineStep step in this.steps)
				{
					step.Execute();
				}
			}
		}
	}
}
