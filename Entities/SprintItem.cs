using AvansDevOps.ItemStatePattern;
using AvansDevOps.StateObserverPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.Entities
{
    internal class SprintItem
    {
        private Sprint sprint;
        private BacklogItem backlogItem;
        private TeamMember? developer;
        private List<SprintItem> subtasks;
        private IItemState state;
        private IItemStateObserver observer;

        public SprintItem(Sprint sprint, BacklogItem backlogItem, Project observer)
        {
            this.sprint = sprint;
            this.backlogItem = backlogItem;
            this.subtasks = new List<SprintItem>();
            this.state = new TodoState(this);
            this.observer = observer;
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
            observer.ItemUpdate(newState, this);
        }
    }
}
