using AvansDevOps.FormMessageObersverPattern;
using AvansDevOps.ItemStatePattern;
using AvansDevOps.StateObserverPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.Entities
{
    public class SprintItem
    {
        private Sprint sprint;
        private BacklogItem backlogItem;
        private TeamMember? developer;
        private List<SprintItem> subtasks;
        private IItemState state;
        private IItemStateObserver itemStateObserver;
        private IFormMessageObserver formObserver;
        private List<FormMessage> messages;

        public SprintItem(Sprint sprint, BacklogItem backlogItem, Project observer)
        {
            this.sprint = sprint;
            this.backlogItem = backlogItem;
            this.subtasks = new List<SprintItem>();
            this.state = new TodoState(this);
            this.itemStateObserver = observer;
            this.formObserver = observer;
            this.messages = new List<FormMessage>();
        }

        public void AddSubtask(SprintItem sprintItem)
        {
            this.subtasks.Add(sprintItem);
        }

        public void SetDeveloper(TeamMember teamMember)
        {
            if (teamMember is Developer)
            {
                this.developer = teamMember;
            }
        }

        public void SetState(IItemState state)
        {
            this.state = state;
            NotifyObserver(state);
        }

        public IItemState GetState()
        {
            return this.state;
        }

        public BacklogItem GetBacklogItem()
        {
            return this.backlogItem;
        }

        public List<SprintItem> GetSubtasks()
        {
            return this.subtasks;
        }

        public void NotifyObserver(IItemState newState)
        {
            itemStateObserver.ItemUpdate(newState, this);
        }

        public void AssignDeveloper(TeamMember teamMember)
        {
            this.state.assignDeveloper(teamMember);
        }

        public void ReadyForTesting()
        {
            this.state.readyForTesting();
        }

        public void TestSucceeded()
        {
            this.state.testSucceeded();
        }

        public void TestFailed()
        {
            this.state.testFailed();
        }

        public void DefinitionOfDoneCheck()
        {
            this.state.definitionOfDoneCheck();
        }

        public void AddMessage(FormMessage message)
        {
            if (state is not DoneState)
            {
                this.messages.Add(message);
                List<TeamMember> membersToNotify = new List<TeamMember>();
                foreach (FormMessage msg in GetMessages())
                {
                    if (msg.GetSender() != message.GetSender())
                    {
                        membersToNotify.Add(msg.GetSender());
                    }
                }
                string notificationMessage = message.GetSender().GetName() + " has sent a message to sprint item " + this.GetBacklogItem().GetTask() + ": " + message.GetMessage();
                formObserver.FormUpdate(notificationMessage, membersToNotify);
            }
        }

        public List<FormMessage> GetMessages()
        {
            return this.messages;
        }

        public void RemoveMessage(FormMessage message)
        {
            if (state is not DoneState)
            {  
                this.messages.Remove(message);
            }
        }
    }
}
