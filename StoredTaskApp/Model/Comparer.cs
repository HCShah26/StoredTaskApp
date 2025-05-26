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

            // Compare only the date parts (ignore time)
            DateTime? date1 = task1.Task_Completion_Date?.Date;
            DateTime? date2 = task2.Task_Completion_Date?.Date;

            int dateCompare = Nullable.Compare(date1, date2);
            if (dateCompare != 0)
            {
                return dateCompare;
            }

            return task1.Task_Priority.Value.CompareTo(task2.Task_Priority.Value);
        }
    }
}
