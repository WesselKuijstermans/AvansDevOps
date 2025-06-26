using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.Entities
{
    public class Branch
    {
        public string Name {  get; }
        private List<Commit> commits { get; } = new List<Commit>();

        public Branch(string name) {
            this.Name = name;
        }

        public void addCommit(Commit commit)
        {
            commits.Add(commit);
        }
    }
}
