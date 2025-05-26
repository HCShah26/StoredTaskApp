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
        private const bool debugFlag = false; //Change this flag True displays log, False does not display log
        public static async Task<bool> DoesFileExist()
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;

            bool fileExists = false;

            try
            {
                StorageFile file = await storageFolder.GetFileAsync(Filename);
                fileExists = true;
            }
            catch (FileNotFoundException)
            {
                fileExists = false;
            }

            return fileExists;
        }

        public static async Task<bool> SaveAsync(TaskCollection taskCollection)
        {
            bool SavedSuccessFully = false;
            try
            {
                StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                StorageFile file = await storageFolder.CreateFileAsync(Filename, CreationCollisionOption.ReplaceExisting);
                Debug.WriteLineIf(debugFlag, $"File location: {storageFolder.Path}");

                using (var stream = System.IO.File.Open(file.Path, FileMode.Create))
                {
                    using (var writer = new BinaryWriter(stream, Encoding.UTF8, true))
                    {
                        writer.Write(taskCollection.TaskLists.Count); // First item stored in the binary file is the count of lists stored in the TaskCollection class
                        Debug.WriteLineIf(debugFlag, $"This collection has {taskCollection.TaskLists.Count} lists");
                        foreach (var taskList in taskCollection.TaskLists)
                        {
                            writer.Write(taskList.GetType().ToString());
                            writer.Write(taskList.Count);
                            writer.Write(taskList.Name);

                            Debug.WriteLineIf(debugFlag, $"Tasklist type is '{taskList.GetType().ToString()}'");
                            Debug.WriteLineIf(debugFlag, $"TaskList count is {taskList.Count}");
                            Debug.WriteLineIf(debugFlag,$"TaskList Name is {taskList.Name}");

                            foreach (var taskitem in taskList.ReturnTasks.ToList())
                            //foreach (var taskitem in (List<Task>)taskList.ReturnTasks)
                                {
                                writer.Write(taskitem.GetType().ToString());
                                Debug.WriteLineIf(debugFlag, $"  Task type is '{taskitem.GetType().ToString()}'");
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

                                        Debug.WriteLineIf(debugFlag, $"Task");
                                        Debug.WriteLineIf(debugFlag, $"      Task Data Information Description -     {currentTask.Description}");
                                        Debug.WriteLineIf(debugFlag, $"      Task Data Information Notes -           {currentTask.Notes}");
                                        Debug.WriteLineIf(debugFlag, $"      Task Data Information Status -          {currentTask.Task_Status}");
                                        Debug.WriteLineIf(debugFlag, $"      Task Data Information Priority -        {currentTask.Task_Priority.Value}");
                                        Debug.WriteLineIf(debugFlag, $"      Task Data Information Creation Date -   {currentTask.Task_Creation_Date}");
                                        Debug.WriteLineIf(debugFlag, $"      Task Data Information Completion Date - {currentTask.Task_Completion_Date}");
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

                                        Debug.WriteLineIf(debugFlag, $"RepeatingTask");
                                        Debug.WriteLineIf(debugFlag, $"      Repeating Task Data Information Description -       {currentTask.Description}");
                                        Debug.WriteLineIf(debugFlag, $"      Repeating Task Data Information Notes -             {currentTask.Notes}");
                                        Debug.WriteLineIf(debugFlag, $"      Repeating Task Data Information Status -            {currentTask.Task_Status}");
                                        Debug.WriteLineIf(debugFlag, $"      Repeating Task Data Information Priority -          {currentTask.Task_Priority.Value}");
                                        Debug.WriteLineIf(debugFlag, $"      Repeating Task Data Information Creation Date -     {currentTask.Task_Creation_Date}");
                                        Debug.WriteLineIf(debugFlag, $"      Repeating Task Data Information Completion Date -   {currentTask.Task_Completion_Date}");
                                        Debug.WriteLineIf(debugFlag, $"      Repeating Task Data Information Repeat Cycle -      {(int)currentTask.RepeatCyclePeriod}");
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

                                        Debug.WriteLineIf(debugFlag, $"Habit");
                                        Debug.WriteLineIf(debugFlag, $"      Habit Task Data Information Description -       {currentTask.Description}");
                                        Debug.WriteLineIf(debugFlag, $"      Habit Task Data Information Notes -             {currentTask.Notes}");
                                        Debug.WriteLineIf(debugFlag, $"      Habit Task Data Information Status -            {currentTask.Task_Status}");
                                        Debug.WriteLineIf(debugFlag, $"      Habit Task Data Information Priority -          {currentTask.Task_Priority.Value}");
                                        Debug.WriteLineIf(debugFlag, $"      Habit Task Data Information Creation Date -     {currentTask.Task_Creation_Date}");
                                        Debug.WriteLineIf(debugFlag, $"      Habit Task Data Information Completion Date -   {currentTask.Task_Completion_Date}");
                                        Debug.WriteLineIf(debugFlag, $"      Habit Task Date Information Repeat Cycle -      {(int)currentTask.RepeatCyclePeriod}");
                                        Debug.WriteLineIf(debugFlag, $"      Habit Task Data Information Streak Count -      {currentTask.StreakCount}");
                                        Debug.WriteLineIf(debugFlag, $"      Habit Task Data Information Last Comp. Date-    {currentTask.LastCompletionDate}");
                                        break;
                                    }
                                }
                            }
                        }
                        writer.Dispose();
                        SavedSuccessFully = true;
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

        public static async Task<TaskCollection> LoadAsync() //The input TaskCollection is only temporary measure whilst the Load Method is built!!!
        {
            try
            {
                Debug.WriteLineIf(debugFlag, "Attempting to read myFile.bin");
                StorageFolder storageFolder = ApplicationData.Current.LocalFolder;

                Debug.WriteLineIf(debugFlag, $"Folder location to read the file: {storageFolder.Path}\\{Filename}");

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

                StoredTaskApp.Model.TaskCollection savedTaskCollection = null;

                using (var stream = System.IO.File.Open(storageFolder.Path + "\\" + Filename, FileMode.Open))
                {
                    Debug.WriteLineIf(debugFlag, $"The content of stream: {stream.Length}");

                    using (var reader = new BinaryReader(stream, Encoding.UTF8, false))
                    { 
                        //Create  a new TaskCollection
                        savedTaskCollection = new TaskCollection();

                        int taskCollection_count = reader.ReadInt32(); //Stores the number of TaskLists in the TaskCollection

                        for (int tc_count = 0; tc_count < taskCollection_count; tc_count++)
                        {
                            StoredTaskApp.Model.TaskList taskList = null;
                            StoredTaskApp.Model.Project project = null;

                            StoredTaskApp.Model.Task tempTask;
                            StoredTaskApp.Model.RepeatingTask tempRepeatingTask = null;
                            StoredTaskApp.Model.Habit tempHabit = null;

                            string tList_Type = reader.ReadString(); //Stores the Type of TaskList (TaskList or Project)
                            int tasklist_count = reader.ReadInt32(); //Stores the number of different tasks there are in this tasklist or project
                            string tlist_Name = reader.ReadString(); //Store the name of this Tasklist or Project;

                            Debug.WriteLineIf(debugFlag, $"Tasklist Type: {tList_Type}");
                            Debug.WriteLineIf(debugFlag, $"The Task Collection has {taskCollection_count} Task Lists");
                            Debug.WriteLineIf(debugFlag, $"This TaskList name: {tlist_Name}");

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
                                    switch (taskType)
                                    {
                                        case "StoredTaskApp.Model.Task":
                                            {
                                                project.Add_Task_To_List(tempTask);
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
                                else
                                {
                                    switch (taskType)
                                    {
                                        case "StoredTaskApp.Model.Task":
                                            {
                                                taskList.Add_Task_To_List(tempTask);
                                                break;
                                            }
                                        case "StoredTaskApp.Model.RepeatingTask":
                                            {
                                                taskList.Add_Task_To_List(tempRepeatingTask);
                                                break;
                                            }
                                        case "StoredTaskApp.Model.Habit":
                                            {
                                                taskList.Add_Task_To_List(tempHabit);
                                                break;
                                            }
                                    }
                                }

                                Debug.WriteLineIf(debugFlag, "Task object created");
                                Debug.WriteLineIf(debugFlag, $"Task Description      : {tempTask.Description}");
                                Debug.WriteLineIf(debugFlag, $"Task Notes            : {tempTask.Notes}");
                                Debug.WriteLineIf(debugFlag, $"Task Status           : {tempTask.Task_Status}");
                                Debug.WriteLineIf(debugFlag, $"Task Priority         : {tempTask.Task_Priority}");
                                Debug.WriteLineIf(debugFlag, $"Task Creation Date    : {tempTask.Task_Creation_Date}");
                                Debug.WriteLineIf(debugFlag, $"Task Completion Date  : {tempTask.Task_Completion_Date}");
                                if ( taskType == "StoredTaskApp.Model.RepeatingTask")
                                {
                                    Debug.WriteLineIf(debugFlag, $"Task Repeat Cycle     : {tempRepeatingTask.RepeatCyclePeriod}");
                                }
                                else if ( taskType == "StoredTaskApp.Model.Habit")
                                {
                                    Debug.WriteLineIf(debugFlag, $"Task Repeat Cycle     : {tempHabit.RepeatCyclePeriod}");
                                    Debug.WriteLineIf(debugFlag, $"Task Streak Count     : {tempHabit.StreakCount}");
                                    Debug.WriteLineIf(debugFlag, $"Task Last Completion  : {tempHabit.LastCompletionDate}");
                                }
                            }
                            // Add currentTaskList to savedTaskCollection
                            if (project != null)
                            {
                                savedTaskCollection.Add_TaskListToCollection(project);
                            }

                            if (taskList != null)
                            {
                                savedTaskCollection.Add_TaskListToCollection(taskList);
                            }
                        }
                    }
                }
                return savedTaskCollection;
            }
            catch (Exception e)
            {
                Debug.WriteLine("An error occured trying to read myFile.bin");
                Debug.WriteLine(e.Message);
                return null;
            }
        }
    }
}
