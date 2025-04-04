using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.SprintStatePattern
{
    public interface ISprintState
    {
        void StartSprint();
        void StopSprint();
        void StartRelease();
        void UploadSummary(string summary);

    }
}
