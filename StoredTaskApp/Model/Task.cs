using StoredTaskApp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace StoredTaskApp.Model
{
    /// <summary>
    /// This is the Task Class to store any task to be created.
    /// The task class has 
    ///     1 constructor to add new task given the description and notes details
    ///     6 properties Description, Notes, Task_Status, Priority, Task_Creation_Date & Task_Completion_Date
    ///     1 method Change Task Status
    /// </summary>
    public class Task
    {
        private string _description; //Should not be blank
        private string _notes;
        private bool _task_status; // This indicates whether the task is completed or not
        private Priority _task_priority; // New property added
        private DateTime _task_creation_date; // Readonly field set at initial creation of the task
        private DateTime? _task_completion_date;  // New property added - This property stores the expected completion date which is optional

        public Task(string description, string notes)
        {
            if (description != string.Empty && description != null)
                _description = description;
            else
                //We shouldn't get here as this error should be handled in the View
                //If in any case we get here, instead of sending an error message, we will 
                //set the description value as "New Task {Todays Date}"
                _description = "New Task " + DateTime.Now.ToShortDateString();

            _notes = notes;
            _task_priority = new Priority();
            _task_status = false; //By default setting a new task as incomplete
            _task_creation_date = DateTime.Now;
        }

        //Overloaded method to allow the data to be loaded from a saved file
        public Task(string description, string notes, bool task_status, Priority task_priority, DateTime task_creation_date, DateTime? task_completion_date)
        {
            _description = description;
            _notes = notes;
            _task_status= task_status;
            _task_priority= task_priority;
            _task_creation_date = task_creation_date;
            _task_completion_date = task_completion_date;
        }

        public string Description
        {
            get
            {
                return _description;
            }

            set
            {
                if (value != string.Empty && value != null)
                    _description = value;
                else
                    //We shouldn't get here as this error should be handled in the View
                    //If in any case we get here, instead of sending an error message, we will 
                    //set the description value as "New Task {Todays Date}"

                    //Before setting the description value, check if it's empty before assigning the "New Task {Today's date}"
                    if (_description == string.Empty)
                    _description = "New Task " + DateTime.Now.ToShortDateString();
            }
        }

        public string Notes
        {
            get
            {
                return _notes;
            }

            set
            {
                _notes = value;
            }
        }

        public Priority Task_Priority
        {
            get
            {
                return _task_priority;
            }

            set
            {
                _task_priority = value;
            }
        }

        public bool? Overdue
        {
            get
            {
                bool? result = null;

                if (_task_completion_date != null)
                {
                    if (_task_completion_date > DateTime.Now)
                    {
                        result = false;
                    }
                    else
                    {
                        result = true;
                    }
                }

                return result;
            }
        }

        public virtual bool Task_Status
        {
            get
            {
                return _task_status;
            }
        }

        public DateTime Task_Creation_Date
        {
            get
            {
                return _task_creation_date;
            }
        }

        public DateTime? Task_Completion_Date
        {
            get
            {
                if (_task_completion_date != null)
                {
                    return _task_completion_date;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                _task_completion_date = value;
            }
        }

        public virtual void Change_Task_Status()
        {
            _task_status = !_task_status; //Toggle boolean value
            if (_task_status == true)
            {
                _task_completion_date = DateTime.Now;
            }
        }

    }

}

