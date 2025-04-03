using AvansDevOps.Entities;
using AvansDevOps.ItemStatePattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.StateObserverPattern
{
    internal interface IItemStateObserver
    {
        void Update(IItemState newState, SprintItem item);
    }
}
