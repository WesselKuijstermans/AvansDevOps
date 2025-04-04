using AvansDevOps.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.ItemStatePattern
{
    public class ReadyForTestingState(SprintItem sprintItem) : IItemState
    {
        private SprintItem _sprintItem = sprintItem;

        public void assignDeveloper(TeamMember teamMember)
        {
            Console.WriteLine("Item is already ready for testing. No developer can be assigned.");
        }

        public void readyForTesting()
        {
            Console.WriteLine("Item is already ready for testing.");
        }

        public void testSucceeded()
        {
            Console.WriteLine("Item test succeeded.");
            _sprintItem.SetState(new TestedState(_sprintItem));
        }

        public void testFailed()
        {
            Console.WriteLine("Item test failed.");
            Console.WriteLine("Item set back to ToDo.");
            _sprintItem.SetState(new TodoState(_sprintItem));
        }

        public void definitionOfDoneCheck()
        {
            Console.WriteLine("Can't review definition of done yet. Item needs to be tested first.");
        }
    }
}
