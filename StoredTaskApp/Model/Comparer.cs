using System;
using System.Collections.Generic;

namespace StoredTaskApp.Model.Comparer
{
    public class PriorityComparer : IComparer<Task>
    {
        public int Compare(Task task1, Task task2)
        {
            if (task1 == null || task2 == null)
            {
                return 0;
            }

            return task1.Task_Priority.CompareTo(task2.Task_Priority);
        }
    }

    public class DescriptionComparer : IComparer<Task>
    {
        public int Compare(Task task1, Task task2)
        {
            if (task1 == null || task2 == null)
            {
                return 0;
            }

            return task1.Description.CompareTo(task2.Description);
        }
    }

    public class DueDateThanPriorityComparer : IComparer<Task>
    {
        public int Compare(Task task1, Task task2)
        {
            if (task1 == null || task2 == null)
            {
                return 0;
            }

            int dateCompare = Nullable.Compare(task1.Task_Completion_Date, task2.Task_Completion_Date);
            if (dateCompare != 0)
            {
                return dateCompare;
            }

            return task1.Task_Priority.Value.CompareTo(task2.Task_Priority.Value);
        }
    }
}
