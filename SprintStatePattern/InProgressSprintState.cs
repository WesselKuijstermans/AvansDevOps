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
            throw new NotImplementedException();
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

        public void UploadSummary()
        {
            throw new NotImplementedException();
        }
    }
}
