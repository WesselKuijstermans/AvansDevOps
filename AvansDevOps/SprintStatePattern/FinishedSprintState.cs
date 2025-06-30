using Spectre.Console;

namespace AvansDevOps.SprintStatePattern {
    public class FinishedSprintState() : ISprintState {
        readonly string message = "Sprint has been finalized. No more changes can be made.";

        public bool StartRelease(bool result) {
            AnsiConsole.WriteLine(message);
            return false;
        }

        public void StartSprint() {
            AnsiConsole.WriteLine(message);
        }

        public void StopSprint() {
            AnsiConsole.WriteLine(message);
        }

        public void UploadSummary(string summary) {
            AnsiConsole.WriteLine(message);
        }
    }
}
