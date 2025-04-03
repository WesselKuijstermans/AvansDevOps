using AvansDevOps.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.ItemStatePattern
{
    internal class DoneState(SprintItem sprintItem) : IItemState
    {
        private SprintItem _sprintItem = sprintItem;

        public void assignDeveloper(TeamMember teamMember)
        {
            Console.WriteLine("Item is already done. No more changes can be made.");
        }

        public void readyForTesting()
        {
            Console.WriteLine("Item is already done. No more changes can be made.");
        }

        public void testSucceeded()
        {
            Console.WriteLine("Item is already done. No more changes can be made.");
        }

        public void testFailed()
        {
            Console.WriteLine("Item is already done. No more changes can be made.");
        }

        public void definitionOfDoneCheck()
        {
            Console.WriteLine("Item is already done. No more changes can be made.");
        }
    }
}
