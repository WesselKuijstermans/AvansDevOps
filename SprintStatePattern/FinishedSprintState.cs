using AvansDevOps.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.SprintStatePattern
{
    internal class FinishedSprintState(Sprint sprint) : ISprintState
    {
        private Sprint _sprint = sprint;

        public void StartRelease()
        {
            Console.WriteLine("Sprint has been finalized. No more changes can be made.");
        }

        public void StartSprint()
        {
            Console.WriteLine("Sprint has been finalized. No more changes can be made.");
        }

        public void StopSprint()
        {
            Console.WriteLine("Sprint has been finalized. No more changes can be made.");
        }

        public void UploadSummary()
        {
            Console.WriteLine("Sprint has been finalized. No more changes can be made.");
        }
    }
}
