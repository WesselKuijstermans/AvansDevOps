using Spectre.Console;

namespace AvansDevOps.PipelineStrategyPattern {
    public class DeployPipeline : Pipeline {
        public override List<IPipelineStep> GetSteps() {
            var stepsToReturn = base.StepsList();
            if (!stepsToReturn.Any(step => step is DeployStep)) {
                stepsToReturn.Add(new DeployStep());
            }
            return stepsToReturn;
        }

        public override bool RunPipeline(bool result) {
            var steps = GetSteps();
            if (steps.Count > 0) {
                bool hasDeployStep = false;
                foreach (IPipelineStep step in steps) {
                    if (step is DeployStep) {
                        hasDeployStep = true;
                    }
                    step.Execute();
                }
                if (!hasDeployStep) {
                    new DeployStep().Execute();
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
    }
}
