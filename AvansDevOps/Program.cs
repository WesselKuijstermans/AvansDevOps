using System.CommandLine;
using System.Diagnostics.CodeAnalysis;
using AvansDevOps.Entities;
using AvansDevOps.Enums;
using AvansDevOps.FactoryPattern;
using AvansDevOps.ItemStatePattern;
using AvansDevOps.NotificationAdapterPattern;
using AvansDevOps.PipelineStrategyPattern;
using AvansDevOps.SprintStateObersverPattern;
using AvansDevOps.VersionControlStrategyPattern;
using Spectre.Console;

namespace AvansDevOps;

[ExcludeFromCodeCoverage]
static class Program {
    static async Task Main() {
        DeveloperFactory developerFactory = new();
        TesterFactory testerFactory = new();
        LeadDeveloperFactory leadDeveloperFactory = new();
        ScrumMasterFactory scrumMasterFactory = new();

        Project initialProject = new("Initial Project");
        BacklogItem initialBacklogItem = new("Initial Backlog Item", 5);
        initialProject.AddBacklogItem(new BacklogItem("Initial Backlog Item", 5));
        initialProject.AddSprint("Sprint 1", DateTime.Now, DateTime.Now.AddDays(14), new DeployPipeline(), new NotificationService(initialProject));
        Sprint initialSprint = initialProject.GetSprints()[0];
        initialSprint.AddSprintItem(initialBacklogItem, new NotificationService(initialProject), new NotificationService(initialProject), new GitVersionControlStrategy());
        TeamMember initialTeamMember = developerFactory.CreateTeamMember("Wessel", new EmailAdapter("wessel@test.com"));

        List<Project> projects = [initialProject];
        List<TeamMember> teamMembers = [initialTeamMember];

        initialProject.AddTeamMember(initialTeamMember);

        var rootCommand = new RootCommand("AvansDevOps CLI");

        string noProjectsResponse = "[red]No projects available. Please create a project first.[/]";
        string noSprintsRespone = "[red]No sprints available in this project.[/]";
        string stateSuffix = "State";


        var createTeamMemberCommand = new Command("create-team-member", "Add a team member to a project");
        createTeamMemberCommand.SetAction((context) => {
            string name = ConsoleInputHelper.UserInput("Enter the team member's name");
            if (name == string.Empty)
                return;
            TeamMemberType? teamMemberType = ConsoleInputHelper.EnumSelection<TeamMemberType>("Enter the team member type");
            if (teamMemberType == null)
                return;
            NotificationAdapterType? notificationAdapterType = ConsoleInputHelper.EnumSelection<NotificationAdapterType>("Enter the notification adapter type");
            if (notificationAdapterType == null)
                return;
            var contactInfoPrompt = notificationAdapterType switch {
                NotificationAdapterType.Email => "Enter the email address",
                NotificationAdapterType.SMS => "Enter the phone number",
                NotificationAdapterType.Slack => "Enter the Slack token",
                _ => throw new ArgumentException("Invalid notification adapter type")
            };
            string contactInfo = ConsoleInputHelper.UserInput(contactInfoPrompt);
            if (contactInfo == string.Empty)
                return;


            INotificationAdapter notificationAdapter = notificationAdapterType switch {
                NotificationAdapterType.Email => new EmailAdapter(contactInfo),
                NotificationAdapterType.SMS => new SmsAdapter(contactInfo),
                NotificationAdapterType.Slack => new SlackAdapter(contactInfo),
                _ => throw new ArgumentException("Invalid notification adapter type")
            };

            TeamMember newMember = teamMemberType switch {
                TeamMemberType.Developer => developerFactory.CreateTeamMember(name, notificationAdapter),
                TeamMemberType.Tester => testerFactory.CreateTeamMember(name, notificationAdapter),
                TeamMemberType.LeadDeveloper => leadDeveloperFactory.CreateTeamMember(name, notificationAdapter),
                TeamMemberType.ScrumMaster => scrumMasterFactory.CreateTeamMember(name, notificationAdapter),
                _ => throw new ArgumentException("Invalid team member type")
            };

            teamMembers.Add(newMember);
            AnsiConsole.MarkupLine($"[green]Added {teamMemberType}:[/] {newMember.GetName()} [green]with contact info[/]: {newMember.getContactInfo()}");
        });

        var quit = new Command("quit", "Exit the application");
        quit.SetAction(context => {
            AnsiConsole.MarkupLine("[green]Exiting the application.[/]");
            Environment.Exit(0);
        });

        var createProjectCommand = new Command("create-project", "Create a new project");
        createProjectCommand.SetAction((context) => {
            // Ask for a unique project name
            string projectName;
            do {
                projectName = ConsoleInputHelper.UserInput("Enter the project name (must be unique)");
                if (projects.Any(p => p.GetName() == projectName)) {
                    AnsiConsole.MarkupLine("[red]Project name must be unique. Please try again.[/]");
                }
            } while (projects.Any(p => p.GetName() == projectName));
            if (string.IsNullOrWhiteSpace(projectName))
                return;
            Project newProject = new(projectName);
            projects.Add(newProject);
            AnsiConsole.MarkupLine($"[green]Created project:[/] {newProject.GetName()}");
        });

        var listProjectsCommand = new Command("list-projects", "List all projects");
        listProjectsCommand.SetAction((context) => {
            if (projects.Count == 0) {
                AnsiConsole.MarkupLine(noProjectsResponse);
                return;
            }
            var table = new Table();
            table.AddColumn("Project Name");
            table.AddColumn("Team Members");
            table.AddColumn("Sprints");
            table.AddColumn("Backlog Items");
            foreach (var project in projects) {
                var teamMembersInProject = project.GetTeamMembers();
                string memberNames = string.Join("\n", teamMembersInProject);
                string sprints = string.Join("\n", project.GetSprints());
                string backlogItems = string.Join("\n", project.GetBacklogItems());
                table.AddRow(project.GetName(), memberNames, sprints, backlogItems);
            }
            AnsiConsole.Write(table);
        });

        var listTeamMembersCommand = new Command("list-team-members", "List all team members");
        listTeamMembersCommand.SetAction((context) => {
            if (teamMembers.Count == 0) {
                AnsiConsole.MarkupLine("[yellow]No team members available.[/]");
                return;
            }
            var table = new Table();
            table.AddColumn("Name");
            table.AddColumn("Type");
            table.AddColumn("Contact Info");
            foreach (var member in teamMembers) {
                table.AddRow(member.GetName(), member.GetType().Name, member.getContactInfo());
            }
            AnsiConsole.Write(table);
        });

        var addTeamMemberToProjectCommand = new Command("add-team-member", "Add a team member to a project");
        addTeamMemberToProjectCommand.SetAction((context) => {
            if (projects.Count == 0) {
                AnsiConsole.MarkupLine(noProjectsResponse);
                return;
            }
            if (teamMembers.Count == 0) {
                AnsiConsole.MarkupLine("[red]No team members available. Please create a team member first.[/]");
                return;
            }
            Project? project = ConsoleInputHelper.ListSelection<Project>("Select a project to add a team member to:", projects);
            if (project == null)
                return; // User cancelled selection
            TeamMember? teamMember = ConsoleInputHelper.ListSelection<TeamMember>("Select a team member to add to the project", teamMembers);
            if (teamMember == null)
                return; // User cancelled selection
            if (project.GetTeamMembers().Contains(teamMember)) {
                AnsiConsole.MarkupLine("[red]This team member is already part of the project.[/]");
                return;
            }
            project.AddTeamMember(teamMember);
        });

        var createSprintCommand = new Command("add-sprint", "Add a sprint to a project");
        createSprintCommand.SetAction((context) => {
            if (projects.Count == 0) {
                AnsiConsole.MarkupLine(noProjectsResponse);
                return;
            }
            Project? project = ConsoleInputHelper.ListSelection<Project>("Select a project to create a sprint for:", projects);
            if (project == null)
                return; // User cancelled selection

            string sprintName = ConsoleInputHelper.UserInput("Enter the sprint name");
            if (sprintName == string.Empty)
                return; // User cancelled input

            DateTime startDate = AnsiConsole.Ask<DateTime>("Enter the [blue]start date[/] of the sprint (YYYY-MM-DD):");
            DateTime endDate = AnsiConsole.Ask<DateTime>("Enter the [blue]end date[/] of the sprint (YYYY-MM-DD):");

            PipelineType? pipelineType = ConsoleInputHelper.EnumSelection<PipelineType>("Select the pipeline type for the sprint");
            if (pipelineType == null)
                return; // User cancelled selection
            Pipeline pipeline = pipelineType switch {
                PipelineType.Deploy => new DeployPipeline(),
                PipelineType.Test => new TestPipeline(),
                _ => throw new ArgumentException("Invalid pipeline type")
            };
            ISprintStateObserver observer = new NotificationService(project);
            project.AddSprint(sprintName, startDate, endDate, pipeline, observer);
            AnsiConsole.MarkupLine($"[green]Created sprint:[/] {sprintName} for project {project.GetName()}");
        });

        var addBacklogItemCommand = new Command("add-backlog-item", "Add a backlog item to a project");
        addBacklogItemCommand.SetAction((context) => {
            if (projects.Count == 0) {
                AnsiConsole.MarkupLine(noProjectsResponse);
                return;
            }
            Project? project = ConsoleInputHelper.ListSelection<Project>("Select a project to add a backlog item to:", projects);
            if (project == null)
                return; // User cancelled selection
            string itemName = ConsoleInputHelper.UserInput("Enter the backlog item name");
            if (itemName == string.Empty)
                return; // User cancelled input
            int storyPoints = ConsoleInputHelper.IntUserInput("How many story points?");
            if (storyPoints < 0) {
                AnsiConsole.MarkupLine("[red]Cancelling command...[/]");
                return;
            }
            BacklogItem newItem = new(itemName, storyPoints);
            project.AddBacklogItem(newItem);
            AnsiConsole.MarkupLine($"[green]Added backlog item:[/] {newItem.GetTask()} to project {project.GetName()}");
        });

        var deleteProjectCommand = new Command("delete-project", "Delete a project");
        deleteProjectCommand.SetAction((context) => {
            if (projects.Count == 0) {
                AnsiConsole.MarkupLine(noProjectsResponse);
                return;
            }
            Project? project = ConsoleInputHelper.ListSelection<Project>("Select a project to delete:", projects);
            if (project == null)
                return; // User cancelled selection
            projects.Remove(project);
            AnsiConsole.MarkupLine($"[green]Deleted project:[/] {project.GetName()}");
        });

        var deleteBacklogItemCommand = new Command("delete-backlog-item", "Delete a backlog item from a project");
        deleteBacklogItemCommand.SetAction((context) => {
            if (projects.Count == 0) {
                AnsiConsole.MarkupLine(noProjectsResponse);
                return;
            }
            Project? project = ConsoleInputHelper.ListSelection<Project>("Select a project to delete a backlog item from:", projects);
            if (project == null)
                return; // User cancelled selection
            if (project.GetBacklogItems().Count == 0) {
                AnsiConsole.MarkupLine("[red]No backlog items available in this project.[/]");
                return;
            }
            BacklogItem? item = ConsoleInputHelper.ListSelection<BacklogItem>("Select a backlog item to delete:", project.GetBacklogItems());
            if (item == null)
                return; // User cancelled selection
            project.RemoveBacklogItem(item);
            AnsiConsole.MarkupLine($"[green]Deleted backlog item:[/] {item.GetTask()} from project {project.GetName()}");
        });

        var addSprintItemCommand = new Command("add-item-to-sprint", "Add an item to a sprint");
        addSprintItemCommand.SetAction((context) => {
            if (projects.Count == 0) {
                AnsiConsole.MarkupLine(noProjectsResponse);
                return;
            }
            Project? project = ConsoleInputHelper.ListSelection<Project>("Select a project to add a sprint item to:", projects);
            if (project == null)
                return; // User cancelled selection
            if (project.GetSprints().Count == 0) {
                AnsiConsole.MarkupLine(noSprintsRespone);
                return;
            }
            Sprint? sprint = ConsoleInputHelper.ListSelection<Sprint>("Select a sprint to add an item to:", project.GetSprints());
            if (sprint == null)
                return; // User cancelled selection
            BacklogItem? item = ConsoleInputHelper.ListSelection<BacklogItem>("Select a backlog item to add to the sprint:", project.GetBacklogItems());
            if (item == null)
                return; // User cancelled selection
            VersionControlType? versionControlType = ConsoleInputHelper.EnumSelection<VersionControlType>("Select the version control type for the sprint item");
            if (versionControlType == null)
                return; // User cancelled selection
            IVersionControlStrategy versionControlStrategy = versionControlType switch {
                VersionControlType.Git => new GitVersionControlStrategy(),
                //VersionControlType.SVN => new SVNVersionControlStategy(),
                _ => throw new ArgumentException("Invalid version control type")
            };

            var observer = new NotificationService(project);

            if (sprint.GetSprintBacklog().Any(si => si.GetBacklogItem() == item)) {
                AnsiConsole.MarkupLine("[red]This backlog item is already in the sprint.[/]");
                return;
            }

            sprint.AddSprintItem(item, observer, observer, versionControlStrategy);
            AnsiConsole.MarkupLine($"[green]Added backlog item:[/] {item.GetTask()} to sprint {sprint.GetName()} in project {project.GetName()}");
        });

        var deleteSprintItemCommand = new Command("delete-sprint-item", "Delete an item from a sprint");
        deleteSprintItemCommand.SetAction((context) => {
            var (project, sprint, item) = ConsoleInputHelper.GetSprintItem(projects);
            if (project == null || sprint == null || item == null) {
                return;
            }
            sprint.RemoveSprintItem(item);
            AnsiConsole.MarkupLine($"[green]Deleted sprint item:[/] {item.GetBacklogItem().GetTask()} from sprint {sprint.GetName()} in project {project.GetName()}");
        });

        var startSprintCommand = new Command("start-sprint", "Start a sprint");
        startSprintCommand.SetAction((context) => {
            if (projects.Count == 0) {
                AnsiConsole.MarkupLine(noProjectsResponse);
                return;
            }
            Project? project = ConsoleInputHelper.ListSelection<Project>("Select a project to start a sprint in:", projects);
            if (project == null)
                return; // User cancelled selection
            if (project.GetSprints().Count == 0) {
                AnsiConsole.MarkupLine(noSprintsRespone);
                return;
            }
            Sprint? sprint = ConsoleInputHelper.ListSelection<Sprint>("Select a sprint to start:", project.GetSprints());
            if (sprint == null)
                return; // User cancelled selection
            sprint.StartSprint();
            AnsiConsole.MarkupLine($"[green]Started sprint:[/] {sprint.GetName()} in project {project.GetName()}");
        });

        var stopSprintCommand = new Command("stop-sprint", "Stop a sprint");
        stopSprintCommand.SetAction((context) => {
            if (projects.Count == 0) {
                AnsiConsole.MarkupLine(noProjectsResponse);
                return;
            }
            Project? project = ConsoleInputHelper.ListSelection<Project>("Select a project to stop a sprint in:", projects);
            if (project == null)
                return; // User cancelled selection
            if (project.GetSprints().Count == 0) {
                AnsiConsole.MarkupLine(noSprintsRespone);
                return;
            }
            Sprint? sprint = ConsoleInputHelper.ListSelection<Sprint>("Select a sprint to stop:", project.GetSprints());
            if (sprint == null)
                return; // User cancelled selection
            sprint.StopSprint();
            AnsiConsole.MarkupLine($"[green]Stopped sprint:[/] {sprint.GetName()} in project {project.GetName()}");
        });

        var addPipelineStepCommand = new Command("add-pipeline-step", "Add a step to a sprint's pipeline");
        addPipelineStepCommand.SetAction((context) => {
            if (projects.Count == 0) {
                AnsiConsole.MarkupLine(noProjectsResponse);
                return;
            }
            Project? project = ConsoleInputHelper.ListSelection<Project>("Select a project to add a pipeline step to:", projects);
            if (project == null)
                return; // User cancelled selection
            if (project.GetSprints().Count == 0) {
                AnsiConsole.MarkupLine(noSprintsRespone);
                return;
            }
            Sprint? sprint = ConsoleInputHelper.ListSelection<Sprint>("Select a sprint to add a pipeline step to:", project.GetSprints());
            if (sprint == null)
                return; // User cancelled selection
            PipelineStepType? pipelineType = ConsoleInputHelper.EnumSelection<PipelineStepType>("Select the type of pipeline step");
            if (pipelineType == null)
                return; // User cancelled selection
            IPipelineStep step = pipelineType switch {
                PipelineStepType.Analyse => new AnalyseStep(),
                PipelineStepType.Build => new BuildStep(),
                PipelineStepType.Deploy => new DeployStep(),
                PipelineStepType.Package => new PackageStep(),
                PipelineStepType.Source => new SourceStep(),
                PipelineStepType.Test => new TestStep(),
                PipelineStepType.Utility => new UtilityStep(),
                _ => throw new ArgumentException("Invalid pipeline step type")
            };
            sprint.AddStepToPipeline(step);
            AnsiConsole.MarkupLine($"[green]Added step:[/] {step.GetType().Name} to sprint {sprint.GetName()} in project {project.GetName()}");
        });

        var removePipelineStepCommand = new Command("remove-pipeline-step", "Remove a step from a sprint's pipeline");
        removePipelineStepCommand.SetAction((context) => {
            if (projects.Count == 0) {
                AnsiConsole.MarkupLine(noProjectsResponse);
                return;
            }
            Project? project = ConsoleInputHelper.ListSelection<Project>("Select a project to remove a pipeline step from:", projects);
            if (project == null)
                return; // User cancelled selection
            if (project.GetSprints().Count == 0) {
                AnsiConsole.MarkupLine(noSprintsRespone);
                return;
            }
            Sprint? sprint = ConsoleInputHelper.ListSelection<Sprint>("Select a sprint to remove a pipeline step from:", project.GetSprints());
            if (sprint == null)
                return; // User cancelled selection
            var steps = sprint.GetPipelineSteps();
            if (steps == null) {
                AnsiConsole.MarkupLine("[red]This sprint has no pipeline.[/]");
                return;
            }
            if (steps.Count == 0) {
                AnsiConsole.MarkupLine("[yellow]No pipeline steps available in this sprint.[/]");
                return;
            }
            IPipelineStep? step = ConsoleInputHelper.ListSelection<IPipelineStep>("Select a pipeline step to remove:", steps);
            if (step == null)
                return; // User cancelled selection
            sprint.RemoveStepFromPipeline(step);
            AnsiConsole.MarkupLine($"[green]Removed step:[/] {step.GetType().Name} from sprint {sprint.GetName()} in project {project.GetName()}");
        });

        var addMessageToSprintItemCommand = new Command("post-message-to-sprint-item", "Add a message to a sprint item");
        addMessageToSprintItemCommand.SetAction((context) => {
            var (project, sprint, item) = ConsoleInputHelper.GetSprintItem(projects);
            if (item == null || sprint == null || project == null) {
                AnsiConsole.MarkupLine("[red]Invalid selection. Please try again.[/]");
                return;
            }
            TeamMember? teamMember = ConsoleInputHelper.ListSelection<TeamMember>("Select a team member to post the message as:", project.GetTeamMembers());
            if (teamMember == null)
                return; // User cancelled selection
            string message = ConsoleInputHelper.UserInput("Enter the message to post");
            if (message == string.Empty)
                return; // User cancelled input
            item.AddMessage(new FormMessage(teamMember, message));
            AnsiConsole.MarkupLine($"[green]Added message:[/] {message} to sprint item {item.GetBacklogItem().GetTask()} in sprint {sprint.GetName()} of project {project.GetName()}");
        });

        var getMessagesFromSprintItemCommand = new Command("get-messages-from-sprint-item", "Get messages from a sprint item");
        getMessagesFromSprintItemCommand.SetAction((context) => {
            var (_, _, item) = ConsoleInputHelper.GetSprintItem(projects);
            if (item == null)
                return; // User cancelled selection
            var messages = item.GetMessages();
            if (messages.Count == 0) {
                AnsiConsole.MarkupLine("[yellow]No messages available for this sprint item.[/]");
                return;
            }
            var table = new Table();
            table.AddColumn("Team Member");
            table.AddColumn("Message");
            foreach (var message in messages) {
                table.AddRow(message.GetSender().GetName(), message.GetMessage());
            }
            AnsiConsole.Write(table);
        });

        var startSprintReleaseCommand = new Command("start-sprint-release", "Start a release for a sprint");
        startSprintReleaseCommand.SetAction((context) => {
            if (projects.Count == 0) {
                AnsiConsole.MarkupLine(noProjectsResponse);
                return;
            }
            Project? project = ConsoleInputHelper.ListSelection<Project>("Select a project to start a sprint release in:", projects);
            if (project == null)
                return; // User cancelled selection
            if (project.GetSprints().Count == 0) {
                AnsiConsole.MarkupLine(noSprintsRespone);
                return;
            }
            Sprint? sprint = ConsoleInputHelper.ListSelection<Sprint>("Select a sprint to start a release for:", project.GetSprints());
            if (sprint == null)
                return; // User cancelled selection
            AnsiConsole.MarkupLine($"[green]Started release for sprint:[/] {sprint.GetName()} in project {project.GetName()}");
            var result = sprint.StartRelease(true);
            if (result) {
                AnsiConsole.MarkupLine("[green]Release completed successfully![/]");
            } else {
                AnsiConsole.MarkupLine("[red]Release failed. Please check the logs and try again.[/]");
            }
        });

        var uploadSummarySprintCommand = new Command("upload-sprint-summary", "Upload a summary for a sprint");
        uploadSummarySprintCommand.SetAction((context) => {
            if (projects.Count == 0) {
                AnsiConsole.MarkupLine(noProjectsResponse);
                return;
            }
            Project? project = ConsoleInputHelper.ListSelection<Project>("Select a project to upload a sprint summary for:", projects);
            if (project == null)
                return; // User cancelled selection
            if (project.GetSprints().Count == 0) {
                AnsiConsole.MarkupLine(noSprintsRespone);
                return;
            }
            Sprint? sprint = ConsoleInputHelper.ListSelection<Sprint>("Select a sprint to upload a summary for:", project.GetSprints());
            if (sprint == null)
                return; // User cancelled selection
            if (sprint.GetPipelineSteps() != null) {
                AnsiConsole.MarkupLine("[yellow]This is a release sprint and should therefore be ended with a sprint release instead of a summary[/]");
                return;
            }
            string summary = ConsoleInputHelper.UserInput("Enter the sprint summary");
            if (summary == string.Empty)
                return; // User cancelled input
            sprint.UploadSummary(summary);
            AnsiConsole.MarkupLine($"[green]Uploaded summary for sprint:[/] {sprint.GetName()} in project {project.GetName()}");
        });

        var printSprintCommand = new Command("print-sprint", "Print the details of a sprint");
        printSprintCommand.SetAction((context) => {
            if (projects.Count == 0) {
                AnsiConsole.MarkupLine(noProjectsResponse);
                return;
            }
            Project? project = ConsoleInputHelper.ListSelection<Project>("Select a project to print sprint details for:", projects);
            if (project == null)
                return; // User cancelled selection
            if (project.GetSprints().Count == 0) {
                AnsiConsole.MarkupLine(noSprintsRespone);
                return;
            }
            Sprint? sprint = ConsoleInputHelper.ListSelection<Sprint>("Select a sprint to print details for:", project.GetSprints());
            if (sprint == null)
                return; // User cancelled selection
            AnsiConsole.MarkupLine($"[blue]Sprint Name:[/] {sprint.GetName()}");
            AnsiConsole.MarkupLine($"[blue]Start Date:[/] {sprint.GetStartDate()}");
            AnsiConsole.MarkupLine($"[blue]End Date:[/] {sprint.GetEndDate()}");
            AnsiConsole.MarkupLine($"[blue]State:[/] {sprint.GetState().GetType().Name.Replace(stateSuffix, string.Empty)}");
            if (sprint.GetSummary() != string.Empty) {
                AnsiConsole.MarkupLine($"[blue]Summary:[/] \n {sprint.GetSummary()}");
            }
            var table = new Table();
            table.AddColumn("Backlog Item");
            table.AddColumn(stateSuffix);
            foreach (var item in sprint.GetSprintBacklog()) {
                table.AddRow(item.GetBacklogItem().GetTask(), item.GetState().GetType().Name.Replace(stateSuffix, string.Empty));
            }
            AnsiConsole.Write(table);
        });

        var printPipelineCommand = new Command("print-pipeline", "Print the details of a sprint's pipeline");
        printPipelineCommand.SetAction((context) => {
            if (projects.Count == 0) {
                AnsiConsole.MarkupLine(noProjectsResponse);
                return;
            }
            Project? project = ConsoleInputHelper.ListSelection<Project>("Select a project to print pipeline details for:", projects);
            if (project == null)
                return; // User cancelled selection
            if (project.GetSprints().Count == 0) {
                AnsiConsole.MarkupLine(noSprintsRespone);
                return;
            }
            Sprint? sprint = ConsoleInputHelper.ListSelection<Sprint>("Select a sprint to print pipeline details for:", project.GetSprints());
            if (sprint == null)
                return; // User cancelled selection

            var steps = sprint.GetPipelineSteps();
            if (steps == null) {
                AnsiConsole.MarkupLine("[red]This sprint does not have a pipeline.[/]");
                return;
            }
            if (steps.Count == 0) {
                AnsiConsole.MarkupLine("[yellow]The pipeline in this sprint has no steps yet. Please add steps to the pipeline before starting a release[/]");
                return;
            }
            foreach (var step in steps) {
                AnsiConsole.MarkupLine($"[blue]Pipeline Step:[/] {step.GetType().Name.Replace("Step", string.Empty)}");
            }
        });

        var printSprintItemCommand = new Command("print-sprint-item", "Print the details of a sprint item");
        printSprintItemCommand.SetAction((context) => {
            var (_, _, item) = ConsoleInputHelper.GetSprintItem(projects);
            if (item == null)
                return; // User cancelled selection
            AnsiConsole.MarkupLine($"[blue]Sprint Item:[/] {item.GetBacklogItem().GetTask()}");
            var state = item.GetState();
            AnsiConsole.MarkupLine($"[blue]State:[/] {state.GetType().Name.Replace(stateSuffix, string.Empty)}");
            if (state is not TodoState or DoingState && item.versionControlFacade!.CurrentBranch is not null) {
                AnsiConsole.MarkupLine($"[blue]Changes pushed to branch:[/] {item.versionControlFacade.CurrentBranch.Name}");
            }
            AnsiConsole.MarkupLine($"[blue]Assigned Developer:[/] {item.GetDeveloper()?.GetName() ?? "None"}");
            var messages = item.GetMessages();
            if (messages.Count > 0) {
                var table = new Table();
                table.AddColumn("Team Member");
                table.AddColumn("Message");
                foreach (var message in messages) {
                    table.AddRow(message.GetSender().GetName(), message.GetMessage());
                }
                AnsiConsole.Write(table);
            } else {
                AnsiConsole.MarkupLine("[yellow]No messages available for this sprint item.[/]");
            }
        });

        var assignDeveloperToSprintItemCommand = new Command("assign-developer-to-sprint-item", "Assign a developer to a sprint item");
        assignDeveloperToSprintItemCommand.SetAction((context) => {
            var (project, _, item) = ConsoleInputHelper.GetSprintItem(projects);
            if (project == null || item == null)
                return; // User cancelled selection
            TeamMember? developer = ConsoleInputHelper.ListSelection<TeamMember>("Select a developer to assign to the sprint item:", project.GetTeamMembers().Where(tm => tm is Developer));
            if (developer == null)
                return; // User cancelled selection
            item.AssignDeveloper(developer);
        });

        var enterSCMCommand = new Command("enter-scm", "Enter the source control management (SCM) interface");
        enterSCMCommand.SetAction((context) => {
            var (project, sprint, item) = ConsoleInputHelper.GetSprintItem(projects);
            if (project == null || sprint == null || item == null)
                return; // User cancelled selection
            AnsiConsole.MarkupLine($"[blue]Entering SCM interface for sprint item:[/] {item.GetBacklogItem().GetTask()} in sprint {sprint.GetName()} of project {project.GetName()}");
            while (true) {
                var command = ConsoleInputHelper.UserInput(string.Empty);
                if (string.IsNullOrWhiteSpace(command)) {
                    AnsiConsole.MarkupLine("[yellow]Exiting SCM cli.[/]");
                    break;
                } else {
                    ConsoleInputHelper.HandleSCMCommand(command, item);
                }
            }
        });

        var testSprintItemCommand = new Command("test-sprint-item", "Test a sprint item");
        testSprintItemCommand.SetAction((context) => {
            var (_, _, item) = ConsoleInputHelper.GetSprintItem(projects);
            if (item == null)
                return; // User cancelled selection
            bool success = ConsoleInputHelper.BoolUserInput("Tests executed successfully?", true);
            if (success) {
                item.TestSucceeded();
            } else {
                item.TestFailed();
            }
        });

        var definitionOfDoneSprintItemCommand = new Command("definition-of-done-sprint-item", "Check the definition of done for a sprint item");
        definitionOfDoneSprintItemCommand.SetAction((context) => {
            var (_, _, item) = ConsoleInputHelper.GetSprintItem(projects);
            if (item == null)
                return; // User cancelled selection
            item.DefinitionOfDoneCheck();
        });

        var setPipelineCommand = new Command("set-pipeline", "Set the pipeline type for a sprint");
        setPipelineCommand.SetAction((context) => {
            if (projects.Count == 0) {
                AnsiConsole.MarkupLine(noProjectsResponse);
                return;
            }
            Project? project = ConsoleInputHelper.ListSelection<Project>("Select a project to set a pipeline for:", projects);
            if (project == null)
                return; // User cancelled selection
            if (project.GetSprints().Count == 0) {
                AnsiConsole.MarkupLine(noSprintsRespone);
                return;
            }
            Sprint? sprint = ConsoleInputHelper.ListSelection<Sprint>("Select a sprint to set a pipeline for:", project.GetSprints());
            if (sprint == null)
                return; // User cancelled selection
            PipelineType? pipelineType = ConsoleInputHelper.EnumSelection<PipelineType>("Select the pipeline type for the sprint");
            if (pipelineType == null)
                return; // User cancelled selection
            Pipeline pipeline = pipelineType switch {
                PipelineType.Deploy => new DeployPipeline(),
                PipelineType.Test => new TestPipeline(),
                _ => throw new ArgumentException("Invalid pipeline type")
            };
            List<string> selected = AnsiConsole.Prompt(new MultiSelectionPrompt<string>()
                .Title("Select pipeline steps to add to the pipeline:")
                .AddChoices(Enum.GetNames(typeof(PipelineStepType))));

            List<IPipelineStep> steps = [];
            while (selected.Count > 0) {
                AnsiConsole.MarkupLine($"[blue]Steps to order:[/] {string.Join(", ", selected)}");

                var nextStep = ConsoleInputHelper.ListSelection<string>("Choose the order in which to put the selected steps:", selected);
                if (nextStep == null)
                    return; // User cancelled selection
                selected.Remove(nextStep);
                IPipelineStep step = nextStep switch {
                    "Analyse" => new AnalyseStep(),
                    "Build" => new BuildStep(),
                    "Deploy" => new DeployStep(),
                    "Package" => new PackageStep(),
                    "Source" => new SourceStep(),
                    "Test" => new TestStep(),
                    "Utility" => new UtilityStep(),
                    _ => throw new ArgumentException("Invalid pipeline step type")
                };
                steps.Add(step);
            }
            pipeline.SetSteps(steps);
            sprint.SetPipeline(pipeline);
            AnsiConsole.MarkupLine($"[green]Set pipeline:[/] {pipelineType} for sprint {sprint.GetName()} in project {project.GetName()}");
        });

        rootCommand.Add(createTeamMemberCommand);
        rootCommand.Add(createProjectCommand);
        rootCommand.Add(deleteProjectCommand);
        rootCommand.Add(addTeamMemberToProjectCommand);
        rootCommand.Add(createSprintCommand);
        rootCommand.Add(addBacklogItemCommand);
        rootCommand.Add(deleteBacklogItemCommand);
        rootCommand.Add(listProjectsCommand);
        rootCommand.Add(listTeamMembersCommand);
        rootCommand.Add(addSprintItemCommand);
        rootCommand.Add(deleteSprintItemCommand);
        rootCommand.Add(startSprintCommand);
        rootCommand.Add(stopSprintCommand);
        rootCommand.Add(addPipelineStepCommand);
        rootCommand.Add(removePipelineStepCommand);
        rootCommand.Add(addMessageToSprintItemCommand);
        rootCommand.Add(getMessagesFromSprintItemCommand);
        rootCommand.Add(startSprintReleaseCommand);
        rootCommand.Add(uploadSummarySprintCommand);
        rootCommand.Add(printSprintCommand);
        rootCommand.Add(printPipelineCommand);
        rootCommand.Add(printSprintItemCommand);
        rootCommand.Add(assignDeveloperToSprintItemCommand);
        rootCommand.Add(enterSCMCommand);
        rootCommand.Add(testSprintItemCommand);
        rootCommand.Add(definitionOfDoneSprintItemCommand);
        rootCommand.Add(setPipelineCommand);
        rootCommand.Add(quit);

        var commandGroups = new Dictionary<string, List<Command>>
        {
            {
                "Projects",
                [
                    listProjectsCommand,
                    createProjectCommand,
                    deleteProjectCommand,
                    addTeamMemberToProjectCommand,
                    createSprintCommand,
                ]
            },
            {
                "Team Members",
                [
                    listTeamMembersCommand,
                    createTeamMemberCommand,
                    addTeamMemberToProjectCommand,
                ]
            },
            {
                "Sprints",
                [
                    printSprintCommand,
                    createSprintCommand,
                    startSprintCommand,
                    stopSprintCommand,
                    addSprintItemCommand,
                    deleteSprintItemCommand,
                    setPipelineCommand,
                    addPipelineStepCommand,
                    removePipelineStepCommand,
                    startSprintReleaseCommand,
                    uploadSummarySprintCommand,
                ]
            },
            {
                "Backlog Items",
                [
                    printSprintItemCommand,
                    addBacklogItemCommand,
                    deleteBacklogItemCommand,
                    addSprintItemCommand,
                    deleteSprintItemCommand,
                    assignDeveloperToSprintItemCommand,
                    testSprintItemCommand,
                    definitionOfDoneSprintItemCommand,
                    addMessageToSprintItemCommand,
                    getMessagesFromSprintItemCommand,
                    enterSCMCommand,
                ]
            },
            {
                "Pipelines",
                [
                    printPipelineCommand,
                    setPipelineCommand,
                    addPipelineStepCommand,
                    removePipelineStepCommand,
                    startSprintReleaseCommand,
                ]
            },
        };

        while (true) {
            string[] groupNames = [.. commandGroups.Keys];
            string exitOption = "[red]Exit[/]";
            string group = await AnsiConsole.PromptAsync(new SelectionPrompt<string>()
                .Title("Select a command group:")
                .AddChoices(groupNames.Append(exitOption)));

            if (group == exitOption)
                break;

            string backOption = "[yellow]Back[/]";
            string command = await AnsiConsole.PromptAsync(new SelectionPrompt<string>()
            .Title($"Select a command from the [blue]{group}[/] group:")
            .AddChoices(commandGroups[group].Select(c => c.Name).Append(backOption)));

            if (command == backOption)
                continue;

            string[] inputs = ConsoleInputHelper.ParseCommandLine(command);

            await rootCommand.Parse(inputs).InvokeAsync();
        }
    }
}
