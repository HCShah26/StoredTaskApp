using System;
using System.Diagnostics;
using StoredTaskApp.Enums;

namespace StoredTaskApp.Model
{
    /// <summary>
    /// Create a project class that inherits from your existing class list class. The difference between
    /// a list and a project is that the tasks in a project are all tasks needed to complete the project.For
    /// example, a user could have a “Paint bedroom” project which contains the “decide on colour”, “buy
    /// paint”, “sand walls” and “paint room” tasks.
    /// 
    /// While the project class is used differently to a normal task list, it is almost identical. 
    /// There are three differences.
    ///     • A calculated property that returns the percentage of the project that is complete.
    ///     • Habits are not allowed in projects.
    ///     • Repeating tasks are not allowed in projects.
    /// </summary>
    public class Project : TaskList
    {
        public Project(string name) : base(name)
        {
        }

        public string Project_Name
        {
            get
            {
                return this.Name;
            }
        }

        public override bool Add_Task_To_List(Task task)
        {
            bool taskAdded = false;
            if (task is RepeatingTask || task is Habit)
            {
                //Not sure whether to raise an exception error or to return a result which can be interpreted as error
                //by the calling function. 
                // I personally prefer the latter as it doesn't throw an error, leaving the user confused.

                //Option 1 throw an exceptional error
                //throw new InvalidOperationException("Projects cannot be repeating tasks or habits");

                //Option 2 (Preferred option)  return a false and log the error message
                Debug.WriteLine("Projects cannot be repeating tasks or habits");
                taskAdded = false;

            }
            else
            {
                base.Add_Task_To_List(task);
                taskAdded = true;
            }
            return taskAdded;
        }

        public override string Name
        {
            get
            {
                return base.Name;
            }

            set
            {
                if (value != string.Empty && value != null)
                    base.Name = value;
                else
                    //We shouldn't get here as this error should be handled in the View
                    //If in any case we get here, instead of sending an error message, we will 
                    //set the name value as "New Project {Todays Date}"

                    //Before setting the description value, check if it's empty before assigning the "New Task {Today's date}"
                    if (base.Name == string.Empty)
                    base.Name = "New Project " + DateTime.Now.Date.ToString();
            }
        }


        //Calculate percentage of completed tasks in project
        public double CompletionPercentage
        {
            get
            {
                if (base.Count == 0)
                {
                    return 0; // This is to avoid division by 0 error
                }
                else
                {
                    int completedTasks = base.Count - base.Count_Of_Incomplete_Tasks; //Calculate the no. of completed tasks
                    return ((double)completedTasks / base.Count) * 100; //Calculate the percentage of tasks completed
                }
            }
        }
    }
}

