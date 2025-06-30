namespace AvansDevOps.PipelineStrategyPattern {
    public class DeployPipeline : Pipeline {
        public override List<IPipelineStep> GetSteps() {
            var stepsToReturn = base.StepsList();
            if (stepsToReturn.Count == 0) {
                return stepsToReturn;
            }
            if (!stepsToReturn.Any(step => step is DeployStep)) {
                stepsToReturn.Add(new DeployStep());
            }
            return stepsToReturn;
        }
    }
}
