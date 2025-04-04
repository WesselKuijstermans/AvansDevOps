using AvansDevOps.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.ItemStatePattern
{
    public interface IItemState
    {
        void assignDeveloper(TeamMember teamMember);
        void readyForTesting();
        void testSucceeded();
        void testFailed();
        void definitionOfDoneCheck();
    }
}
