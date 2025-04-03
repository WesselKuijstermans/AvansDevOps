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
            throw new NotImplementedException();
        }

        public void StartSprint()
        {
            Console.WriteLine("Cannot start Sprint. Sprint has already ended.");
        }

        public void StopSprint()
        {
            Console.WriteLine("Cannot stop Sprint. Sprint has already ended.");
        }

        public void UploadSummary()
        {
            throw new NotImplementedException();
        }
    }
}
