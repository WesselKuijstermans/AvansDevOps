﻿using AvansDevOps.FormMessageObersverPattern;
using AvansDevOps.SprintStateObersverPattern;
using AvansDevOps.SprintStatePattern;
using AvansDevOps.StateObserverPattern;
using AvansDevOps.TemplatePattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.Entities
{
    public class Sprint
    {
        private string name;
        private DateTime startDate;
        private DateTime endDate;
        private List<SprintItem> sprintBacklog;
        private PipelineTemplate pipeline;
        private ISprintState state;
        private ISprintStateObserver observer;
        private string summary;

        public Sprint(string name, DateTime startDate, DateTime endDate, PipelineTemplate pipeline, ISprintStateObserver observer)
        {
            this.name = name;
            this.startDate = startDate;
            this.endDate = endDate;
            this.sprintBacklog = new List<SprintItem>();
            this.pipeline = pipeline;
            this.state = new ConceptSprintState(this);
            this.observer = observer;
            this.summary = "";
        }

        public string GetName()
        {
            return this.name;
        }

        public void SetName(string name)
        {
            if (state is ConceptSprintState)
            {
                this.name = name;
            } else
            {
                Console.WriteLine("Cannot change the name of a sprint that has already started.");
            }
        }

        public DateTime GetStartDate()
        {
            return this.startDate;
        }

        public void SetStartDate(DateTime startDate)
        {
            if (state is ConceptSprintState)
            {
                this.startDate = startDate;
            }
            else
            {
                Console.WriteLine("Cannot change the startDate of a sprint that has already started.");
            }
        }

        public DateTime GetEndDate()
        {
            return this.endDate;
        }

        public void SetEndDate(DateTime endDate)
        {
            if (state is ConceptSprintState)
            {
                this.endDate = endDate;
            }
            else
            {
                Console.WriteLine("Cannot change the endDate of a sprint that has already started.");
            }
        }

        public List<SprintItem> GetSprintBacklog()
        {
            return this.sprintBacklog;
        }



        public void AddSprintItem(BacklogItem backlogItem, IItemStateObserver itemObserver, IFormMessageObserver formObserver)
        {
            this.sprintBacklog.Add(new SprintItem(this, backlogItem, itemObserver, formObserver));
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
            if (this.pipeline == null)
            {
                Console.WriteLine("Pipeline is not set.");
                return null;
            }
            return this.pipeline.GetSteps();
        }

        public void RunPipeline()
        {
            this.pipeline.RunPipeline();
        }

        public void SetState(ISprintState state)
        {
            this.state = state;
            this.observer.SprintUpdate(state, this);
        }

        public ISprintState GetState()
        {
            return this.state;
        }

        public void SetSummary(string summary)
        {
            this.summary = summary;
        }

        public string GetSummary()
        {
            return this.summary;
        }
    }
}
