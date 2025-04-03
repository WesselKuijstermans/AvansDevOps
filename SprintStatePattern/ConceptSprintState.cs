using AvansDevOps.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.SprintStatePattern
{
    internal class ConceptSprintState(Sprint sprint) : ISprintState
    {
        private Sprint _sprint = sprint;

        public void StartRelease()
        {
            throw new NotImplementedException();
        }

        public void StartSprint()
        {
            throw new NotImplementedException();
        }

        public void StopSprint()
        {
            throw new NotImplementedException();
        }

        public void UploadSummary()
        {
            throw new NotImplementedException();
        }
    }
}
