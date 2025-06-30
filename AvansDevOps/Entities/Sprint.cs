using AvansDevOps.FacadePattern;
using AvansDevOps.FormMessageObersverPattern;
using AvansDevOps.ItemStateObserverPattern;
using AvansDevOps.PipelineStrategyPattern;
using AvansDevOps.SprintStateObersverPattern;
using AvansDevOps.SprintStatePattern;
using AvansDevOps.VersionControlStrategyPattern;
using Spectre.Console;

namespace AvansDevOps.Entities {
    public class Sprint {
        private string name;
        private DateTime startDate;
        private DateTime endDate;
        private readonly List<SprintItem> sprintBacklog;
        private Pipeline? pipeline;
        private ISprintState state;
        private readonly ISprintStateObserver observer;
        private string summary;

        public Sprint(string name, DateTime startDate, DateTime endDate, Pipeline? pipeline, ISprintStateObserver observer) {
            this.name = name;
            this.startDate = startDate;
            this.endDate = endDate;
            this.sprintBacklog = [];
            this.pipeline = pipeline;
            this.state = new ConceptSprintState(this);
            this.observer = observer;
            this.summary = string.Empty;
        }

        public string GetName() {
            return this.name;
        }

        public void SetName(string name) {
            if (state is ConceptSprintState) {
                this.name = name;
            } else {
                AnsiConsole.MarkupLine("Cannot change the name of a sprint that has already started.");
            }
        }

        public DateTime GetStartDate() {
            return this.startDate;
        }

        public void SetStartDate(DateTime startDate) {
            if (state is ConceptSprintState) {
                this.startDate = startDate;
            } else {
                AnsiConsole.MarkupLine("Cannot change the startDate of a sprint that has already started.");
            }
        }

        public DateTime GetEndDate() {
            return this.endDate;
        }

        public void SetEndDate(DateTime endDate) {
            if (state is ConceptSprintState) {
                this.endDate = endDate;
            } else {
                AnsiConsole.MarkupLine("Cannot change the endDate of a sprint that has already started.");
            }
        }

        public List<SprintItem> GetSprintBacklog() {
            return this.sprintBacklog;
        }



        public void AddSprintItem(BacklogItem backlogItem, IItemStateObserver itemObserver, IFormMessageObserver formObserver, IVersionControlStrategy strategy) {
            SprintItem sprintItem = new(backlogItem, itemObserver, formObserver, new VersionControlFacade(strategy, null));
            sprintItem.versionControlFacade = new VersionControlFacade(strategy, sprintItem);
            this.sprintBacklog.Add(sprintItem);

        }

        public void RemoveSprintItem(SprintItem sprintItem) {
            this.sprintBacklog.Remove(sprintItem);
        }

        public void StartSprint() {
            this.state.StartSprint();
        }

        public void StopSprint() {
            this.state.StopSprint();
        }

        public void UploadSummary(string summary) {
            this.state.UploadSummary(summary);
        }

        public bool StartRelease() {
            return this.state.StartRelease();
        }

        public void AddStepToPipeline(IPipelineStep step) {
            this.pipeline.AddStep(step);
        }

        public void RemoveStepFromPipeline(IPipelineStep step) {
            this.pipeline.RemoveStep(step);
        }

        public List<IPipelineStep>? GetPipelineSteps() {
            if (this.pipeline == null) {
                AnsiConsole.MarkupLine("Pipeline is not set.");
                return null;
            }
            return this.pipeline.GetSteps();
        }

        public void SetPipeline(Pipeline pipeline) {
            this.pipeline = pipeline;
        }

        public bool RunPipeline() {
            return this.pipeline.RunPipeline(true);
        }

        public void SetState(ISprintState state) {
            this.state = state;
            this.observer.SprintUpdate(state, this);
        }

        public ISprintState GetState() {
            return this.state;
        }

        public void SetSummary(string summary) {
            this.summary = summary;
        }

        public string GetSummary() {
            return this.summary;
        }

        public override string ToString() {
            // Print the name of the state (e.g., "Concept", "Active", etc.)
            var stateType = GetState().GetType().Name;
            var suffix = "SprintState";
            var stateName = stateType.EndsWith(suffix) ? stateType[..^suffix.Length] : stateType;
            return $"{this.name}: {stateName}";
        }
    }
}
