using AvansDevOps.PipelineStrategyPattern;
using AvansDevOps.SprintStateObersverPattern;

namespace AvansDevOps.Entities {
    public class Project(string name) {
        private readonly string name = name;
        private readonly List<TeamMember> teamMembers = [];
        private readonly List<Sprint> sprints = [];
        private readonly List<BacklogItem> backlog = [];

        public string GetName() {
            return this.name;
        }

        public void AddSprint(string name, DateTime startDate, DateTime endDate, Pipeline? pipeline, ISprintStateObserver observer) {
            Sprint sprint = new(name, startDate, endDate, pipeline, observer);
            sprints.Add(sprint);
        }

        public void RemoveSprint(string name) {
            var sprint = sprints.Find(s => s.GetName() == name);
            if (sprint != null) {
                sprints.Remove(sprint);
            }
        }

        public List<Sprint> GetSprints() {
            return sprints;
        }

        public void AddTeamMember(TeamMember teamMember) {
            teamMembers.Add(teamMember);
        }

        public void RemoveTeamMember(TeamMember teamMember) {
            teamMembers.Remove(teamMember);
        }

        public List<TeamMember> GetTeamMembers() {
            return teamMembers;
        }

        public void AddBacklogItem(BacklogItem backlogItem) {
            backlog.Add(backlogItem);
        }

        public void RemoveBacklogItem(BacklogItem backlogItem) {
            backlog.Remove(backlogItem);
        }

        public List<BacklogItem> GetBacklogItems() {
            return backlog;
        }

        public override string ToString() {
            return name;
        }
    }
}
