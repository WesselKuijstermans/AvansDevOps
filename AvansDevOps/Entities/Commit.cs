using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.Entities
{
    public class Commit(string name, TeamMember teamMember)
    {
        private string Name { get; set; } = name;
        private TeamMember TeamMember { get; } = teamMember;
        private DateTime timestamp { get; } = DateTime.Now;
    }
}
