namespace AvansDevOps.PipelineStrategyPattern {
    public class TestPipeline : Pipeline {
        public override List<IPipelineStep> GetSteps() {
            var stepsToReturn = base.StepsList();
            if (stepsToReturn.Count == 0) {
                return stepsToReturn;
            }
            if (!stepsToReturn.Any(step => step is TestStep)) {
                stepsToReturn.Add(new TestStep());
            }
            return stepsToReturn;
        }
    }
}
