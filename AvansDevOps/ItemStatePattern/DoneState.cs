using AvansDevOps.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;

namespace AvansDevOps.ItemStatePattern
{
    public class DoneState(SprintItem sprintItem) : IItemState
    {
        private readonly SprintItem _sprintItem = sprintItem;

        public void AssignDeveloper(TeamMember teamMember)
        {
            AnsiConsole.WriteLine("Item is already done. No more changes can be made.");
        }

        public void ReadyForTesting()
        {
            AnsiConsole.WriteLine("Item is already done. No more changes can be made.");
        }

        public void TestSucceeded()
        {
            AnsiConsole.WriteLine("Item is already done. No more changes can be made.");
        }

        public void TestFailed()
        {
            AnsiConsole.WriteLine("Item is already done. No more changes can be made.");
        }

        public void DefinitionOfDoneCheck()
        {
            AnsiConsole.WriteLine("Item is already done. No more changes can be made.");
        }
    }
}
