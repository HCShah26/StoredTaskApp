using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Xaml.Automation.Peers;

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

        public bool LoadTaskCollection()
        { 
            return _tasklists != null; 
        }

        public bool SaveTaskCollection()
        {
            Debug.WriteLine($"This collection has {this.Count} tasks");
            Debug.WriteLine($"This collection has {_tasklists.Count} lists");
            foreach(var taskList in _tasklists)
            {
                Debug.WriteLine($"Tasklist type is '{taskList.GetType().ToString()}'");
                foreach (var taskitem in (List<Task>)taskList.ReturnTasks)
                {
                    Debug.WriteLine($"  Task type is '{taskitem.GetType().ToString()}'");
                    switch (taskitem.GetType().ToString())
                        {
                        case "StoredTaskApp.Model.Task":
                        {
                            Task currentTask = (Task)taskitem;
                            Debug.WriteLine($"      Task Data Information Description -     {currentTask.Description}");
                            Debug.WriteLine($"      Task Data Information Notes -           {currentTask.Notes}");
                            Debug.WriteLine($"      Task Data Information Status -          {currentTask.Task_Status}");
                            Debug.WriteLine($"      Task Data Information Priority -        {currentTask.Task_Priority}");
                            Debug.WriteLine($"      Task Data Information Creation Date -   {currentTask.Task_Creation_Date}");
                            Debug.WriteLine($"      Task Data Information Completion Date - {currentTask.Task_Completion_Date}");
                            break;
                        }
                        case "StoredTaskApp.Model.RepeatingTask":
                        {
                            RepeatingTask currentTask = (RepeatingTask)taskitem;
                            Debug.WriteLine($"      Repeating Task Data Information Description -       {currentTask.Description}");
                            Debug.WriteLine($"      Repeating Task Data Information Notes -             {currentTask.Notes}");
                            Debug.WriteLine($"      Repeating Task Data Information Status -            {currentTask.Task_Status}");
                            Debug.WriteLine($"      Repeating Task Data Information Priority -          {currentTask.Task_Priority}");
                            Debug.WriteLine($"      Repeating Task Data Information Creation Date -     {currentTask.Task_Creation_Date}");
                            Debug.WriteLine($"      Repeating Task Data Information Completion Date -   {currentTask.Task_Completion_Date}");
                            break;
                        }
                        case "StoredTaskApp.Model.Habit":
                        {
                            Habit currentTask = (Habit)taskitem;
                            Debug.WriteLine($"      Habit Task Data Information Description -       {currentTask.Description}");
                            Debug.WriteLine($"      Habit Task Data Information Notes -             {currentTask.Notes}");
                            Debug.WriteLine($"      Habit Task Data Information Status -            {currentTask.Task_Status}");
                            Debug.WriteLine($"      Habit Task Data Information Priority -          {currentTask.Task_Priority}");
                            Debug.WriteLine($"      Habit Task Data Information Creation Date -     {currentTask.Task_Creation_Date}");
                            Debug.WriteLine($"      Habit Task Data Information Completion Date-    {currentTask.Task_Completion_Date}");
                            Debug.WriteLine($"      Habit Task Data Information Streak Count -      {currentTask.StreakCount}");
                            Debug.WriteLine($"      Habit Task Data Information Last Comp. Date-    {currentTask.LastCompletionDate}");
                            break;

                        }
                    }
                }
            }
            return _tasklists != null; 
        }

    }
}
