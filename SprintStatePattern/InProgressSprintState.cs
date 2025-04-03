using AvansDevOps.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.SprintStatePattern
{
    internal class InProgressSprintState(Sprint sprint) : ISprintState
    {
        private Sprint _sprint = sprint;

        public void StartRelease()
        {
            Console.WriteLine("Sprint not over yet. Wait until the sprint has been stopped to start release");
        }

        public void StartSprint()
        {
            Console.WriteLine("Sprint already started");
        }

        public void StopSprint()
        {
            Console.WriteLine("Stopping sprint");
            _sprint.SetState(new StoppedSprintState(_sprint));
        }

        public void UploadSummary(string summary)
        {
            Console.WriteLine("Sprint not over yet. Wait until the sprint has been stopped to upload a summary");
        }
    }
}
