using AvansDevOps.Entities;
using Spectre.Console;

namespace AvansDevOps.SprintStatePattern {
    public class ConceptSprintState(Sprint sprint) : ISprintState {
        private readonly Sprint _sprint = sprint;

        public bool StartRelease(bool result) {
            AnsiConsole.WriteLine("Sprint not started yet. Wait until the sprint is done to start release");
            return false;
        }

        public void StartSprint() {
            AnsiConsole.WriteLine("Starting sprint");
            _sprint.SetState(new InProgressSprintState(_sprint));
        }

        public void StopSprint() {
            AnsiConsole.WriteLine("Cannot stop sprint, because it has not started yet");
        }

        public void UploadSummary(string summary) {
            AnsiConsole.WriteLine("Sprint not started yet. Wait until the sprint is done to upload a summary");
        }
    }
}
