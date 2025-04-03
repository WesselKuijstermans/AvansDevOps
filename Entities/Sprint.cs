using AvansDevOps.SprintStatePattern;
using AvansDevOps.TemplatePattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.Entities
{
    internal class Sprint
    {
        private string name;
        private DateTime startDate;
        private DateTime endDate;
        private List<SprintItem> sprintBacklog;
        private PipelineTemplate pipeline;
        private ISprintState state;
        private Project project;
        private string summary;

        public Sprint(string name, DateTime startDate, DateTime endDate, PipelineTemplate pipeline, Project project)
        {
            this.name = name;
            this.startDate = startDate;
            this.endDate = endDate;
            this.sprintBacklog = new List<SprintItem>();
            this.pipeline = pipeline;
            this.state = new ConceptSprintState(this);
            this.project = project;
            this.summary = "";
        }

        public string GetName()
        {
            return this.name;
        }

        public void AddSprintItem(BacklogItem backlogItem)
        {
            this.sprintBacklog.Add(new SprintItem(this, backlogItem, project));
        }

        public void RemoveSprintItem(SprintItem sprintItem)
        {
            this.sprintBacklog.Remove(sprintItem);
        }

        public void StartSprint()
        {
            this.state.StartSprint();
        }

        public void StopSprint()
        {
            this.state.StopSprint();
        }

        public void UploadSummary(string summary)
        {
            this.state.UploadSummary(summary);
        }

        public void StartRelease()
        {
            this.state.StartRelease();
        }

        public void AddStepToPipeline(IPipelineStep step)
        {
            this.pipeline.AddStep(step);
        }

        public void RemoveStepFromPipeline(IPipelineStep step)
        {
            this.pipeline.RemoveStep(step);
        }

        public List<IPipelineStep> GetPipelineSteps()
        {
            return this.pipeline.GetSteps();
        }

        public void RunPipeline()
        {
            this.pipeline.RunPipeline();
        }

        public void SetState(ISprintState state)
        {
            this.state = state;
            this.project.SprintUpdate(state, this);
        }

        public void SetSummary(string summary)
        {
            this.summary = summary;
        }
    }
}
