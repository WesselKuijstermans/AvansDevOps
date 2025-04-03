using AvansDevOps.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.ItemStatePattern
{
    internal class TodoState(SprintItem sprintItem) : IItemState
    {
        private SprintItem _sprintItem = sprintItem;

        public void assignDeveloper(TeamMember teamMember)
        {
            Console.WriteLine("Assigning developer to item");
            _sprintItem.SetDeveloper(teamMember);
            _sprintItem.SetState(new DoingState(_sprintItem));
        }

        public void readyForTesting()
        {
            Console.WriteLine("Can't move to ready for testing. Assign a developer first");
        }

        public void testSucceeded()
        {
            Console.WriteLine("Can't perform tests yet. Assign a developer first");
        }

        public void testFailed()
        {
            Console.WriteLine("Can't perform tests yet. Assign a developer first");
        }

        public void definitionOfDoneCheck()
        {
            Console.WriteLine("Can't review definition of done yet. Assign a developer first");
        }
    }
}
