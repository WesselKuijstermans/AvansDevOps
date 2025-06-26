using Spectre.Console;

namespace AvansDevOps.PipelineStrategyPattern {
    public class TestPipeline : Pipeline {
        public override List<IPipelineStep> GetSteps() {
            return [.. base.StepsList(), new TestStep()];
        }

        public override bool RunPipeline(bool result) {
            var steps = GetSteps();
            if (steps.Count > 0) {
                bool hasTestStep = false;
                foreach (IPipelineStep step in steps) {
                    if (step is TestStep) {
                        hasTestStep = true;
                    }
                    step.Execute();
                }
                if (!hasTestStep) {
                    new TestStep().Execute();
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
