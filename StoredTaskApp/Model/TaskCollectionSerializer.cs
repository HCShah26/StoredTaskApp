using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using System.Diagnostics;
using System.Linq.Expressions;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using static System.Net.WebRequestMethods;
using StoredTaskApp.Enums;


namespace StoredTaskApp.Model
{
    public static class TaskCollectionSerializer
    {
        private const string Filename = "myFile.bin";

        public static async Task<bool> SaveAsync(TaskCollection taskCollection)
        {
            bool SavedSuccessFully = false;
            try
            {
                StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                StorageFile file = await storageFolder.CreateFileAsync(Filename, CreationCollisionOption.ReplaceExisting);
                Debug.WriteLine($"File location: {storageFolder.Path}");

                using (var stream = System.IO.File.Open(file.Path, FileMode.Create))
                {
                    using (var writer = new BinaryWriter(stream, Encoding.UTF8, true))
                    {
                        writer.Write(taskCollection.TaskLists.Count); // First item stored in the binary file is the count of lists stored in the TaskCollection class
                        Debug.WriteLine($"This collection has {taskCollection.TaskLists.Count} lists");
                        foreach (var taskList in taskCollection.TaskLists)
                        {
                            writer.Write(taskList.GetType().ToString());
                            writer.Write(taskList.Count);
                            writer.Write(taskList.Name);

                            Debug.WriteLine($"Tasklist type is '{taskList.GetType().ToString()}'");
                            Debug.WriteLine($"TaskList count is {taskList.Count}");
                            Debug.WriteLine($"TaskList Name is {taskList.Name}");

                            foreach (var taskitem in (List<Task>)taskList.ReturnTasks)
                            {
                                writer.Write(taskitem.GetType().ToString());
                                Debug.WriteLine($"  Task type is '{taskitem.GetType().ToString()}'");
                                switch (taskitem.GetType().ToString())
                                {
                                    case "StoredTaskApp.Model.Task":
                                        {
                                            StoredTaskApp.Model.Task currentTask = (StoredTaskApp.Model.Task)taskitem;
                                            writer.Write(currentTask.Description);
                                            writer.Write(currentTask.Notes);
                                            writer.Write(currentTask.Task_Status);
                                            writer.Write(currentTask.Task_Priority.Value);
                                            writer.Write(currentTask.Task_Creation_Date.Ticks);
                                            writer.Write(currentTask.Task_Completion_Date.HasValue);
                                            if (currentTask.Task_Completion_Date.HasValue)
                                            {
                                                writer.Write(currentTask.Task_Completion_Date.Value.Ticks);
                                            }

                                            Debug.WriteLine($"Task");
                                            Debug.WriteLine($"      Task Data Information Description -     {currentTask.Description}");
                                            Debug.WriteLine($"      Task Data Information Notes -           {currentTask.Notes}");
                                            Debug.WriteLine($"      Task Data Information Status -          {currentTask.Task_Status}");
                                            Debug.WriteLine($"      Task Data Information Priority -        {currentTask.Task_Priority.Value}");
                                            Debug.WriteLine($"      Task Data Information Creation Date -   {currentTask.Task_Creation_Date}");
                                            Debug.WriteLine($"      Task Data Information Completion Date - {currentTask.Task_Completion_Date}");
                                            //SavedSuccessFully = SaveTask(taskitem, writer);
                                            break;
                                        }
                                    case "StoredTaskApp.Model.RepeatingTask":
                                        {
                                            StoredTaskApp.Model.RepeatingTask currentTask = (StoredTaskApp.Model.RepeatingTask)taskitem;
                                            writer.Write(currentTask.Description);
                                            writer.Write(currentTask.Notes);
                                            writer.Write(currentTask.Task_Status);
                                            writer.Write(currentTask.Task_Priority.Value);
                                            writer.Write(currentTask.Task_Creation_Date.Ticks);
                                            writer.Write(currentTask.Task_Completion_Date.HasValue);
                                            if (currentTask.Task_Completion_Date.HasValue)
                                            {
                                                writer.Write(currentTask.Task_Completion_Date.Value.Ticks);
                                            }
                                            writer.Write((int)currentTask.RepeatCyclePeriod);

                                            Debug.WriteLine($"RepeatingTask");
                                            Debug.WriteLine($"      Repeating Task Data Information Description -       {currentTask.Description}");
                                            Debug.WriteLine($"      Repeating Task Data Information Notes -             {currentTask.Notes}");
                                            Debug.WriteLine($"      Repeating Task Data Information Status -            {currentTask.Task_Status}");
                                            Debug.WriteLine($"      Repeating Task Data Information Priority -          {currentTask.Task_Priority.Value}");
                                            Debug.WriteLine($"      Repeating Task Data Information Creation Date -     {currentTask.Task_Creation_Date}");
                                            Debug.WriteLine($"      Repeating Task Data Information Completion Date -   {currentTask.Task_Completion_Date}");
                                            Debug.WriteLine($"      Repeating Task Data Information Repeat Cycle -      {(int)currentTask.RepeatCyclePeriod}");
                                            //SavedSuccessFully = SaveRepeatingTask((RepeatingTask)taskitem, writer);
                                            break;
                                        }
                                    case "StoredTaskApp.Model.Habit":
                                        {
                                            StoredTaskApp.Model.Habit currentTask = (StoredTaskApp.Model.Habit)taskitem;
                                            writer.Write(currentTask.Description);
                                            writer.Write(currentTask.Notes);
                                            writer.Write(currentTask.Task_Status);
                                            writer.Write(currentTask.Task_Priority.Value);
                                            writer.Write(currentTask.Task_Creation_Date.Ticks);
                                            writer.Write(currentTask.Task_Completion_Date.HasValue);
                                            if (currentTask.Task_Completion_Date.HasValue)
                                            {
                                                writer.Write(currentTask.Task_Completion_Date.Value.Ticks); // Converting Task_Completion_Date to long
                                            }
                                            writer.Write((int)currentTask.RepeatCyclePeriod);
                                            writer.Write(currentTask.StreakCount);
                                            if (currentTask.LastCompletionDate == null)
                                            {
                                                writer.Write(false); //This is equivalent to LastCompletionDate.HasValue
                                            }
                                            else
                                            {
                                                writer.Write(true); //This is equivalent to LastCompletionDate.HasValue
                                                                    //Converting the returned string value (LastCompletionDate) to date and storing as long in binary
                                                DateTime dtTemp = DateTime.Parse(currentTask.LastCompletionDate);
                                                writer.Write(dtTemp.Ticks);
                                            }

                                            Debug.WriteLine($"Habit");
                                            Debug.WriteLine($"      Habit Task Data Information Description -       {currentTask.Description}");
                                            Debug.WriteLine($"      Habit Task Data Information Notes -             {currentTask.Notes}");
                                            Debug.WriteLine($"      Habit Task Data Information Status -            {currentTask.Task_Status}");
                                            Debug.WriteLine($"      Habit Task Data Information Priority -          {currentTask.Task_Priority.Value}");
                                            Debug.WriteLine($"      Habit Task Data Information Creation Date -     {currentTask.Task_Creation_Date}");
                                            Debug.WriteLine($"      Habit Task Data Information Completion Date -   {currentTask.Task_Completion_Date}");
                                            Debug.WriteLine($"      Habit Task Date Information Repeat Cycle -      {(int)currentTask.RepeatCyclePeriod}");
                                            Debug.WriteLine($"      Habit Task Data Information Streak Count -      {currentTask.StreakCount}");
                                            Debug.WriteLine($"      Habit Task Data Information Last Comp. Date-    {currentTask.LastCompletionDate}");
                                            //SavedSuccessFully = SaveHabit((Habit)taskitem, writer);
                                            break;
                                        }
                                }
                            }
                        }
                        writer.Dispose();
                    }
                }
                return true; // Saved successfully
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false; // An error occured - file not saved successfully
            }
        }

        public static TaskCollection LoadAsync(TaskCollection taskCollection) //The input TaskCollection is only temporary measure whilst the Load Method is built!!!
        {
            try
            {
                Debug.WriteLine("Attempting to read myFile.bin");
                StorageFolder storageFolder = ApplicationData.Current.LocalFolder;

                Debug.WriteLine($"Folder location to read the file: {storageFolder.Path}\\{Filename}");

                string taskType;
                string task_desc;
                string task_notes;
                bool task_status;
                Priority task_priority;
                DateTime task_Creation_Date;
                bool completion_date_has_value;
                DateTime task_Completion_Date;
                RepeatCycle repeatCycle;
                int streakCount;
                bool lastcompletion_has_value;
                DateTime lastCompletionDate;

                StoredTaskApp.Model.TaskCollection savedTaskCollection;
                StoredTaskApp.Model.TaskList taskList;
                StoredTaskApp.Model.Project project;

                using (var stream = System.IO.File.Open(storageFolder.Path + "\\" + Filename, FileMode.Open))
                {
                    Debug.WriteLine($"The content of stream: {stream.Length}");

                    using (var reader = new BinaryReader(stream, Encoding.UTF8, false))
                    {
                        //Create  a new TaskCollection
                        savedTaskCollection = new TaskCollection();

                        int taskCollection_count = reader.ReadInt32(); //Stores the number of TaskLists in the TaskCollection

                        for (int tc_count = 0; tc_count < taskCollection_count; tc_count++)
                        {
                            string tList_Type = reader.ReadString(); //Stores the Type of TaskList (TaskList or Project)
                            int tasklist_count = reader.ReadInt32(); //Stores the number of different tasks there are in this tasklist or project
                            string tlist_Name = reader.ReadString(); //Store the name of this Tasklist or Project;

                            Debug.WriteLine($"Tasklist Type: {tList_Type}");
                            Debug.WriteLine($"The Task Collection has {taskCollection_count} Task Lists");
                            Debug.WriteLine($"This TaskList name: {tlist_Name}");

                            if (tList_Type == "StoredTaskApp.Model.TaskList")
                            {
                                StoredTaskApp.Model.TaskList currentTaskList = new TaskList(tlist_Name);
                                taskList = currentTaskList;
                            }
                            else if (tList_Type == "StoredTaskApp.Model.Project")
                            {
                                StoredTaskApp.Model.Project currentTaskList = new Project(tlist_Name);
                                project = currentTaskList;
                            }
                            else
                            {
                                //Error has occured!!!
                            }


                            for (int tlist_count = 0; tlist_count < tasklist_count; tlist_count++)
                            {
                                StoredTaskApp.Model.Task tempTask;
                                StoredTaskApp.Model.RepeatingTask tempRepeatingTask;
                                StoredTaskApp.Model.Habit tempHabit;

                                taskType = reader.ReadString();
                                task_desc = reader.ReadString(); // writer.Write(currentTask.Description);
                                task_notes = reader.ReadString(); //writer.Write(currentTask.Notes);
                                task_status = reader.ReadBoolean(); // writer.Write(currentTask.Task_Status);
                                task_priority.Value = reader.ReadInt32(); //writer.Write(currentTask.Task_Priority.Value);
                                task_Creation_Date = new DateTime(reader.ReadInt64());  //writer.Write(currentTask.Task_Creation_Date.Ticks);
                                completion_date_has_value = reader.ReadBoolean(); //writer.Write(currentTask.Task_Completion_Date.HasValue);
                                if (completion_date_has_value)
                                {
                                    task_Completion_Date = new DateTime(reader.ReadInt64()); //writer.Write(currentTask.Task_Completion_Date.Value.Ticks);
                                    tempTask = new Task(task_desc, task_notes, task_status, task_priority, task_Creation_Date, task_Completion_Date);
                                }
                                else
                                {
                                    tempTask = new Task(task_desc, task_notes, task_status, task_priority, task_Creation_Date, null);
                                }
                                if (taskType == "StoredTaskApp.Model.RepeatingTask")
                                {
                                    repeatCycle = (RepeatCycle)reader.ReadInt32(); //writer.Write((int)currentTask.RepeatCyclePeriod);
                                    StoredTaskApp.Model.RepeatingTask currentTask = new RepeatingTask(tempTask.Description, tempTask.Notes, tempTask.Task_Status, tempTask.Task_Priority, tempTask.Task_Creation_Date, tempTask.Task_Completion_Date, repeatCycle);
                                    tempRepeatingTask = currentTask;
                                }
                                else if (taskType == "StoredTaskApp.Model.Habit")
                                {
                                    repeatCycle = (RepeatCycle)reader.ReadInt32(); //writer.Write((int)currentTask.RepeatCyclePeriod);
                                    streakCount = reader.ReadInt32();
                                    lastcompletion_has_value = reader.ReadBoolean();
                                    if (lastcompletion_has_value)
                                    {
                                        lastCompletionDate = new DateTime(reader.ReadInt64());
                                        StoredTaskApp.Model.Habit currentTask = new Habit(tempTask.Description, tempTask.Notes, tempTask.Task_Status, tempTask.Task_Priority, tempTask.Task_Creation_Date, tempTask.Task_Completion_Date, repeatCycle, streakCount, lastCompletionDate);
                                        tempHabit = currentTask;
                                    }
                                    else
                                    {
                                        StoredTaskApp.Model.Habit currentTask = new Habit(tempTask.Description, tempTask.Notes, tempTask.Task_Status, tempTask.Task_Priority, tempTask.Task_Creation_Date, tempTask.Task_Completion_Date, repeatCycle, streakCount, null);
                                        tempHabit = currentTask;
                                    }
                                }
                                else if (taskType == "StoredTaskApp.Model.Task")
                                {
                                    StoredTaskApp.Model.Task currentTask = tempTask;
                                }

                                //Todo create a switch case to store the correct object into var task
                                if (tList_Type == "StoredTaskApp.Model.Project")
                                {
                                    switch (taskType)
                                    {
                                        case "StoredTaskApp.Model.Task":
                                        {
                                            if (project<> null)
                                            {
                                                project.Add_Task_To_List(tempTask);
                                            }
                                            break;
                                        }
                                        case "StoredTaskApp.Model.RepeatingTask":
                                        {
                                            project.Add_Task_To_List(tempRepeatingTask);
                                            break;
                                        }
                                        case "StoredTaskApp.Model.Habit":
                                        {
                                            //This should never happen as Habit can not be in the Project
                                            //project.Add_Task_To_List(tempHabit);
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    switch (taskType)
                                    {
                                //        case "StoredTaskApp.Model.Task":
                                //            {
                                //                taskList.Add_Task_To_List(tempTask);
                                //                break;
                                //            }
                                //        case "StoredTaskApp.Model.RepeatingTask":
                                //            {
                                //                taskList.Add_Task_To_List(tempRepeatingTask);
                                //                break;
                                //            }
                                //        case "StoredTaskApp.Model.Habit":
                                //            {
                                //                taskList.Add_Task_To_List(tempHabit);
                                //                break;
                                //            }
                                    }
                                }

                                Debug.WriteLine("Task object created");
                                Debug.WriteLine($"Task Description      : {tempTask.Description}");
                                Debug.WriteLine($"Task Notes            : {tempTask.Notes}");
                                Debug.WriteLine($"Task Status           : {tempTask.Task_Status}");
                                Debug.WriteLine($"Task Priority         : {tempTask.Task_Priority}");
                                Debug.WriteLine($"Task Creation Date    : {tempTask.Task_Creation_Date}");
                                Debug.WriteLine($"Task Completion Date  : {tempTask.Task_Completion_Date}");
                                
                                if ( taskType == "StoredTaskApp.Model.RepeatingTask")
                                {
                                    Debug.WriteLine($"Task Repeat Cycle     : "); //{tempRepeatingTask.RepeatCyclePeriod}");
                                }
                                else if ( taskType == "StoredTaskApp.Model.Habit")
                                {
                                    Debug.WriteLine($"Task Repeat Cycle     : "); // {tempHabit.RepeatCyclePeriod}");
                                    Debug.WriteLine($"Task Streak Count     : "); // {tempHabit.StreakCount}");
                                    Debug.WriteLine($"Task Last Completion  : "); // {tempHabit.LastCompletionDate}");
                                }
                            }
                        }
                    }
                }
                return taskCollection;
            }
            catch (Exception e)
            {
                Debug.WriteLine("An error occured trying to read myFile.bin");
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        private static bool SaveTask(StoredTaskApp.Model.Task currentTask, BinaryWriter writer)
        {
            try
            {
                using (writer)
                {
                    Debug.WriteLine($"Task");
                    Debug.WriteLine($"      Task Data Information Description -     {currentTask.Description}");
                    Debug.WriteLine($"      Task Data Information Notes -           {currentTask.Notes}");
                    Debug.WriteLine($"      Task Data Information Status -          {currentTask.Task_Status}");
                    Debug.WriteLine($"      Task Data Information Priority -        {currentTask.Task_Priority}");
                    Debug.WriteLine($"      Task Data Information Creation Date -   {currentTask.Task_Creation_Date}");
                    Debug.WriteLine($"      Task Data Information Completion Date - {currentTask.Task_Completion_Date}");

                    
                }
                return true; //Successfully saved Task 
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false; // Failed to successfully save Task
            }
        }

        private static bool SaveRepeatingTask(RepeatingTask currentTask, BinaryWriter writer)
        {
            try
            {
                using (writer)
                {
                    Debug.WriteLine($"RepeatingTask");
                    Debug.WriteLine($"      Repeating Task Data Information Description -       {currentTask.Description}");
                    Debug.WriteLine($"      Repeating Task Data Information Notes -             {currentTask.Notes}");
                    Debug.WriteLine($"      Repeating Task Data Information Status -            {currentTask.Task_Status}");
                    Debug.WriteLine($"      Repeating Task Data Information Priority -          {currentTask.Task_Priority}");
                    Debug.WriteLine($"      Repeating Task Data Information Creation Date -     {currentTask.Task_Creation_Date}");
                    Debug.WriteLine($"      Repeating Task Data Information Completion Date -   {currentTask.Task_Completion_Date}");

                    
                }
                return true; //Successfully saved RepeatingTask
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false; // Failed to successfully save RepeatingTask
            }
        }

        private static bool SaveHabit(Habit currentTask, BinaryWriter writer)
        {
            try
            {
                using (writer)
                {
                    Debug.WriteLine($"Habit");
                    Debug.WriteLine($"      Habit Task Data Information Description -       {currentTask.Description}");
                    Debug.WriteLine($"      Habit Task Data Information Notes -             {currentTask.Notes}");
                    Debug.WriteLine($"      Habit Task Data Information Status -            {currentTask.Task_Status}");
                    Debug.WriteLine($"      Habit Task Data Information Priority -          {currentTask.Task_Priority}");
                    Debug.WriteLine($"      Habit Task Data Information Creation Date -     {currentTask.Task_Creation_Date}");
                    Debug.WriteLine($"      Habit Task Data Information Completion Date-    {currentTask.Task_Completion_Date}");
                    Debug.WriteLine($"      Habit Task Data Information Streak Count -      {currentTask.StreakCount}");
                    Debug.WriteLine($"      Habit Task Data Information Last Comp. Date-    {currentTask.LastCompletionDate}");

                    

                }
                return true; //Successfully saved Habit
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false; // Failed to successfully save Habit
            }
        }
    }
}
