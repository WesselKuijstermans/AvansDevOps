namespace AvansDevOps.SprintStatePattern {
    public interface ISprintState {
        void StartSprint();
        void StopSprint();
        bool StartRelease(bool result);
        void UploadSummary(string summary);

    }
}
