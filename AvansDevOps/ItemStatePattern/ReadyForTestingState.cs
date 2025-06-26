using AvansDevOps.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;

namespace AvansDevOps.ItemStatePattern
{
    public class ReadyForTestingState(SprintItem sprintItem) : IItemState
    {
        private readonly SprintItem _sprintItem = sprintItem;

        public void AssignDeveloper(TeamMember teamMember)
        {
            AnsiConsole.WriteLine("Item is already ready for testing. No developer can be assigned.");
        }

        public void ReadyForTesting()
        {
            AnsiConsole.WriteLine("Item is already ready for testing.");
        }

        public void TestSucceeded()
        {
            AnsiConsole.WriteLine("Item test succeeded.");
            _sprintItem.SetState(new TestedState(_sprintItem));
        }

        public void TestFailed()
        {
            AnsiConsole.WriteLine("Item test failed.");
            AnsiConsole.WriteLine("Item set back to ToDo.");
            _sprintItem.SetState(new TodoState(_sprintItem));
        }

        public void DefinitionOfDoneCheck()
        {
            AnsiConsole.WriteLine("Can't review definition of done yet. Item needs to be tested first.");
        }
    }
}
