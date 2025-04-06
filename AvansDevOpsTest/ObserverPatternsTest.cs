using AvansDevOps.Adapter;
using AvansDevOps.Entities;
using AvansDevOps.FormMessageObersverPattern;
using AvansDevOps.ItemStatePattern;
using AvansDevOps.SprintStateObersverPattern;
using AvansDevOps.SprintStatePattern;
using AvansDevOps.StateObserverPattern;
using AvansDevOps.TemplatePattern;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOpsTest
{
    public class ObserverPatternsTest
    {
        private Project project;
        private Sprint sprint;
        private SprintItem item;
        private TeamMember developer;
        private TeamMember tester;
        private TeamMember leadDeveloper;
        private TeamMember scrumMaster;
        private Mock<IItemStateObserver> mockItemObserver;
        private Mock<ISprintStateObserver> mockSprintObserver;
        private Mock<IFormMessageObserver> mockFormObserver;

        public ObserverPatternsTest()
        {
            this.mockItemObserver = new Mock<IItemStateObserver>();
            this.mockSprintObserver = new Mock<ISprintStateObserver>();
            this.mockFormObserver = new Mock<IFormMessageObserver>();
            this.project = new Project("Test Project");
            var pipeline = new TestPipeline();
            pipeline.AddStep(new BuildStep());
            project.AddSprint(project.GetName() + "-1", DateTime.Now, DateTime.Now.AddDays(14), pipeline, mockSprintObserver.Object);
            this.sprint = project.GetSprints()[0];
            this.item = new SprintItem(sprint, new BacklogItem("Test Task", 2), mockItemObserver.Object, mockFormObserver.Object);
            this.developer = new Developer("Test Developer", new EmailAdapter());
            this.tester = new Tester("Test Tester", new EmailAdapter());
            this.leadDeveloper = new LeadDeveloper("Test Lead Developer", new SlackAdapter());
            this.scrumMaster = new ScrumMaster("Test Scrum Master", new SlackAdapter());
            project.AddTeamMember(developer);
            project.AddTeamMember(tester);
            project.AddTeamMember(leadDeveloper);
            project.AddTeamMember(scrumMaster);
        }

        [Fact]
        public void ItemStateObserver_Should_Notify_TeamMember_When_State_Stages()
        {
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
        public void SprintStateObserver_Should_Notify_TeamMember_When_State_Stages()
        {
            sprint.StartSprint();
            sprint.StopSprint();
            sprint.StartRelease();

            mockSprintObserver.Verify(o => o.SprintUpdate(It.IsAny<InProgressSprintState>(), sprint), Times.Once);
            mockSprintObserver.Verify(o => o.SprintUpdate(It.IsAny<StoppedSprintState>(), sprint), Times.Once);
            mockSprintObserver.Verify(o => o.SprintUpdate(It.IsAny<FinishedSprintState>(), sprint), Times.Once);
        }

        [Fact]
        public void FormMessageObserver_Should_Notify_TeamMember_When_State_Stages()
        {
            item.AddMessage(new FormMessage(developer, "Test"));

            // recipients is empty because no one has sent a message before this
            List<TeamMember> recipients = new List<TeamMember>();
            
            mockFormObserver.Verify(o => o.FormUpdate(It.IsAny<string>(), recipients), Times.Once);

            // now the developer will be notified of future messages from other teamMembers on this item
            recipients.Add(developer);

            item.AddMessage(new FormMessage(tester, "Test"));

            mockFormObserver.Verify(o => o.FormUpdate(It.IsAny<string>(), recipients), Times.Once);

        }
    }
}
