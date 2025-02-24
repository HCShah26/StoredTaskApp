using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace StoredTaskApp.Model
{
    /// <summary>
    /// This is the TaskList Class to store any task to be created.
    /// The task class has
    ///     1 constructor to create a new task list given the name for the task list
    ///     2 properties Name and Tasks (List of Tasks)
    ///     2 calculated properties Count, Count Of Incomplete Tasks
    ///     3 methods Add Task to the Task List, Display All Tasks in List & Remove Copleted Task from List
    /// </summary>

    public class TaskList
    {
        private string _name; //Can't be blank
        private List<Task> _tasks;

        public TaskList(string name)
        {
            if (name != string.Empty && name != null)
                _name = name;
            else
                //We shouldn't get here as this error should be handled in the View
                //If in any case we get here, instead of sending an error message, we will 
                //set the name value as "New List {Todays Date}"

                _name = "New List " + DateTime.Now.ToShortDateString();

            _tasks = new List<Task>();
        }

        public virtual string Name
        {
            get
            {
                return _name;
            }

            set
            {
                if (value != string.Empty && value != null)
                    _name = value;
                else
                    //We shouldn't get here as this error should be handled in the View
                    //If in any case we get here, instead of sending an error message, we will 
                    //set the name value as "New List {Todays Date}"

                    //Before setting the description value, check if it's empty before assigning the "New Task {Today's date}"
                    if (_name == string.Empty)
                    _name = "New List " + DateTime.Now.ToShortDateString();
            }
        }

        public int Count
        {
            get
            {
                return _tasks.Count;
            }
        }

        public int Count_Of_Incomplete_Tasks
        {
            get
            {
                // Only iterate through the list if it's not empty
                int tempCount = 0;

                if (_tasks != null)
                    foreach (Task task in _tasks)
                        if (task.Task_Status == false)
                            tempCount++;

                return tempCount;
            }
        }

        public virtual bool Add_Task_To_List(Task task)
        {
            _tasks.Add(task);
            return true; //Task added
        }

        public void Display_Tasks()
        {
            if (_tasks != null)
                foreach (var task in _tasks)
                    Debug.WriteLine($"Desc: '{task.Description}', Notes: '{task.Notes}', Task Status: {task.Task_Status}, Creation Date: {task.Task_Creation_Date}, Completion Date: {task.Task_Completion_Date}, OverDue: {task.Overdue}");
        }

        public void Remove_Completed_Tasks()
        {
            // Only remove completed tasks from list that are not empty
            if (_tasks != null)
            {
                List<Task> filteredList = new List<Task>(); //Create a new list to only store incomplete tasks
                foreach (Task task in _tasks)
                    if (task.Task_Status == false)
                        filteredList.Add(task);

                _tasks = filteredList; //Save the incomplete task list back to the task list
            }
        }
    }
}
