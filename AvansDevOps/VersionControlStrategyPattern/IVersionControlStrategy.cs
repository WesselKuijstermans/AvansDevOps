using AvansDevOps.Entities;
using AvansDevOps.FacadePattern;

namespace AvansDevOps.VersionControlStrategyPattern {
    public interface IVersionControlStrategy {
        public bool BranchExists(VersionControlFacade facade, string branchName);
        public void SelectBranch(VersionControlFacade facade, string branchName);
        public void PullBranch(VersionControlFacade facade, string branchName);
        public void Commit(VersionControlFacade facade, string message, TeamMember teamMember);
        public void Push(VersionControlFacade facade);
    }
}
