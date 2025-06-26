using AvansDevOps.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;

namespace AvansDevOps.ItemStatePattern
{
    public class TodoState(SprintItem sprintItem) : IItemState
    {
        private readonly SprintItem _sprintItem = sprintItem;

        public void AssignDeveloper(TeamMember teamMember)
        {
            AnsiConsole.WriteLine("Assigning developer to item");
            _sprintItem.SetDeveloper(teamMember);
            _sprintItem.SetState(new DoingState(_sprintItem));
        }

        public void ReadyForTesting()
        {
            AnsiConsole.WriteLine("Can't move to ready for testing. Assign a developer first");
        }

        public void TestSucceeded()
        {
            AnsiConsole.WriteLine("Can't perform tests yet. Assign a developer first");
        }

        public void TestFailed()
        {
            AnsiConsole.WriteLine("Can't perform tests yet. Assign a developer first");
        }

        public void DefinitionOfDoneCheck()
        {
            AnsiConsole.WriteLine("Can't review definition of done yet. Assign a developer first");
        }
    }
}
