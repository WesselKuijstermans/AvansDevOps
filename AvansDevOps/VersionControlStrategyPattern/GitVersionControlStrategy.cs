using AvansDevOps.Entities;
using AvansDevOps.FacadePattern;
using Spectre.Console;

namespace AvansDevOps.VersionControlStrategyPattern {
    public class GitVersionControlStrategy : IVersionControlStrategy {

        public bool BranchExists(VersionControlFacade facade, string branchName) {
            // Deterministically generate a seed from the branchName using ASCII values
            int seed = 0;
            foreach (char c in branchName.ToLowerInvariant()) {
                seed = (seed * 31) + c;
            }
            var random = new Random(seed);
            bool result = random.Next(0, 2) == 0; // 50% chance, deterministic for each branchName
            if (result) {
                AnsiConsole.WriteLine($"Branch '{branchName}' found.");
            } else {
                AnsiConsole.WriteLine($"Branch '{branchName}' does not exist.");
            }
            return result;
        }

        public void Commit(VersionControlFacade facade, string message, TeamMember teamMember) {
            if (facade.CurrentBranch == null) {
                AnsiConsole.WriteLine("No branch selected. Please select a branch before committing.");
            } else {
                AnsiConsole.WriteLine($"Committing changes to branch '{facade.CurrentBranch.Name}' with message: '{message}' by {teamMember.GetName()}.");
                facade.CurrentBranch.AddCommit(new Commit(message, teamMember));
            }
        }

        public void PullBranch(VersionControlFacade facade, string branchName) {
            Branch newBranch = new(branchName);
            AnsiConsole.WriteLine($"Pulling branch '{branchName}'...");
            facade.AddBranch(newBranch);
        }

        public void SelectBranch(VersionControlFacade facade, string branchName) {
            Branch? branchToSelect = facade.getBranches().FirstOrDefault(b => b.Name.Equals(branchName, StringComparison.OrdinalIgnoreCase));
            if (branchToSelect != null) {
                AnsiConsole.WriteLine($"Switching to branch '{branchName}'...");
                facade.CurrentBranch = branchToSelect;
            } else {
                AnsiConsole.WriteLine($"Branch '{branchName}' does not exist.");
            }
        }

        public void Push(VersionControlFacade facade) {
            if (facade.CurrentBranch != null) {
                AnsiConsole.WriteLine($"Pushing changes to branch '{facade.CurrentBranch.Name}'...");
                facade.SprintItem!.ReadyForTesting();
            } else {
                AnsiConsole.WriteLine("No branch selected. Please select a branch before pushing.");
            }
        }
    }
}
