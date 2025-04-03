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

        public Sprint(string name, DateTime startDate, DateTime endDate, PipelineTemplate pipeline, Project project)
        {
            this.name = name;
            this.startDate = startDate;
            this.endDate = endDate;
            this.sprintBacklog = new List<SprintItem>();
            this.pipeline = pipeline;
            this.state = new ConceptSprintState(this);
            this.project = project;
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

        public void UploadSummary()
        {
            this.state.UploadSummary();
        }

        public void StartRelease()
        {
            this.state.StartRelease();
        }
    }
}
