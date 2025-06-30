using AvansDevOps.Entities;
using AvansDevOps.PipelineStrategyPattern;
using AvansDevOps.SprintStatePattern;

namespace AvansDevOpsTest {
    public class PipelineTest {
        readonly Project project = new("TestProject");
        readonly Sprint sprint;

        public PipelineTest() {
            var startDate = DateTime.Now; // Example start date
            var endDate = startDate.AddDays(14); // Example end date (2 weeks sprint)
            Pipeline pipeline = null; // Assuming no pipeline is set initially
            NotificationService observer = new NotificationService(project);
            project.AddSprint("Sprint 1", startDate, endDate, pipeline, observer);
            this.sprint = project.GetSprints()[0];
            this.sprint.SetState(new StoppedSprintState(sprint));
        }


        [Fact]
        public void DeployPipeline_Should_Execute_Given_Steps_If_Includes_Deploy() {
            // Arrange
            sprint.SetPipeline(new DeployPipeline());
            sprint.AddStepToPipeline(new BuildStep());
            sprint.AddStepToPipeline(new DeployStep());

            // Act
            var result = sprint.StartRelease(true);

            // Assert
            Assert.True(result);
            Assert.Equal(2, sprint.GetPipelineSteps()!.Count);
        }

        [Fact]
        public void DeployPipeline_Should_Append_Deploy_If_Deploy_Step_Missing() {
            // Arrange
            sprint.SetPipeline(new DeployPipeline());
            var buildStep = new BuildStep();
            var testStep = new TestStep();
            sprint.AddStepToPipeline(buildStep);
            sprint.AddStepToPipeline(testStep);

            // Act
            var result = sprint.GetPipelineSteps();

            // Assert
            Assert.Equal(3, result!.Count);
            Assert.IsType<BuildStep>(result[0]);
            Assert.IsType<TestStep>(result[1]);
            Assert.IsType<DeployStep>(result[2]);
        }

        [Fact]
        public void TestPipeline_Should_Execute_Given_Steps_If_Includes_Test() {
            // Arrange
            sprint.SetPipeline(new TestPipeline());
            var buildStep = new BuildStep();
            var testStep = new TestStep();
            sprint.AddStepToPipeline(buildStep);
            sprint.AddStepToPipeline(testStep);

            // Act
            var result = sprint.StartRelease(true);

            // Assert
            Assert.True(result);
            Assert.Equal(2, sprint.GetPipelineSteps()!.Count);
        }

        [Fact]
        public void TestPipeline_Should_Append_Test_If_Test_Step_Missing() {
            // Arrange
            sprint.SetPipeline(new TestPipeline());
            var buildStep = new BuildStep();
            var deployStep = new DeployStep();
            sprint.AddStepToPipeline(buildStep);
            sprint.AddStepToPipeline(deployStep);

            // Act
            var result = sprint.GetPipelineSteps()!;

            // Assert
            Assert.Equal(3, result.Count);
            Assert.IsType<BuildStep>(result[0]);
            Assert.IsType<DeployStep>(result[1]);
            Assert.IsType<TestStep>(result[2]);
        }

        [Fact]
        public void Pipeline_Should_Return_False_When_No_Steps_Are_Set() {
            // Arrange
            sprint.SetPipeline(new DeployPipeline());

            // Act
            var result = sprint.StartRelease(true);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Pipeline_Should_Return_False_When_Steps_Fail() {
            // Arrange
            sprint.SetPipeline(new DeployPipeline());
            var buildStep = new BuildStep();
            sprint.AddStepToPipeline(buildStep);

            // Act
            var result = sprint.StartRelease(false);

            // Assert
            Assert.False(result);
        }
    }
}
