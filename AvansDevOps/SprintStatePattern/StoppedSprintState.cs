using AvansDevOps.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.SprintStatePattern
{
    public class StoppedSprintState(Sprint sprint) : ISprintState
    {
        private Sprint _sprint = sprint;

        public void StartRelease()
        {
            if (_sprint.GetPipelineSteps() == null) {
                Console.WriteLine("This is not a release sprint. Cannot start release.");
                return;
            } 
            else if (_sprint.GetPipelineSteps().Count == 0)
            {
                Console.WriteLine("No steps in the pipeline. Cannot start release.");
                return;
            }
            _sprint.RunPipeline();
            Console.WriteLine("Release started.");
            Console.WriteLine("Sprint has been stopped. No more changes can be made.");
            _sprint.SetState(new FinishedSprintState(_sprint));
        }

        public void StartSprint()
        {
            Console.WriteLine("Cannot start Sprint. Sprint has already ended.");
        }

        public void StopSprint()
        {
            Console.WriteLine("Cannot stop Sprint. Sprint has already ended.");
        }

        public void UploadSummary(string summary)
        {
            if (_sprint.GetPipelineSteps() != null)
            {
                Console.WriteLine("This is a release sprint. Cannot upload summary.");
                return;
            }
            _sprint.SetSummary(summary);
            Console.WriteLine("Summary uploaded.");
            Console.WriteLine("Sprint has been stopped. No more changes can be made.");
            _sprint.SetState(new FinishedSprintState(_sprint));
        }
    }
}
