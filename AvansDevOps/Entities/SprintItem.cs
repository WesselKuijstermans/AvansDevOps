using AvansDevOps.FacadePattern;
using AvansDevOps.FormMessageObersverPattern;
using AvansDevOps.ItemStateObserverPattern;
using AvansDevOps.ItemStatePattern;
using Spectre.Console;

namespace AvansDevOps.Entities {
    public class SprintItem {
        private readonly BacklogItem backlogItem;
        private TeamMember? developer;
        private readonly List<SprintItem> subtasks;
        private IItemState state;
        private readonly IItemStateObserver itemStateObserver;
        private readonly IFormMessageObserver formObserver;
        private readonly List<FormMessage> messages;
        public VersionControlFacade versionControlFacade;

        public SprintItem(BacklogItem backlogItem, IItemStateObserver itemObserver, IFormMessageObserver formObserver, VersionControlFacade facade) {
            this.backlogItem = backlogItem;
            this.subtasks = [];
            this.state = new TodoState(this);
            this.itemStateObserver = itemObserver;
            this.formObserver = formObserver;
            this.messages = [];
            this.versionControlFacade = facade;
        }

        public void AddSubtask(SprintItem sprintItem) {
            this.subtasks.Add(sprintItem);
        }

        public void SetDeveloper(TeamMember teamMember) {
            if (teamMember is Developer || teamMember is LeadDeveloper || teamMember is null) {
                this.developer = teamMember;
            } else {
                AnsiConsole.MarkupLine("[yellow]Assigned team member is not a developer or lead developer.[/]");
            }
        }

        public Developer? GetDeveloper() {
            if (this.developer is not null && (this.developer is Developer || this.developer is LeadDeveloper)) {
                return ( Developer )this.developer;
            }
            AnsiConsole.WriteLine("No member assigned or assigned member is not a developer.");
            return null;
        }

        public void SetState(IItemState state) {
            this.state = state;
            NotifyObserver(state);
        }

        public IItemState GetState() {
            return this.state;
        }

        public BacklogItem GetBacklogItem() {
            return this.backlogItem;
        }

        public List<SprintItem> GetSubtasks() {
            return this.subtasks;
        }

        public void NotifyObserver(IItemState newState) {
            itemStateObserver.ItemUpdate(newState, this);
        }

        public void AssignDeveloper(TeamMember teamMember) {
            this.state.AssignDeveloper(teamMember);
        }

        public void ReadyForTesting() {
            this.state.ReadyForTesting();
        }

        public void TestSucceeded() {
            this.state.TestSucceeded();
        }

        public void TestFailed() {
            this.state.TestFailed();
        }

        public void DefinitionOfDoneCheck() {
            this.state.DefinitionOfDoneCheck();
        }

        public void PullBranch(string branchName) {
            versionControlFacade.Pull(branchName);
        }

        public void CheckoutBranch(string branchName) {
            versionControlFacade.Checkout(branchName);
        }

        public void Commit(string message) {
            TeamMember? developer = this.GetDeveloper();
            if (developer is not null) {
                versionControlFacade.Commit(message, developer);
            } else {
                AnsiConsole.WriteLine("No developer assigned to commit changes.");
            }
        }

        public void Push() {
            versionControlFacade.Push();
        }

        public void AddMessage(FormMessage message) {
            if (state is not DoneState) {
                this.messages.Add(message);
                List<TeamMember> membersToNotify = [];
                foreach (FormMessage msg in GetMessages()) {
                    if (msg.GetSender() != message.GetSender()) {
                        membersToNotify.Add(msg.GetSender());
                    }
                }
                string notificationMessage = message.GetSender().GetName() + " has sent a message to sprint item " + this.GetBacklogItem().GetTask() + ": " + message.GetMessage();
                formObserver.FormUpdate(notificationMessage, membersToNotify);
            }
        }

        public List<FormMessage> GetMessages() {
            return this.messages;
        }

        public void RemoveMessage(FormMessage message) {
            if (state is not DoneState) {
                this.messages.Remove(message);
            }
        }

        public override string ToString() {
            return $"SprintItem: {this.backlogItem.GetTask()} - State: {this.state.GetType().Name} - Developer: {this.developer?.GetName() ?? "None"}";
        }
    }
}
