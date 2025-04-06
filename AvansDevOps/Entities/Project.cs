using AvansDevOps.FormMessageObersverPattern;
using AvansDevOps.ItemStatePattern;
using AvansDevOps.SprintStateObersverPattern;
using AvansDevOps.SprintStatePattern;
using AvansDevOps.StateObserverPattern;
using AvansDevOps.TemplatePattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.Entities
{
    public class Project: IItemStateObserver, ISprintStateObserver, IFormMessageObserver
    {
        private string name;
        private List<TeamMember> teamMembers;
        private List<Sprint> sprints;
        private List<BacklogItem> backlog;

        public Project(string name)
        {
            this.name = name;
            this.teamMembers = new List<TeamMember>();
            this.sprints = new List<Sprint>();
            this.backlog = new List<BacklogItem>();
        }

        public string GetName()
        {
            return this.name;
        }

        public void AddSprint(string name, DateTime startDate, DateTime endDate, PipelineTemplate pipeline, ISprintStateObserver observer)
        {
            Sprint sprint = new Sprint(name, startDate, endDate, pipeline, observer);
            sprints.Add(sprint);
        }

        public void RemoveSprint(string name)
        {
            var sprint = sprints.Find(s => s.GetName() == name);
            if (sprint != null)
            {
                sprints.Remove(sprint);
            }
        }

        public List<Sprint> GetSprints()
        {
            return sprints;
        }

        public void AddTeamMember(TeamMember teamMember)
        {
            teamMembers.Add(teamMember);
        }

        public void RemoveTeamMember(TeamMember teamMember)
        {
            teamMembers.Remove(teamMember);
        }

        public List<TeamMember> GetTeamMembers()
        {
            return teamMembers;
        }

        public void AddBacklogItem(BacklogItem backlogItem)
        {
            backlog.Add(backlogItem);
        }

        public void RemoveBacklogItem(BacklogItem backlogItem)
        {
            backlog.Remove(backlogItem);
        }

        public List<BacklogItem> GetBacklogItems()
        {
            return backlog;
        }

        public void ItemUpdate(IItemState newState, SprintItem item)
        {
            string message = "The item " + item.GetBacklogItem().GetTask() + " from the project '" + this.name + "' is now in state " + newState.GetType().Name;
            switch (newState)
            {
                case TestedState:
                    NotifySrumMaster(message);
                    break;
                case ReadyForTestingState:
                    NotifyTesters(message);
                    break;
                case TodoState:
                    NotifyDevelopers(message);
                    NotifySrumMaster(message);
                    break;
            }
        }

        public void SprintUpdate(ISprintState newState, Sprint sprint)
        {
            string message = "The sprint " + sprint.GetName() + " from the project '" + this.name + "' is now in state " + newState.GetType().Name;
            switch (newState)
            {
                case DoneState:
                    NotifySrumMaster(message);
                    break;
                case InProgressSprintState:
                    break;
                case StoppedSprintState:
                    break;
            }
        }

        public void FormUpdate(string notification, List<TeamMember> members)
        {
            foreach (TeamMember member in members)
            {
                member.Notify(notification);
            }
        }

        private void NotifyTesters(string message)
        {
            
            foreach (TeamMember teamMember in teamMembers)
            {
                if (teamMember is Tester)
                {
                    teamMember.Notify("To " + teamMember.GetName() + ':' + message);
                }
            }
        }

        private void NotifyDevelopers(string message)
        {
            foreach (TeamMember teamMember in teamMembers)
            {
                if (teamMember is Developer || teamMember is LeadDeveloper)
                {
                    teamMember.Notify("To " + teamMember.GetName() + ':' + message);
                }
            }
        }

        private void NotifySrumMaster(string message)
        {
            foreach (TeamMember teamMember in teamMembers)
            {
                if (teamMember is ScrumMaster)
                {
                    teamMember.Notify("To " + teamMember.GetName() + ':' + message);
                }
            }
        }
    }
}
