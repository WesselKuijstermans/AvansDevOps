using AvansDevOps.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.SprintStatePattern
{
    internal class StoppedSprintState(Sprint sprint) : ISprintState
    {
        private Sprint _sprint = sprint;

        public void StartRelease()
        {
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
            _sprint.SetSummary(summary);
            Console.WriteLine("Summary uploaded.");
            Console.WriteLine("Sprint has been stopped. No more changes can be made.");
            _sprint.SetState(new FinishedSprintState(_sprint));
        }
    }
}
