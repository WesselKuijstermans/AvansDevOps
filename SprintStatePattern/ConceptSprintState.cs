using AvansDevOps.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.SprintStatePattern
{
    public class ConceptSprintState(Sprint sprint) : ISprintState
    {
        private Sprint _sprint = sprint;

        public void StartRelease()
        {
            Console.WriteLine("Sprint not started yet. Wait until the sprint is done to start release");
        }

        public void StartSprint()
        {
            Console.WriteLine("Starting sprint");
            _sprint.SetState(new InProgressSprintState(_sprint));
        }

        public void StopSprint()
        {
            Console.WriteLine("Cannot stop sprint, because it has not started yet");
        }

        public void UploadSummary(string summary)
        {
            Console.WriteLine("Sprint not started yet. Wait until the sprint is done to upload a summary");
        }
    }
}
