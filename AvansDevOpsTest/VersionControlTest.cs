using AvansDevOps.Entities;
using AvansDevOps.FacadePattern;
using AvansDevOps.ItemStatePattern;
using AvansDevOps.NotificationAdapterPattern;
using AvansDevOps.PipelineStrategyPattern;
using AvansDevOps.VersionControlStrategyPattern;

namespace AvansDevOpsTest {
    public class VersionControlTest {
        private readonly Project project;
        private readonly Sprint sprint;
        private readonly SprintItem item;
        private readonly TeamMember developer;
        private readonly NotificationService notificationService;
        private readonly VersionControlFacade versionControlFacade;

        public VersionControlTest() {
            this.project = new Project("Test Project");
            this.notificationService = new(project);
            project.AddSprint(project.GetName() + "-1", DateTime.Now, DateTime.Now.AddDays(14), new TestPipeline(), notificationService);
            this.sprint = project.GetSprints()[0];
            this.item = new SprintItem(new BacklogItem("Test Task", 2), notificationService, notificationService, null);
            this.versionControlFacade = new VersionControlFacade(new GitVersionControlStrategy(), item);
            this.item.versionControlFacade = this.versionControlFacade;
            this.developer = new Developer("Test Developer", new EmailAdapter("test@gmail.com"));
            this.project.AddTeamMember(developer);
            this.item.AssignDeveloper(developer);
        }

        [Fact]
        public void VersionControlFacade_Pull_Should_Load_Branch_And_Set_As_CurrentBranch_If_Branch_Exists() {
            item.versionControlFacade!.Pull("Main");
            Assert.Contains(item.versionControlFacade.getBranches(), b => b.Name == "Main");
            Assert.Equal("Main", item.versionControlFacade.CurrentBranch?.Name);
        }

        [Fact]
        public void VersionControlFacade_Pull_Should_Abort_If_Branch_Not_Exists() {
            item.versionControlFacade!.Pull("Nope");
            Assert.DoesNotContain(item.versionControlFacade.getBranches(), b => b.Name == "Nope");
            Assert.Null(item.versionControlFacade.CurrentBranch);
        }

        [Fact]
        public void VersionControlFacade_Checkout_Should_Set_CurrentBranch_If_Branch_Exists() {
            item.versionControlFacade!.AddBranch(new Branch("Feature1"));
            item.versionControlFacade.Checkout("Feature1");
            Assert.Contains(item.versionControlFacade.getBranches(), b => b.Name == "Feature1");
            Assert.Equal("Feature1", item.versionControlFacade.CurrentBranch?.Name);
        }

        [Fact]
        public void VersionControlFacade_Checkout_Should_Not_Set_CurrentBranch_If_Branch_Not_Exists() {
            item.versionControlFacade!.Checkout("NonExistentBranch");
            Assert.DoesNotContain(item.versionControlFacade.getBranches(), b => b.Name == "NonExistentBranch");
            Assert.Null(item.versionControlFacade.CurrentBranch);
        }

        [Fact]
        public void VersionControlFacade_Commit_Should_Add_Commit_To_CurrentBranch() {
            item.versionControlFacade!.Pull("Main");
            item.versionControlFacade.Commit("Initial commit", developer);
            Assert.Equal(1, item.versionControlFacade.CurrentBranch?.GetCommits().Count);
            var commit = item.versionControlFacade.CurrentBranch?.GetCommits()[0];
            Assert.NotNull(commit);
            Assert.Equal("Initial commit", commit.GetMessage());
            Assert.Equal(developer, commit.GetTeamMember());
        }

        [Fact]
        public void VersionControlFacade_Push_Should_Set_SprintItem_To_ReadyForTesting() {
            item.versionControlFacade!.Pull("Main");
            item.versionControlFacade.Push();
            Assert.IsType<ReadyForTestingState>(item.GetState());
        }

        [Fact]
        public void VersionControlFacade_Push_Should_Abort_If_No_CurrentBranch() {
            item.versionControlFacade!.Push();
            Assert.IsType<DoingState>(item.GetState());
        }

        [Fact]
        public void VersionControlFacade_RemoveBranch_Should_Remove_Branch_And_CurrentBranch_If_CurrentBranch() {
            item.versionControlFacade!.Pull("Main");
            item.versionControlFacade.RemoveBranch("Main");
            Assert.DoesNotContain(item.versionControlFacade.getBranches(), b => b.Name == "Main");
            Assert.Null(item.versionControlFacade.CurrentBranch);
        }

        [Fact]
        public void VersionControlFacade_RemoveBranch_Should_Only_Remove_Branch_If_Not_CurrentBranch() {
            item.versionControlFacade!.Pull("Main");
            item.versionControlFacade.Pull("Dev");
            item.versionControlFacade.RemoveBranch("Main");
            Assert.DoesNotContain(item.versionControlFacade.getBranches(), b => b.Name == "Main");
            Assert.Contains(item.versionControlFacade.getBranches(), b => b.Name == "Dev");
            Assert.Equal("Dev", item.versionControlFacade.CurrentBranch?.Name);
        }
    }
}
