using StoredTaskApp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace StoredTaskApp.Model
{
    /// <summary>
    /// Habits. A habit is a sub-class of the repeating task which keeps track of how long you have
    /// successfully been completing the task.For example, if you have a habit task for exercise and
    /// you have exercised every day for five days, it would know you have a streak of five days.If you
    /// miss a day, the streak is broken and resets to zero
    /// </summary>
    public class Habit : RepeatingTask
    {
        int _streakCount;
        private DateTime? _lastCompletionDate;

        public Habit(string description, string notes, RepeatCycle repeatCyclePeriod) : base(description, notes, repeatCyclePeriod)
        {
            _streakCount = 0;
            _lastCompletionDate = null;
        }

        public override void Change_Task_Status()
        {
            if (this.Task_Status == false)
            {
                DateTime today = DateTime.Now.Date;

                if (_lastCompletionDate == null || base.Next_Completion_Date() == today)
                {
                    _streakCount++;
                }
                else
                {
                    _streakCount = 0;
                }
                _lastCompletionDate = today;
                base.Change_Task_Status();
            }
        }

        public int StreakCount
        {
            get
            {
                return _streakCount;
            }
        }

        public string LastCompletionDate
        {
            get
            {
                return _lastCompletionDate.ToString();
            }
        }
    }

}