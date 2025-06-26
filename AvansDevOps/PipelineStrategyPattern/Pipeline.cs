using Spectre.Console;

namespace AvansDevOps.PipelineStrategyPattern {
    public abstract class Pipeline {
        private readonly List<IPipelineStep> steps;

        public Pipeline() {
            steps = [];
        }

        public void AddStep(IPipelineStep step) {
            steps.Add(step);
        }

        public void RemoveStep(IPipelineStep step) {
            steps.Remove(step);
        }

        abstract public List<IPipelineStep> GetSteps();

        public virtual bool RunPipeline(bool result) {
            if (steps.Count > 0) {
                foreach (IPipelineStep step in steps) {
                    step.Execute();
                }
                if (result) {
                    AnsiConsole.WriteLine("Pipeline successful");
                } else {
                    AnsiConsole.WriteLine("Pipeline failed");
                }
                return result;
            } else {
                AnsiConsole.WriteLine("No steps in the pipeline.");
                return false;
            }
        }

        internal List<IPipelineStep> StepsList() {
            return steps;
        }
    }
}
