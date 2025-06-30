using AvansDevOps.AdapterPattern;
using AvansDevOps.Entities;
using AvansDevOps.FacadePattern;
using AvansDevOps.ItemStatePattern;
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
            this.item.versionControlFacade = new VersionControlFacade(new GitVersionControlStrategy(), item);
            this.developer = new Developer("Test Developer", new EmailAdapter("test@gmail.com"));
            this.project.AddTeamMember(developer);
            this.item.AssignDeveloper(developer);
        }

        [Fact]
        public void VersionControlFacade_Pull_Should_Load_Branch_And_Set_As_CurrentBranch_If_Branch_Exists() {
            this.item.versionControlFacade.Pull("Main");
            Assert.Contains(this.item.versionControlFacade.getBranches(), b => b.Name == "Main");
            Assert.Equal("Main", this.item.versionControlFacade.CurrentBranch?.Name);
        }

        [Fact]
        public void VersionControlFacade_Pull_Should_Abort_If_Branch_Not_Exists() {
            this.item.versionControlFacade.Pull("Nope");
            Assert.DoesNotContain(this.item.versionControlFacade.getBranches(), b => b.Name == "Nope");
            Assert.Null(this.item.versionControlFacade.CurrentBranch);
        }

        [Fact]
        public void VersionControlFacade_Checkout_Should_Set_CurrentBranch_If_Branch_Exists() {
            this.item.versionControlFacade.AddBranch(new Branch("Feature1"));
            this.item.versionControlFacade.Checkout("Feature1");
            Assert.Contains(this.item.versionControlFacade.getBranches(), b => b.Name == "Feature1");
            Assert.Equal("Feature1", this.item.versionControlFacade.CurrentBranch?.Name);
        }

        [Fact]
        public void VersionControlFacade_Checkout_Should_Not_Set_CurrentBranch_If_Branch_Not_Exists() {
            this.item.versionControlFacade.Checkout("NonExistentBranch");
            Assert.DoesNotContain(this.item.versionControlFacade.getBranches(), b => b.Name == "NonExistentBranch");
            Assert.Null(this.item.versionControlFacade.CurrentBranch);
        }

        [Fact]
        public void VersionControlFacade_Commit_Should_Add_Commit_To_CurrentBranch() {
            this.item.versionControlFacade.Pull("Main");
            this.item.versionControlFacade.Commit("Initial commit", this.developer);
            Assert.Single(item.versionControlFacade.CurrentBranch?.GetCommits());
            var commit = item.versionControlFacade.CurrentBranch?.GetCommits()[0];
            Assert.Equal("Initial commit", commit.GetMessage());
            Assert.Equal(this.developer, commit.GetTeamMember());
        }

        [Fact]
        public void VersionControlFacade_Push_Should_Set_SprintItem_To_ReadyForTesting() {
            this.item.versionControlFacade.Pull("Main");
            this.item.versionControlFacade.Push();
            Assert.IsType<ReadyForTestingState>(this.item.GetState());
        }

        [Fact]
        public void VersionControlFacade_Push_Should_Abort_If_No_CurrentBranch() {
            this.item.versionControlFacade.Push();
            Assert.IsType<DoingState>(this.item.GetState());
        }

        [Fact]
        public void VersionControlFacade_RemoveBranch_Should_Remove_Branch_And_CurrentBranch_If_CurrentBranch() {
            this.item.versionControlFacade.Pull("Main");
            this.item.versionControlFacade.RemoveBranch("Main");
            Assert.DoesNotContain(this.item.versionControlFacade.getBranches(), b => b.Name == "Main");
            Assert.Null(this.item.versionControlFacade.CurrentBranch);
        }

        [Fact]
        public void VersionControlFacade_RemoveBranch_Should_Only_Remove_Branch_If_Not_CurrentBranch() {
            this.item.versionControlFacade.Pull("Main");
            this.item.versionControlFacade.Pull("Dev");
            this.item.versionControlFacade.RemoveBranch("Main");
            Assert.DoesNotContain(this.item.versionControlFacade.getBranches(), b => b.Name == "Main");
            Assert.Contains(this.item.versionControlFacade.getBranches(), b => b.Name == "Dev");
            Assert.Equal("Dev", this.item.versionControlFacade.CurrentBranch?.Name);
        }
    }
}
