using AvansDevOps.AdapterPattern;
using AvansDevOps.Entities;
using AvansDevOps.SprintStatePattern;
using AvansDevOps.PipelineStrategyPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOpsTest
{
    public class SprintStateTest
    {
        private readonly Project project;
        private readonly NotificationService notificationService;
        private readonly Sprint sprintWithPipeline;
        private readonly Sprint sprint;
        private readonly TeamMember developer;

        public SprintStateTest()
        {
            this.project = new Project("TestProject");
            this.notificationService = new(project);
            var startDate = DateTime.Now;
            var endDate = startDate.AddDays(10);
            this.project.AddSprint("TestSprint", startDate, endDate, null, notificationService);
            this.sprint = project.GetSprints()[0];
            var pipeline = new TestPipeline();
            pipeline.AddStep(new BuildStep());
            this.project.AddSprint("TestSprintWithPipeline", startDate, endDate, pipeline, notificationService);
            this.sprintWithPipeline = project.GetSprints()[1];
            this.developer = new Developer("TestUser", new SlackAdapter("test-slack-token"));
            this.project.AddTeamMember(developer);
        }

        [Fact]
        public void ConceptSprintState_Should_Transition_To_InProgressSprintState()
        {
            sprint.SetState(new ConceptSprintState(sprint));
            sprint.StartSprint();
            Assert.IsType<InProgressSprintState>(sprint.GetState());
        }

        [Fact]
        public void InProgressSprintState_Should_Transition_To_StoppedSprintState()
        {
            sprint.SetState(new InProgressSprintState(sprint));
            sprint.StopSprint();
            Assert.IsType<StoppedSprintState>(sprint.GetState());
        }

        [Fact]
        public void StoppedSprintState_Without_Pipeline_Should_Transition_To_FinishedSprintState_When_Uploading_Summary()
        {
            sprint.SetState(new StoppedSprintState(sprint));
            sprint.UploadSummary("Test");
            Assert.IsType<FinishedSprintState>(sprint.GetState());
            Assert.Equal("Test", sprint.GetSummary());
        }

        [Fact]
        public void StoppedSprintState_With_Pipeline_Should_Transition_To_FinishedSprintState_When_Starting_Release()
        {
            sprintWithPipeline.SetState(new StoppedSprintState(sprintWithPipeline));
            sprintWithPipeline.StartRelease();
            Assert.IsType<FinishedSprintState>(sprintWithPipeline.GetState());
        }

        [Fact]
        public void FinishedSprintState_Should_Not_Allow_Any_Transitions()
        {
            sprint.SetState(new FinishedSprintState(sprint));
            sprint.StartSprint();
            sprint.StopSprint();
            sprint.UploadSummary("Test");
            sprint.StartRelease();
            Assert.IsType<FinishedSprintState>(sprint.GetState());
        }

        [Fact]
        public void ConceptSprintState_Should_Not_Allow_Release_Summary_Or_Stop()
        {
            sprint.SetState(new ConceptSprintState(sprint));
            sprint.StartRelease();
            sprint.UploadSummary("Test");
            sprint.StopSprint();
            Assert.IsType<ConceptSprintState>(sprint.GetState());
        }

        [Fact]
        public void InProgressSprintState_Should_Not_Allow_Release_Summary_Or_Start()
        {
            sprint.SetState(new InProgressSprintState(sprint));
            sprint.StartRelease();
            sprint.UploadSummary("Test");
            sprint.StartSprint();
            Assert.IsType<InProgressSprintState>(sprint.GetState());
        }

        [Fact]
        public void StoppedSprintState_With_Pipeline_Should_Not_Allow_Start_Stop_Or_UploadSummary()
        {
            sprintWithPipeline.SetState(new StoppedSprintState(sprintWithPipeline));
            sprintWithPipeline.StartSprint();
            sprintWithPipeline.StopSprint();
            sprintWithPipeline.UploadSummary("Test");
            Assert.IsType<StoppedSprintState>(sprintWithPipeline.GetState());
        }

        [Fact]
        public void StoppedSprintState_Without_Pipeline_Should_Not_Allow_Start_Stop_Or_StartRelease()
        {
            sprint.SetState(new StoppedSprintState(sprint));
            sprint.StartSprint();
            sprint.StopSprint();
            sprint.StartRelease();
            Assert.IsType<StoppedSprintState>(sprint.GetState());
        }
    }
}
