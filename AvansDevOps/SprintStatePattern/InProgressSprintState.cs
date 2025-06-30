using AvansDevOps.Entities;
using Spectre.Console;

namespace AvansDevOps.SprintStatePattern {
    public class InProgressSprintState(Sprint sprint) : ISprintState {
        private readonly Sprint _sprint = sprint;

        public bool StartRelease(bool result) {
            AnsiConsole.WriteLine("Sprint not over yet. Wait until the sprint has been stopped to start release");
            return false;
        }

        public void StartSprint() {
            AnsiConsole.WriteLine("Sprint already started");
        }

        public void StopSprint() {
            AnsiConsole.WriteLine("Stopping sprint");
            _sprint.SetState(new StoppedSprintState(_sprint));
        }

        public void UploadSummary(string summary) {
            AnsiConsole.WriteLine("Sprint not over yet. Wait until the sprint has been stopped to upload a summary");
        }
    }
}
