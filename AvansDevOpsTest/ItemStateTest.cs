namespace AvansDevOpsTest

{
    using AvansDevOps.TemplatePattern;
    using AvansDevOps.Entities;
    using AvansDevOps.ItemStatePattern;
    using AvansDevOps.Adapter;

    public class ItemStateTest
    {
        private Project project;
        private Sprint sprint;
        private SprintItem item;
        private TeamMember developer;

        public ItemStateTest() 
        {
            this.project = new Project("Test Project");
            project.AddSprint(project.GetName() + "-1", DateTime.Now, DateTime.Now.AddDays(14), new TestPipeline());
            this.sprint = project.GetSprints()[0];
            this.item = new SprintItem(sprint, new BacklogItem("Test Task", 2), project);
            this.developer = new Developer("Test Developer", new EmailAdapter());
            this.project.AddTeamMember(developer);
        }
        [Fact]
        public void Todo_Should_Transition_To_Doing()
        {
            this.item.SetState(new TodoState(item));
            this.item.AssignDeveloper(this.developer);

            Assert.Equal("DoingState", this.item.GetState().GetType().Name);
        }

        [Fact]
        public void Todo_Should_Only_Transition_When_Assigning_Developer()
        {
            this.item.SetState(new TodoState(item));
            this.item.ReadyForTesting();
            this.item.TestFailed();
            this.item.TestSucceeded();
            this.item.DefinitionOfDoneCheck();

            Assert.Equal("TodoState", this.item.GetState().GetType().Name);
        }

        [Fact]
        public void Doing_Should_Transition_To_ReadyForTesting()
        {
            this.item.SetState(new DoingState(item));
            this.item.ReadyForTesting();

            Assert.Equal("ReadyForTestingState", this.item.GetState().GetType().Name);
        }

        [Fact]
        public void Doing_Should_Only_Transition_When_ReadyForTesting()
        {
            this.item.SetState(new DoingState(item));
            this.item.AssignDeveloper(this.developer);
            this.item.TestFailed();
            this.item.TestSucceeded();
            this.item.DefinitionOfDoneCheck();

            Assert.Equal("DoingState", this.item.GetState().GetType().Name);
        }

        [Fact]
        public void ReadyForTesting_Should_Transition_To_Tested_If_Tests_Succeeded()
        {
            this.item.SetState(new ReadyForTestingState(item));
            this.item.TestSucceeded();

            Assert.Equal("TestedState", this.item.GetState().GetType().Name);
        }

        [Fact]
        public void ReadyForTesting_Should_Transition_To_Todo_If_Tests_Failed()
        {
            this.item.SetState(new ReadyForTestingState(item));
            this.item.TestFailed();

            Assert.Equal("TodoState", this.item.GetState().GetType().Name);
        }

        [Fact]
        public void ReadyForTesting_Should_Only_Transition_When_Testing()
        {
            this.item.SetState(new ReadyForTestingState(item));
            this.item.AssignDeveloper(this.developer);
            this.item.ReadyForTesting();
            this.item.DefinitionOfDoneCheck();

            Assert.Equal("ReadyForTestingState", this.item.GetState().GetType().Name);
        }

        [Fact]
        public void Tested_Should_Transition_To_Done_If_DefinitionOfDone_NoSubtasks()
        {
            this.item.SetState(new TestedState(item));
            this.item.DefinitionOfDoneCheck();

            Assert.Equal("DoneState", this.item.GetState().GetType().Name);
        }

        [Fact]
        public void Tested_Should_Transition_To_Done_If_DefinitionOfDone_SubtasksDone()
        {
            this.item.SetState(new TestedState(item));
            this.item.AddSubtask(new SprintItem(sprint, new BacklogItem("Subtask", 1), project));
            this.item.GetSubtasks()[0].SetState(new DoneState(item));
            this.item.DefinitionOfDoneCheck();

            Assert.Equal("DoneState", this.item.GetState().GetType().Name);
        }


        [Fact]
        public void Tested_Should_Transition_To_ReadyForTesting_If_DefinitionOfDone_SubtasksNotDone()
        {
            this.item.SetState(new TestedState(item));
            this.item.AddSubtask(new SprintItem(sprint, new BacklogItem("Subtask", 1), project));
            this.item.GetSubtasks()[0].SetState(new DoingState(item));
            this.item.DefinitionOfDoneCheck();

            Assert.Equal("ReadyForTestingState", this.item.GetState().GetType().Name);
        }

        [Fact]
        public void Tested_Should_Only_Transition_When_DefinitionOfDone_Checked()
        {
            this.item.SetState(new TestedState(item));
            this.item.AssignDeveloper(this.developer);
            this.item.ReadyForTesting();
            this.item.TestFailed();
            this.item.TestSucceeded();

            Assert.Equal("TestedState", this.item.GetState().GetType().Name);
        }

        [Fact]
        public void Done_Should_Not_Be_Able_To_Transtition()
        {
            this.item.SetState(new DoneState(item));
            this.item.AssignDeveloper(this.developer);
            this.item.ReadyForTesting();
            this.item.TestFailed();
            this.item.TestSucceeded();
            this.item.DefinitionOfDoneCheck();

            Assert.Equal("DoneState", this.item.GetState().GetType().Name);
        }
    }
}