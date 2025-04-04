using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.Entities
{
    public class BacklogItem
    {
        private string task;
        private int storyPoints;

        public BacklogItem(string task, int storyPoints)
        {
            this.task = task;
            this.storyPoints = storyPoints;
        }

        public string GetTask()
        {
            return this.task;
        }

        public int GetStoryPoints()
        {
            return this.storyPoints;
        }

        public void SetTask(string task)
        {
            this.task = task;
        }

        public void SetStoryPoints(int storyPoints)
        {
            this.storyPoints = storyPoints;
        }
    }
}
