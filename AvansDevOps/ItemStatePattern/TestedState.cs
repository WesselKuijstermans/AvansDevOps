using AvansDevOps.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.ItemStatePattern
{
    public class TestedState(SprintItem sprintItem) : IItemState
    {
        private SprintItem _sprintItem = sprintItem;

        public void assignDeveloper(TeamMember teamMember)
        {
            Console.WriteLine("Item already tested. No developer can be assigned.");
        }

        public void readyForTesting()
        {
            Console.WriteLine("Tests already succeeded");
        }

        public void testSucceeded()
        {
            Console.WriteLine("Tests already succeeded");
        }

        public void testFailed()
        {
            Console.WriteLine("Tests already succeeded");
        }

        public void definitionOfDoneCheck()
        {
            List<SprintItem> subTasks = _sprintItem.GetSubtasks();
            if (subTasks.Count != 0)
            {
                for (int i = 0; i < subTasks.Count; i++)
                {
                    if (subTasks[i].GetState().GetType() != typeof(DoneState))
                    {
                        Console.WriteLine("Item does not meet the definition of done.");
                        Console.WriteLine("Subtask " + subTasks[i].GetBacklogItem().GetTask() + " is not done.");
                        _sprintItem.SetState(new ReadyForTestingState(_sprintItem));
                        return;
                    }
                }
            }
            Console.WriteLine("Item meets the definition of done.");
            _sprintItem.SetState(new DoneState(_sprintItem));
        }
    }
}
