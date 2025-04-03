using AvansDevOps.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.ItemStatePattern
{
    internal interface IItemState
    {
        void assignDeveloper(TeamMember teamMember);
        void readyForTesting();
        void testSucceeded();
        void testFailed();
        void definitionOfDoneCheck();
    }
}
