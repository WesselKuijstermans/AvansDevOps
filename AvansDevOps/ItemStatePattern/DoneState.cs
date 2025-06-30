using AvansDevOps.Entities;
using Spectre.Console;

namespace AvansDevOps.ItemStatePattern {
    public class DoneState() : IItemState {
        readonly string message = "Item is already done. No more changes can be made.";
        public void AssignDeveloper(TeamMember teamMember) {
            AnsiConsole.WriteLine(message);
        }

        public void ReadyForTesting() {
            AnsiConsole.WriteLine(message);
        }

        public void TestSucceeded() {
            AnsiConsole.WriteLine(message);
        }

        public void TestFailed() {
            AnsiConsole.WriteLine(message);
        }

        public void DefinitionOfDoneCheck() {
            AnsiConsole.WriteLine(message);
        }
    }
}
