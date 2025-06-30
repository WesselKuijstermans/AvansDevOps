using AvansDevOps.Entities;
using Spectre.Console;

namespace AvansDevOps.ItemStatePattern {
    public class TestedState(SprintItem sprintItem) : IItemState {
        private readonly SprintItem _sprintItem = sprintItem;

        public void AssignDeveloper(TeamMember teamMember) {
            AnsiConsole.WriteLine("Item already tested. No developer can be assigned.");
        }

        public void ReadyForTesting() {
            AnsiConsole.WriteLine("Tests already succeeded");
        }

        public void TestSucceeded() {
            AnsiConsole.WriteLine("Tests already succeeded");
        }

        public void TestFailed() {
            AnsiConsole.WriteLine("Tests already succeeded");
        }

        public void DefinitionOfDoneCheck() {
            List<SprintItem> subTasks = _sprintItem.GetSubtasks();
            if (subTasks.Count != 0) {
                for (int i = 0 ; i < subTasks.Count ; i++) {
                    if (subTasks[i].GetState().GetType() != typeof(DoneState)) {
                        AnsiConsole.WriteLine("Item does not meet the definition of done.");
                        AnsiConsole.WriteLine("Subtask " + subTasks[i].GetBacklogItem().GetTask() + " is not done.");
                        _sprintItem.SetState(new ReadyForTestingState(_sprintItem));
                        return;
                    }
                }
            }
            AnsiConsole.WriteLine("Item meets the definition of done.");
            _sprintItem.SetState(new DoneState());
        }
    }
}
