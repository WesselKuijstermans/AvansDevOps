using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvansDevOps.FormMessageObersverPattern;
using AvansDevOps.ItemStateObserverPattern;
using AvansDevOps.ItemStatePattern;
using AvansDevOps.SprintStateObersverPattern;
using AvansDevOps.SprintStatePattern;

namespace AvansDevOps.Entities
{
    public class NotificationService(Project project) : IItemStateObserver, ISprintStateObserver, IFormMessageObserver
    {
        public void ItemUpdate(IItemState newState, SprintItem item)
        {
            string message = "The item " + item.GetBacklogItem().GetTask() + " from the project '" + project.GetName() + "' is now in state " + newState.GetType().Name;
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
            string message = "The sprint " + sprint.GetName() + " from the project '" + project.GetName() + "' is now in state " + newState.GetType().Name;
            switch (newState)
            {
                case InProgressSprintState:
                    break;
                case StoppedSprintState:
                    NotifySrumMaster(message);
                    break;
                case FinishedSprintState:
                    break;
            }
        }

        public void FormUpdate(string notification, List<TeamMember> teamMembers)
        {
            foreach (TeamMember teamMember in teamMembers)
            {
                teamMember.Notify("New message in item form", notification);
            }
        }

        private void NotifyTesters(string message)
        {
            foreach (TeamMember teamMember in project.GetTeamMembers())
            {
                if (teamMember is Tester)
                {
                    teamMember.Notify("New item state", message);
                }
            }
        }

        private void NotifyDevelopers(string message)
        {
            foreach (TeamMember teamMember in project.GetTeamMembers())
            {
                if (teamMember is Developer || teamMember is LeadDeveloper)
                {
                    teamMember.Notify("New item state", message);
                }
            }
        }

        private void NotifySrumMaster(string message)
        {
            foreach (TeamMember teamMember in project.GetTeamMembers())
            {
                if (teamMember is ScrumMaster)
                {
                    teamMember.Notify("New item state", message);
                }
            }
        }
    }
}
