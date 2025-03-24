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

                using (var stream = File.Open(file.Path, FileMode.Create))
                {
                    using (var writer = new BinaryWriter(stream, Encoding.UTF8))
                    {
                        writer.Write(taskCollection.Count); // First item stored in the binary file is the count of lists stored in the TaskCollection class
                        Debug.WriteLine($"This collection has {taskCollection.Count} lists");
                        foreach (var taskList in taskCollection.TaskLists)
                        {
                            writer.Write(taskList.GetType().ToString());
                            writer.Write(taskList.Count);

                            Debug.WriteLine($"Tasklist type is '{taskList.GetType().ToString()}'");
                            Debug.WriteLine($"TaskList count is {taskList.Count}");

                            foreach (var taskitem in (List<Task>)taskList.ReturnTasks)
                            {
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

                                            Debug.WriteLine($"RepeatingTask");
                                            Debug.WriteLine($"      Repeating Task Data Information Description -       {currentTask.Description}");
                                            Debug.WriteLine($"      Repeating Task Data Information Notes -             {currentTask.Notes}");
                                            Debug.WriteLine($"      Repeating Task Data Information Status -            {currentTask.Task_Status}");
                                            Debug.WriteLine($"      Repeating Task Data Information Priority -          {currentTask.Task_Priority.Value}");
                                            Debug.WriteLine($"      Repeating Task Data Information Creation Date -     {currentTask.Task_Creation_Date}");
                                            Debug.WriteLine($"      Repeating Task Data Information Completion Date -   {currentTask.Task_Completion_Date}");
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
                                            Debug.WriteLine($"      Habit Task Data Information Completion Date-    {currentTask.Task_Completion_Date}");
                                            Debug.WriteLine($"      Habit Task Data Information Streak Count -      {currentTask.StreakCount}");
                                            Debug.WriteLine($"      Habit Task Data Information Last Comp. Date-    {currentTask.LastCompletionDate}");
                                            //SavedSuccessFully = SaveHabit((Habit)taskitem, writer);
                                            break;
                                        }
                                }
                            }
                        }
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
