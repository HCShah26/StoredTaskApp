using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Documents;

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
        private List<TaskList> _taskCollection;

        public int Count
        {
            get
            {
                //Only count items for lists that are not empty
                int tempCount = 0;

                if (_taskCollection != null)
                    foreach (TaskList tasklist in _taskCollection)
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

                if (_taskCollection != null)
                    foreach (TaskList tasklist in _taskCollection)
                        tempCount += tasklist.Count_Of_Incomplete_Tasks;

                return tempCount;
            }
        }

        public List<TaskList> TaskLists
        {
            get
            {
                return _taskCollection;
            }
        }

        public void Add_TaskListToCollection(TaskList tasklist)
        {
            //Check if the _tasklists has been initialised, if not then initialise before adding the Task List
            if (_taskCollection == null)
                _taskCollection = new List<TaskList>();

            _taskCollection.Add(tasklist);
        }

        public void Remove_All_Completed_Tasks()
        {
            if (_taskCollection != null)
            {
                foreach (TaskList tasklist in _taskCollection)
                    tasklist.Remove_Completed_Tasks();
            }
        }

        public bool LoadTaskCollection()
        { 
            return _taskCollection != null; 
        }

        public async Task<bool> SaveTaskCollectionAsync()
        {
            return await TaskCollectionSerializer.SaveAsync(this);
        }


    }
}
