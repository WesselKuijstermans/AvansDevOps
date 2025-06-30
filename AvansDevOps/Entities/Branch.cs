namespace AvansDevOps.Entities {
    public class Branch {
        public string Name { get; }
        private readonly List<Commit> Commits = [];

        public Branch(string name) {
            this.Name = name;
        }

        public void AddCommit(Commit commit) {
            Commits.Add(commit);
        }

        public List<Commit> GetCommits() {
            return Commits;
        }
    }
}
