using AvansDevOps.Entities;
using AvansDevOps.VersionControlStrategyPattern;
using Spectre.Console;

namespace AvansDevOps.FacadePattern {
    public class VersionControlFacade(IVersionControlStrategy strategy, SprintItem sprintItem) {
        private readonly IVersionControlStrategy Strategy = strategy;
        public SprintItem SprintItem { get; } = sprintItem;
        private readonly List<Branch> Branches = [];
        public Branch? CurrentBranch { get; set; } = null;

        public void AddBranch(Branch branch) {
            Branches.Add(branch);
        }

        public void RemoveBranch(string branchName) {
            Branch? branchToRemove = Branches.FirstOrDefault(b => b.Name == branchName);
            if (branchToRemove != null) {
                if (CurrentBranch == branchToRemove) {
                    CurrentBranch = null; // Clear current branch if it's being removed
                }
                Branches.Remove(branchToRemove);
            } else {
                AnsiConsole.WriteLine($"Branch '{branchName}' does not exist.");
            }
        }

        public List<Branch> getBranches() {
            return Branches;
        }

        public IVersionControlStrategy GetStrategy() {
            return Strategy;
        }

        public void Pull(string branchName) {
            if (Strategy.BranchExists(this, branchName)) {
                Strategy.PullBranch(this, branchName);
                Strategy.SelectBranch(this, branchName);
            } else {
                AnsiConsole.WriteLine($"Branch '{branchName}' does not exist.");
            }
        }

        public void Checkout(string branchName) {
            Strategy.SelectBranch(this, branchName);
        }

        public void Commit(string message, TeamMember teamMember) {
            Strategy.Commit(this, message, teamMember);
        }

        public void Push() {
            Strategy.Push(this);
        }
    }
}
