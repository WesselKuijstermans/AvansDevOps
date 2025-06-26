namespace AvansDevOps.SprintStatePattern {
    public interface ISprintState {
        void StartSprint();
        void StopSprint();
        bool StartRelease();
        void UploadSummary(string summary);

    }
}
