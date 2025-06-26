using AvansDevOps.Entities;
using Spectre.Console;

namespace AvansDevOps.ItemStatePattern {
    public class DoingState(SprintItem sprintItem) : IItemState {
        private readonly SprintItem _sprintItem = sprintItem;

        public void AssignDeveloper(TeamMember teamMember) {
            AnsiConsole.WriteLine("A developer is already assigned. Assigning new developer");
            this._sprintItem.SetDeveloper(teamMember);
        }

        public void ReadyForTesting() {
            Developer? developer = _sprintItem.GetDeveloper();
            if (_sprintItem.versionControlFacade.CurrentBranch != null && developer != null) {
                string branch = _sprintItem.versionControlFacade.CurrentBranch.Name;
                AnsiConsole.WriteLine($"implementation for item '{_sprintItem.GetBacklogItem().GetTask()}' has been pushed to branch '{branch}' and is ready for testing");
                _sprintItem.SetState(new ReadyForTestingState(_sprintItem));
                _sprintItem.SetDeveloper(null);
            } else {
                AnsiConsole.WriteLine("Can't mark item as ready for testing. Either no branch selected in version control or no developer assigned");
            }
        }

        public void TestSucceeded() {
            AnsiConsole.WriteLine("Can't perform tests yet. Developer needs to mark the item as ready for testing first");
        }

        public void TestFailed() {
            AnsiConsole.WriteLine("Can't perform tests yet. Developer needs to mark the item as ready for testing first");
        }

        public void DefinitionOfDoneCheck() {
            AnsiConsole.WriteLine("Can't review definition of done yet. Developer needs to mark the item as ready for testing first");
        }
    }
}
