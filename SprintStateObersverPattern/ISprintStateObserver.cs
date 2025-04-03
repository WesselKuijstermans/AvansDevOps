using AvansDevOps.Entities;
using AvansDevOps.ItemStatePattern;
using AvansDevOps.SprintStatePattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.SprintStateObersverPattern
{
    internal interface ISprintStateObserver
    {
        void SprintUpdate(ISprintState newState, Sprint sprint);
    }
}
