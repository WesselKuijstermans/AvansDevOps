using AvansDevOps.ItemStatePattern;
using AvansDevOps.StateObserverPattern;
using AvansDevOps.TemplatePattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.Entities
{
    internal class Project: IItemStateObserver
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

        public void AddSprint(string name, DateTime startDate, DateTime endDate, PipelineTemplate pipeline)
        {
            Sprint sprint = new Sprint(name, startDate, endDate, pipeline, this);
            sprints.Add(sprint);
        }

        public void RemoveSprint(string name)
        {
            sprints.Remove(sprints.Find(s => s.GetName() == name));
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

        public void Update(IItemState newState, SprintItem item)
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
                    break;
            }
        }

        private void NotifyTesters(string message)
        {
            
            foreach (TeamMember teamMember in teamMembers)
            {
                if (teamMember is Tester)
                {
                    teamMember.notify("To " + teamMember.getName() + ':' + message);
                }
            }
        }

        private void NotifyDevelopers(string message)
        {
            foreach (TeamMember teamMember in teamMembers)
            {
                if (teamMember is Developer || teamMember is LeadDeveloper)
                {
                    teamMember.notify("To " + teamMember.getName() + ':' + message);
                }
            }
        }

        private void NotifySrumMaster(string message)
        {
            foreach (TeamMember teamMember in teamMembers)
            {
                if (teamMember is ScrumMaster)
                {
                    teamMember.notify("To " + teamMember.getName() + ':' + message);
                }
            }
        }
    }
}
