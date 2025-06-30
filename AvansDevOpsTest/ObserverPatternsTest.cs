using AvansDevOps.Entities;
using AvansDevOps.FacadePattern;
using AvansDevOps.FormMessageObersverPattern;
using AvansDevOps.ItemStateObserverPattern;
using AvansDevOps.ItemStatePattern;
using AvansDevOps.NotificationAdapterPattern;
using AvansDevOps.PipelineStrategyPattern;
using AvansDevOps.SprintStateObersverPattern;
using AvansDevOps.SprintStatePattern;
using AvansDevOps.VersionControlStrategyPattern;
using Moq;


namespace AvansDevOpsTest {
    public class ObserverPatternsTest {
        private readonly Project project;
        private readonly Sprint sprint;
        private readonly SprintItem item;
        private readonly TeamMember developer;
        private readonly TeamMember tester;
        private readonly TeamMember leadDeveloper;
        private readonly TeamMember scrumMaster;
        private readonly Mock<IItemStateObserver> mockItemObserver;
        private readonly Mock<ISprintStateObserver> mockSprintObserver;
        private readonly Mock<IFormMessageObserver> mockFormObserver;

        public ObserverPatternsTest() {
            this.mockItemObserver = new Mock<IItemStateObserver>();
            this.mockSprintObserver = new Mock<ISprintStateObserver>();
            this.mockFormObserver = new Mock<IFormMessageObserver>();
            this.project = new Project("Test Project");
            var pipeline = new TestPipeline();
            pipeline.AddStep(new BuildStep());
            project.AddSprint(project.GetName() + "-1", DateTime.Now, DateTime.Now.AddDays(14), pipeline, mockSprintObserver.Object);
            this.sprint = project.GetSprints()[0];
            this.item = new SprintItem(new BacklogItem("Test Task", 2), mockItemObserver.Object, mockFormObserver.Object, null);
            this.item.versionControlFacade = new VersionControlFacade(new GitVersionControlStrategy(), item);
            this.item.versionControlFacade.Pull("Main");
            this.developer = new Developer("Test Developer", new EmailAdapter("test@gmail.com"));
            this.tester = new Tester("Test Tester", new EmailAdapter("test@gmail.com"));
            this.leadDeveloper = new LeadDeveloper("Test Lead Developer", new SlackAdapter("test-slack-token"));
            this.scrumMaster = new ScrumMaster("Test Scrum Master", new SlackAdapter("test-slack-token"));
            project.AddTeamMember(developer);
            project.AddTeamMember(tester);
            project.AddTeamMember(leadDeveloper);
            project.AddTeamMember(scrumMaster);
        }

        [Fact]
        public void ItemStateObserver_Should_Notify_TeamMember_When_State_Stages() {
            item.AssignDeveloper(developer);
            item.ReadyForTesting();
            item.TestFailed();
            item.AssignDeveloper(developer);
            item.ReadyForTesting();
            item.TestSucceeded();
            item.DefinitionOfDoneCheck();


            mockItemObserver.Verify(o => o.ItemUpdate(It.IsAny<TodoState>(), item), Times.Once);
            mockItemObserver.Verify(o => o.ItemUpdate(It.IsAny<DoingState>(), item), Times.Exactly(2));
            mockItemObserver.Verify(o => o.ItemUpdate(It.IsAny<ReadyForTestingState>(), item), Times.Exactly(2));
            mockItemObserver.Verify(o => o.ItemUpdate(It.IsAny<TestedState>(), item), Times.Once);
            mockItemObserver.Verify(o => o.ItemUpdate(It.IsAny<DoneState>(), item), Times.Once);
        }

        [Fact]
        public void SprintStateObserver_Should_Notify_TeamMember_When_State_Stages() {
            sprint.StartSprint();
            sprint.StopSprint();
            sprint.StartRelease();

            mockSprintObserver.Verify(o => o.SprintUpdate(It.IsAny<InProgressSprintState>(), sprint), Times.Once);
            mockSprintObserver.Verify(o => o.SprintUpdate(It.IsAny<StoppedSprintState>(), sprint), Times.Once);
            mockSprintObserver.Verify(o => o.SprintUpdate(It.IsAny<FinishedSprintState>(), sprint), Times.Once);
        }

        [Fact]
        public void FormMessageObserver_Should_Notify_TeamMember_When_State_Stages() {
            item.AddMessage(new FormMessage(developer, "Test"));

            // recipients is empty because no one has sent a message before this
            List<TeamMember> recipients = [];

            mockFormObserver.Verify(o => o.FormUpdate(It.IsAny<string>(), recipients), Times.Once);

            // now the developer will be notified of future messages from other teamMembers on this item
            recipients.Add(developer);

            item.AddMessage(new FormMessage(tester, "Test"));

            mockFormObserver.Verify(o => o.FormUpdate(It.IsAny<string>(), recipients), Times.Once);

            // now the tester is added too
            recipients.Add(tester);

            item.AddMessage(new FormMessage(scrumMaster, "Test"));

            mockFormObserver.Verify(o => o.FormUpdate(It.IsAny<string>(), recipients), Times.Once);
        }

        [Fact]
        public void FormMessageObserver_Should_Not_Notify_Message_Sender() {
            item.AddMessage(new FormMessage(developer, "Test"));

            // recipients is empty because no one has sent a message before this
            List<TeamMember> recipients = [];

            mockFormObserver.Verify(o => o.FormUpdate(It.IsAny<string>(), recipients), Times.Once);

            // developer should not receive notification for their own message
            item.AddMessage(new FormMessage(developer, "Test"));

            mockFormObserver.Verify(o => o.FormUpdate(It.IsAny<string>(), recipients), Times.Exactly(2));
        }
    }
}
