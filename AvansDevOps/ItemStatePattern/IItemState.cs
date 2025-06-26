using AvansDevOps.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.ItemStatePattern
{
    public interface IItemState
    {
        void AssignDeveloper(TeamMember teamMember);
        void ReadyForTesting();
        void TestSucceeded();
        void TestFailed();
        void DefinitionOfDoneCheck();
    }
}
