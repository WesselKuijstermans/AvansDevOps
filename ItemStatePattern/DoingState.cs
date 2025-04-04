using AvansDevOps.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.ItemStatePattern
{
    public class DoingState(SprintItem sprintItem) : IItemState
    {
        private SprintItem _sprintItem = sprintItem;

        public void assignDeveloper(TeamMember teamMember)
        {
            Console.WriteLine("A developer is already assigned. Assigning new developer");
            this._sprintItem.SetDeveloper(teamMember);
		}

        public void readyForTesting()
        {
            Console.WriteLine("Item is ready for testing");
            _sprintItem.SetState(new ReadyForTestingState(_sprintItem));
            _sprintItem.SetDeveloper(null);
        }

        public void testSucceeded()
        {
            Console.WriteLine("Can't perform tests yet. Developer needs to mark the item as ready for testing first");
        }

        public void testFailed()
        {
            Console.WriteLine("Can't perform tests yet. Developer needs to mark the item as ready for testing first");
        }

        public void definitionOfDoneCheck()
        {
            Console.WriteLine("Can't review definition of done yet. Developer needs to mark the item as ready for testing first");
        }
    }
}
