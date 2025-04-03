using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.Entities
{
    internal class BacklogItem
    {
        private string task;
        private int storyPoints;
        private List<BacklogItem> subTasks;
        private bool isSubTask;

        public BacklogItem(string task, int storyPoints)
        {
            this.task = task;
            this.storyPoints = storyPoints;
            this.subTasks = new List<BacklogItem>();
            this.isSubTask = false;
        }

        public void AddSubTask(BacklogItem subTask)
        {
            this.subTasks.Add(subTask);
            subTask.isSubTask = true;
        }

        public void RemoveSubTask(BacklogItem subTask)
        {
            this.subTasks.Remove(subTask);
            subTask.isSubTask = false;
        }

        public List<BacklogItem> GetSubTasks()
        {
            return this.subTasks;
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
