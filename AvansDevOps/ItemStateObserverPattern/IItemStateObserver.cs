using AvansDevOps.Entities;
using AvansDevOps.ItemStatePattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.ItemStateObserverPattern
{
    public interface IItemStateObserver
    {
        void ItemUpdate(IItemState newState, SprintItem item);
    }
}
