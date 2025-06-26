using AvansDevOps.Entities;
using Spectre.Console;

namespace AvansDevOps.SprintStatePattern {
    public class FinishedSprintState(Sprint sprint) : ISprintState {
        private readonly Sprint _sprint = sprint;

        public bool StartRelease() {
            AnsiConsole.WriteLine("Sprint has been finalized. No more changes can be made.");
            return false;
        }

        public void StartSprint() {
            AnsiConsole.WriteLine("Sprint has been finalized. No more changes can be made.");
        }

        public void StopSprint() {
            AnsiConsole.WriteLine("Sprint has been finalized. No more changes can be made.");
        }

        public void UploadSummary(string summary) {
            AnsiConsole.WriteLine("Sprint has been finalized. No more changes can be made.");
        }
    }
}
