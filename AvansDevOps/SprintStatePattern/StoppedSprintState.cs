using AvansDevOps.Entities;
using Spectre.Console;

namespace AvansDevOps.SprintStatePattern {
    public class StoppedSprintState(Sprint sprint) : ISprintState {
        private readonly Sprint _sprint = sprint;

        public bool StartRelease() {
            if (_sprint.GetPipelineSteps() == null) {
                AnsiConsole.WriteLine("This is not a release sprint. Cannot start release.");
                return false;
            } else if (_sprint.GetPipelineSteps().Count == 0) {
                AnsiConsole.WriteLine("No steps in the pipeline. Cannot start release.");
                return false;
            }
            AnsiConsole.WriteLine("Release started.");
            if (_sprint.RunPipeline()) {
                AnsiConsole.WriteLine("Pipeline has successfully run! Sprint has been marked as finished");
                _sprint.SetState(new FinishedSprintState(_sprint));
                return true;
            }
            AnsiConsole.WriteLine("The pipeline has failed. Check for errors and try again.");
            _sprint.SetState(new StoppedSprintState(_sprint));
            return false;
        }

        public void StartSprint() {
            AnsiConsole.WriteLine("Cannot start Sprint. Sprint has already ended.");
        }

        public void StopSprint() {
            AnsiConsole.WriteLine("Cannot stop Sprint. Sprint has already ended.");
        }

        public void UploadSummary(string summary) {
            if (_sprint.GetPipelineSteps() != null) {
                AnsiConsole.WriteLine("This is a release sprint. Cannot upload summary.");
                return;
            }
            _sprint.SetSummary(summary);
            AnsiConsole.WriteLine("Summary uploaded.");
            AnsiConsole.WriteLine("Sprint has been stopped. No more changes can be made.");
            _sprint.SetState(new FinishedSprintState(_sprint));
        }
    }
}
