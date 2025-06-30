namespace AvansDevOps.Entities {
    public class Commit(string message, TeamMember teamMember) {
        private readonly string Message = message;
        private readonly TeamMember TeamMember = teamMember;
        private readonly DateTime Timestamp = DateTime.Now;

        public string GetMessage() {
            return Message;
        }

        public TeamMember GetTeamMember() {
            return TeamMember;
        }

        public DateTime GetTimestamp() {
            return Timestamp;
        }
    }
}
