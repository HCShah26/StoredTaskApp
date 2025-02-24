using StoredTaskApp.Enums;
using System;

namespace StoredTaskApp.Model
{
    /// <summary>
    /// This is an inherited class RepeatingTask
    /// It will have additional property called RepeatCycle
    /// A method to recalculate the next cycle date when the task is completed
    /// </summary>
    /// 
    public class RepeatingTask : Task
    {
        private RepeatCycle _repeatCyclePeriod;

        public RepeatingTask(string description, string notes, RepeatCycle repeatCyclePeriod) : base(description, notes)
        {
            _repeatCyclePeriod = repeatCyclePeriod;
        }

        //Overloaded method to allowing loading saved data from file
        public RepeatingTask(string description, string notes, bool task_status, Priority task_priority, DateTime task_creation_date, DateTime? task_completion_date, RepeatCycle repeatCyclePeriod) : base(description, notes, task_status, task_priority, task_creation_date, task_completion_date)
        {
            _repeatCyclePeriod = repeatCyclePeriod;
        }

        public RepeatCycle RepeatCyclePeriod
        {
            get
            {
                return _repeatCyclePeriod;
            }
        }

        // This overrider method Change Task Status does the following
        // 1. Changes the task status
        // 2. Checks if the new status is true (meaning completed task)
        //    a. If the task is completed, then recalculate the new completion date
        //       and reset the task_status back to false as the new cycle has started
        public override void Change_Task_Status()
        {

            base.Change_Task_Status();  //Change the Task Status
            if (this.Task_Status == true)
            {
                this.Task_Completion_Date = Next_Completion_Date(); //Change completion date to new completion date
                base.Change_Task_Status(); //Reset task status to false (Incomplete task)
            }
        }


        public DateTime Next_Completion_Date()
        {
            DateTime tempDt;

            //Check if Task_Completion_Date has been set?
            //If true then assign this date to a temp date field
            //else set temp date as today
            if (this.Task_Completion_Date != null)
            {
                tempDt = (DateTime)this.Task_Completion_Date;
            }
            else
            {
                tempDt = DateTime.Now.Date;
            }

            switch (_repeatCyclePeriod)
            {
                case RepeatCycle.Daily:
                    tempDt = tempDt.AddDays((int)RepeatCycle.Daily);
                    break;
                case RepeatCycle.Weekly:
                    tempDt = tempDt.AddDays((int)RepeatCycle.Weekly);
                    break;
                case RepeatCycle.Biweekly:
                    tempDt = tempDt.AddDays((int)RepeatCycle.Biweekly);
                    break;
                case RepeatCycle.Monthly:
                    tempDt = tempDt.AddMonths(1);
                    break;
                case RepeatCycle.BiMonthly:
                    tempDt = tempDt.AddMonths(2);
                    break;
                case RepeatCycle.Quarterly:
                    tempDt = tempDt.AddMonths(3);
                    break;
                case RepeatCycle.Biannually:
                    tempDt = tempDt.AddMonths(6);
                    break;
                case RepeatCycle.Annually:
                    tempDt = tempDt.AddYears(1);
                    break;
                default:
                    //Shouldn't get here
                    break;
            }
            return tempDt;
        }
    }

}

