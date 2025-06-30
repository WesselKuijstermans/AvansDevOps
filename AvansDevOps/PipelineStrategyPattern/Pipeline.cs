using Spectre.Console;

namespace AvansDevOps.PipelineStrategyPattern {
    public abstract class Pipeline {
        private readonly List<IPipelineStep> steps;

        protected Pipeline() {
            steps = [];
        }

        public void AddStep(IPipelineStep step) {
            steps.Add(step);
        }

        public void RemoveStep(IPipelineStep step) {
            steps.Remove(step);
        }

        abstract public List<IPipelineStep> GetSteps();

        public bool RunPipeline(bool result) {
            var stepsToRun = GetSteps();
            if (stepsToRun.Count > 0) {
                foreach (IPipelineStep step in stepsToRun) {
                    step.Execute();
                }
                if (!result) {
                    AnsiConsole.WriteLine("Pipeline failed.");
                } else {
                    AnsiConsole.WriteLine("Pipeline succeeded.");
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

        public void SetSteps(List<IPipelineStep> steps) {
            this.steps.Clear();
            this.steps.AddRange(steps);
        }
    }
}
