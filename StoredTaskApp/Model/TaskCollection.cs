using System.Collections.Generic;

namespace StoredTaskApp.Model
{
    /// <summary>
    /// This is the TaskCollection Class to store any task to be created.
    /// The task collection class has
    ///     No constructor
    ///     1 property List of TaskLists
    ///     2 calculated properties Count, Count Of Incomplete Tasks
    ///     2 methods Add Task List To Collection & Remove All Completed Tasks
    /// </summary>

    public class TaskCollection
    {
        private List<TaskList> _tasklists;

        public int Count
        {
            get
            {
                //Only count items for lists that are not empty
                int tempCount = 0;

                if (_tasklists != null)
                    foreach (TaskList tasklist in _tasklists)
                        tempCount += tasklist.Count;

                return tempCount;
            }
        }

        public int Count_Of_Incomplete_Tasks
        {
            get
            {
                //Only count incomplete tasks for lists that are not empty
                int tempCount = 0;

                if (_tasklists != null)
                    foreach (TaskList tasklist in _tasklists)
                        tempCount += tasklist.Count_Of_Incomplete_Tasks;

                return tempCount;
            }
        }

        public void Add_TaskListToCollection(TaskList tasklist)
        {
            //Check if the _tasklists has been initialised, if not then initialise before adding the Task List
            if (_tasklists == null)
                _tasklists = new List<TaskList>();

            _tasklists.Add(tasklist);
        }

        public void Remove_All_Completed_Tasks()
        {
            if (_tasklists != null)
            {
                foreach (TaskList tasklist in _tasklists)
                    tasklist.Remove_Completed_Tasks();
            }
        }
    }
}
