using StoredTaskApp.Enums;
using StoredTaskApp.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace StoredTaskApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            Task my_Task0, my_Task1, my_Task2, my_Task3;
            RepeatingTask my_RepeatingTask0, my_RepeatingTask1, my_RepeatingTask2;
            Habit habit0, habit1, habit2;
            TaskList my_TaskList0, my_TaskList1, my_TaskList2;
            Project preConstructionPhase;
            TaskCollection my_TaskCollection;

            Debug.WriteLine("-----------------------------------------------------------------------------------------------------------------");
            Debug.WriteLine("Testing TaskApp functionality - Started");

            Test_Priority_Functionality();

            Debug.WriteLine("");
            Debug.WriteLine("Test TaskApp Funtionality - Started");
            Debug.WriteLine("");

            Test_TaskApp_Functionality(out my_Task0, out my_Task1, out my_Task2, out my_Task3,
                out my_RepeatingTask0, out my_RepeatingTask1, out my_RepeatingTask2,
                out habit0, out habit1, out habit2,
                out my_TaskList0, out my_TaskList1, out my_TaskList2, out preConstructionPhase, out my_TaskCollection);

            Debug.WriteLine("");
            Debug.WriteLine("Test TaskApp Funtionality - Completed");
            Debug.WriteLine("");

            Debug.WriteLine("");
            Debug.WriteLine("Testing TaskApp functionality - Completed");
            Debug.WriteLine("-----------------------------------------------------------------------------------------------------------------");

        }

        private void Test_Priority_Functionality()
        {

            Debug.WriteLine("");
            Debug.WriteLine("Test Priority functionality - Started");
            Debug.WriteLine("");

            Priority p1 = new Priority();
            Debug.WriteLine(p1.Display_Priority());
            p1++;
            Debug.WriteLine(p1.Display_Priority());
            p1++;
            Debug.WriteLine(p1.Display_Priority());
            p1++;
            Debug.WriteLine(p1.Display_Priority());


            p1--;
            Debug.WriteLine(p1.Display_Priority());
            p1--;
            Debug.WriteLine(p1.Display_Priority());
            p1--;
            Debug.WriteLine(p1.Display_Priority());

            Debug.WriteLine("");
            Debug.WriteLine("Test Priority Functionality - Completed");
            Debug.WriteLine("");
        }

        private void Test_TaskApp_Functionality(out Task task0, out Task task1, out Task task2, out Task task3,
            out RepeatingTask repeatingTask0, out RepeatingTask repeatingTask1, out RepeatingTask repeatingTask2,
            out Habit habit0, out Habit habit1, out Habit habit2,
            out TaskList tasklist0, out TaskList tasklist1, out TaskList tasklist2, out Project project,
            out TaskCollection taskcollection)
        {
            CreateTasks(out task0, out task1, out task2, out task3);

            CreateRepeatingTasks(out repeatingTask0, out repeatingTask1, out repeatingTask2);

            CreateHabitTasks(out habit0, out habit1, out habit2, repeatingTask0, repeatingTask1, repeatingTask2);

            Debug.WriteLine("Creating all TaskList Classes");
            CreateTaskLists(out tasklist0, out tasklist1, out tasklist2, out project,
                task0, task1, task2, task3, repeatingTask0,
                repeatingTask1, repeatingTask2, habit0, habit1, habit2);
            Debug.WriteLine("TaskList creation complete");
            Debug.WriteLine("");

            Debug.WriteLine("Creating TaskCollection Class");
            CreateCollection(out taskcollection, tasklist0, tasklist1, tasklist2, project);
            Debug.WriteLine("TaskCollection creation complete");

            StoreDataAsync(taskcollection);
        }

        private async void StoreDataAsync(TaskCollection taskCollection)
        {
            Debug.WriteLine("Saving the newly created TaskCollection Class to binary file");
            bool savedSuccessfully = await TaskCollectionSerializer.SaveAsync(taskCollection);
            Debug.WriteLine($"The call SaveTaskCollectionAsyn was successfull? {savedSuccessfully}");
            TaskCollection savedTC = TaskCollectionSerializer.LoadAsync(taskCollection);

        }

        private void CreateTasks(out Task task0, out Task task1, out Task task2, out Task task3)
        {
            Debug.WriteLine("");
            Debug.WriteLine("Test Task Class Functionality - Started");
            Debug.WriteLine("");

            Debug.WriteLine("Create Task 1");
            task0 = Add_New_Task("Define project scope, budget and timeline", "");

            Debug.WriteLine("Create Task 2");
            task1 = Add_New_Task("Hire architect and structural engineer", "");

            Debug.WriteLine("Create Task 3");
            task2 = Add_New_Task("Create house design and blueprints", "");

            Debug.WriteLine("Change Task Status for Task 3");
            task2.Change_Task_Status();
            Log_Debug_Message(task2);

            Debug.WriteLine("Create Task 4");
            task3 = Add_New_Task(null, "Test null / blank task");

            Debug.WriteLine("Change Description of Task 4 to a blank");
            task3.Description = ""; //Blank description
            Log_Debug_Message(task3);

            Debug.WriteLine("Change Description of Task 4 to 'Fourth Task'");
            task3.Description = "Obtain necessary permits and approvals";
            Log_Debug_Message(task3);

            Debug.WriteLine("");
            Debug.WriteLine("Test  Task Class Functionality - Completed");
            Debug.WriteLine("");

        }

        private void CreateRepeatingTasks(out RepeatingTask repeattask0, out RepeatingTask repeattask1,
            out RepeatingTask repeattask2)
        {
            Debug.WriteLine("");
            Debug.WriteLine("Test Repeating Task Class Functionality - Started");
            Debug.WriteLine("");

            Debug.WriteLine("Create Repeating Task 1");
            repeattask0 = Add_New_Task("Practice C# Code", "Spend 15 minutes daily to practice C#", RepeatCycle.Daily);
            Log_Debug_Message(repeattask0);
            repeattask0.Change_Task_Status();
            Log_Debug_Message(repeattask0);

            Debug.WriteLine("Create Repeating Task 2");
            repeattask1 = Add_New_Task("Check your finances", "", RepeatCycle.Biweekly);
            Log_Debug_Message(repeattask1);
            repeattask1.Change_Task_Status();
            Log_Debug_Message(repeattask1);

            Debug.WriteLine("Create Repeating Task 3");
            repeattask2 = Add_New_Task("Review you goals", "", RepeatCycle.Annually);
            Log_Debug_Message(repeattask2);
            repeattask2.Change_Task_Status();
            Log_Debug_Message(repeattask2);

            Debug.WriteLine("");
            Debug.WriteLine("Test Repeating Task Class Functionality - Completed");
            Debug.WriteLine("");
        }

        private void CreateHabitTasks(out Habit habit0, out Habit habit1, out Habit habit2,
            RepeatingTask repeatTask0, RepeatingTask repeatTask1, RepeatingTask repeatTask2)
        {
            Debug.WriteLine("");
            Debug.WriteLine("Test Habit Class Funtionality - Started");
            Debug.WriteLine("");

            Debug.WriteLine("Create Habit Task 1");
            habit0 = new Habit("Gym", "45 mins daily", RepeatCycle.Daily);
            Log_Debug_Message(habit0);
            habit0.Change_Task_Status();
            Log_Debug_Message(habit0);

            Debug.WriteLine("Create Habit Task 2");
            habit1 = new Habit("Declutter office", "", RepeatCycle.Biweekly);
            Log_Debug_Message(habit1);
            habit1.Change_Task_Status();
            Log_Debug_Message(habit1);

            Debug.WriteLine("Create Habit Task 3");
            habit2 = new Habit("Update resume", "", RepeatCycle.Annually);
            Log_Debug_Message(habit2);
            habit2.Change_Task_Status();


            Debug.WriteLine("");
            Debug.WriteLine("Test Habit Class Funtionality - Started");
            Debug.WriteLine("");
        }

        private void CreateTaskLists(out TaskList tasklist0, out TaskList tasklist1, out TaskList tasklist2, out Project project,
            Task task0, Task task1, Task task2, Task task3,
            RepeatingTask repeatingTask0, RepeatingTask repeatingTask1, RepeatingTask repeatingTask2,
            Habit habit0, Habit habit1, Habit habit2)
        {
            //Create Tasklist
            tasklist0 = CreateTaskList0(task0, task1, task2, task3, repeatingTask0);

            tasklist1 = CreateTaskList1(task2, task3, habit0, repeatingTask0);

            tasklist2 = CreateTaskList2(task0, task3);

            //Create Project (Tasklist)
            project = CreateProjectTaskList(habit0, repeatingTask0, task0, task1, task2, task3);
        }

        private void CreateCollection(out TaskCollection taskCollection, TaskList taskList0,
            TaskList taskList1, TaskList taskList2, Project project)
        {
            Debug.WriteLine("Create new Task Collection");
            taskCollection = new TaskCollection();
            Log_Debug_Message(taskCollection);

            Debug.WriteLine($"Adding TaskList (Hiten's List) to Collection - Count = {taskList0.Count} & Incomplete count = {taskList0.Count_Of_Incomplete_Tasks}");
            taskCollection.Add_TaskListToCollection(taskList0);
            Log_Debug_Message(taskCollection);

            Debug.WriteLine($"Adding TaskList (Another List) to Collection - Count = {taskList1.Count} & Incomplete count = {taskList1.Count_Of_Incomplete_Tasks}");
            taskCollection.Add_TaskListToCollection(taskList1);
            Log_Debug_Message(taskCollection);

            Debug.WriteLine($"Adding TaskList (Blank List) to Collection - Count = {taskList2.Count} & Incomplete count = {taskList2.Count_Of_Incomplete_Tasks}");
            taskCollection.Add_TaskListToCollection(taskList2);
            Log_Debug_Message(taskCollection);

            Debug.WriteLine("Removing completed tasks from all lists");
            taskCollection.Remove_All_Completed_Tasks();
            Log_Debug_Message(taskCollection);

            Debug.WriteLine($"Adding Project (Pre-Construction Phase) to Collection - Count = {project.Count} & Incomplete count = {project.Count_Of_Incomplete_Tasks}");
            taskCollection.Add_TaskListToCollection(project);
            Log_Debug_Message(taskCollection);
        }

        private TaskList CreateTaskList0(Task task0, Task task1, Task task2, Task task3, RepeatingTask repeatTask)
        {
            string errorMsg = "";
            TaskList tasklist;

            //Create Tasklist
            Debug.WriteLine("Create new TaskList (Hiten's List)");
            tasklist = Add_New_Task_List("Hiten's List");

            Debug.WriteLine("Add Task 1 to TaskList (Hiten's List)");
            errorMsg = "Failed to add Task 1 to TaskList (Hiten's List)";
            tasklist = AddTaskToList(tasklist, task0, errorMsg);

            Debug.WriteLine("Add Task 2 to TaskList (Hiten's List)");
            errorMsg = "Failed to add Task 2 to TaskList (Hiten's List)";
            tasklist = AddTaskToList(tasklist, task1, errorMsg);

            Debug.WriteLine("Add Task 3 to TaskList (Hiten's List)");
            errorMsg = "Failed to add Task 3 to TaskList (Hiten's List)";
            tasklist = AddTaskToList(tasklist, task2, errorMsg);

            Debug.WriteLine("Add Task 4 to TaskList (Hiten's List)");
            errorMsg = "Failed to add Task 4 to TaskList (Hiten's List)";
            tasklist = AddTaskToList(tasklist, task3, errorMsg);

            Debug.WriteLine("Add Repeating Task 1 to Tasklist (Hiten's List)");
            errorMsg = "Failed to add Repeating Task 1 to TaskList (Hiten's List)";
            tasklist = AddTaskToList(tasklist, repeatTask, errorMsg);

            Debug.WriteLine("Displaying all the tasks for Tasklist (Hiten's List)");
            tasklist.Display_Tasks();

            Debug.WriteLine("Remove all completed tasks from TaskList (Hiten's List)");
            tasklist.Remove_Completed_Tasks();
            Log_Debug_Message(tasklist);

            Debug.WriteLine("Displaying all the tasks for Tasklist (Hiten's List) after removing completed tasks");
            tasklist.Display_Tasks();
            Debug.WriteLine("");

            return tasklist;
        }

        private TaskList CreateTaskList1(Task task2, Task task3, Habit habit, RepeatingTask repeatingTask)
        {
            string errorMsg;
            TaskList tasklist;

            Debug.WriteLine("Create new TaskList (Another List)");
            tasklist = Add_New_Task_List("Another List");

            Debug.WriteLine("Add Task 3 to TaskList (Another List)");
            errorMsg = "Failed to add Task 3 to TaskList (Another List)";
            tasklist = AddTaskToList(tasklist, task2, errorMsg);

            Debug.WriteLine("Add Task 4 to TaskList (Another List)");
            errorMsg = "Failed to add Task 4 to TaskList (Another List)";
            tasklist = AddTaskToList(tasklist, task3, errorMsg);

            Debug.WriteLine("Add a Habit Task to TaskList (Another List)");
            errorMsg = "Failed to add Task 4 to TaskList (Another List)";
            tasklist = AddTaskToList(tasklist, habit, errorMsg);

            Debug.WriteLine("Add a Repeating Task 4 to TaskList (Another List)");
            errorMsg = "Failed to add Task 4 to TaskList (Another List)";
            tasklist = AddTaskToList(tasklist, repeatingTask, errorMsg);

            Debug.WriteLine("Displaying all the tasks for Tasklist (Another List)");
            tasklist.Display_Tasks();
            Debug.WriteLine("");

            return tasklist;

        }

        private TaskList CreateTaskList2(Task task0, Task task3)
        {
            string errorMsg;
            TaskList tasklist;

            // Create Tasklist
            Debug.WriteLine("Create new TaskList (Blank List)");
            tasklist = Add_New_Task_List(null);

            Debug.WriteLine("Create new TaskList (Blank List)");
            tasklist.Name = "";
            Log_Debug_Message(tasklist);

            Debug.WriteLine("Add Task 1 to TaskList (Blank List)");
            errorMsg = "Failed to add Task 1 to TaskList (Blank List)";
            tasklist = AddTaskToList(tasklist, task0, errorMsg);

            Debug.WriteLine("Add Task 4 to TaskList (Blank List)");
            errorMsg = "Failed to add Task 4 to TaskList (Blank List)";
            tasklist = AddTaskToList(tasklist, task3, errorMsg);

            Debug.WriteLine("Displaying all the tasks for Tasklist (Blank List)");
            tasklist.Display_Tasks();
            Debug.WriteLine("");

            return tasklist;

        }

        private Project CreateProjectTaskList(Habit habit0, RepeatingTask repeatingTask0,
            Task task0, Task task1, Task task2, Task task3)
        {
            string errorMsg;
            Project project;

            //Create project
            Debug.WriteLine("Create new Project (Pre-Construction Phase)");
            project = Add_New_Project("Pre-Construction Phase");

            Debug.WriteLine("Add a Habit to Project - This should return an error msg!");
            errorMsg = $"Failed to add a Habit Task - {habit0.Description}";
            project = AddTaskToList(project, habit0, errorMsg);

            Debug.WriteLine("Add a Repeating Task to Project - This should return an error msg!");
            errorMsg = $"Failed to add a Repeating Task - {repeatingTask0.Description}";
            project = AddTaskToList(project, repeatingTask0, errorMsg);

            Debug.WriteLine("Add Task 1 to Project (Pre-Construction Phase)");
            errorMsg = $"Failed to add a Task - {task0.Description}";
            project = AddTaskToList(project, task0, errorMsg);

            Debug.WriteLine("Add Task 2 to Project (Pre-Construction Phase)");
            errorMsg = $"Failed to add a Task - {task1.Description}";
            project = AddTaskToList(project, task1, errorMsg);

            Debug.WriteLine("Add Task 3 to Project (Pre-Construction Phase)");
            errorMsg = $"Failed to add a Task - {task2.Description}";
            project = AddTaskToList(project, task2, errorMsg);

            Debug.WriteLine("Add Task 4 to Project (Pre-Construction Phase)");
            errorMsg = $"Failed to add a Task - {task3.Description}";
            project = AddTaskToList(project, task3, errorMsg);

            Debug.WriteLine("Displaying all the tasks for Project (Pre-Construction Phase)");
            project.Display_Tasks();
            Debug.WriteLine("");

            return project;
        }

        private Task Add_New_Task(string description, string notes)
        {
            Task myTask = new Task(description, notes);
            Log_Debug_Message(myTask);
            return myTask;
        }

        private RepeatingTask Add_New_Task(string description, string notes, RepeatCycle repeatCycle)
        {
            RepeatingTask myTask = new RepeatingTask(description, notes, repeatCycle);
            Log_Debug_Message(myTask);
            return myTask;
        }

        private TaskList Add_New_Task_List(string name)
        {
            TaskList myTaskList = new TaskList(name);
            Log_Debug_Message(myTaskList);
            return myTaskList;
        }

        private Project Add_New_Project(string name)
        {
            Project myProject = new Project(name);
            Log_Debug_Message(myProject);
            return myProject;
        }

        private TaskList AddTaskToList(TaskList taskList, Task task, string errorMsg)
        {
            if (taskList.Add_Task_To_List(task))
            {
                Log_Debug_Message(taskList);
            }
            else
            {
                Debug.WriteLine(errorMsg);
            }
            return taskList;
        }

        private TaskList AddTaskToList(TaskList taskList, Habit habit, string errorMsg)
        {
            if (taskList.Add_Task_To_List(habit))
            {
                Log_Debug_Message(taskList);
            }
            else
            {
                Debug.WriteLine(errorMsg);
            }
            return taskList;
        }

        private TaskList AddTaskToList(TaskList taskList, RepeatingTask repeatTask, string errorMsg)
        {
            if (taskList.Add_Task_To_List(repeatTask))
            {
                Log_Debug_Message(taskList);
            }
            else
            {
                Debug.WriteLine(errorMsg);
            }
            return taskList;
        }

        private Project AddTaskToList(Project project, Task task, string errorMsg)
        {
            if (project.Add_Task_To_List(task))
            {
                Log_Debug_Message(project);
            }
            else
            {
                Debug.WriteLine(errorMsg);
            }
            return project;
        }

        private Project AddTaskToList(Project project, Habit habit, string errorMsg)
        {
            if (project.Add_Task_To_List(habit))
            {
                Log_Debug_Message(project);
            }
            else
            {
                Debug.WriteLine(errorMsg);
            }
            return project;
        }

        private Project AddTaskToList(Project project, RepeatingTask repeatTask, string errorMsg)
        {
            if (project.Add_Task_To_List(repeatTask))
            {
                Log_Debug_Message(project);
            }
            else
            {
                Debug.WriteLine(errorMsg);
            }
            return project;
        }

        private void Log_Debug_Message(Task task)
        {
            Debug.WriteLine("Task object");
            Debug.WriteLine($"Description: '{task.Description}', Notes: '{task.Notes}', Task Priority: {task.Task_Priority.Display_Priority()}, Task Status: {task.Task_Status}, Task Creation Date: {task.Task_Creation_Date}");
            Debug.WriteLine("");
        }

        private void Log_Debug_Message(RepeatingTask repeatingTask)
        {
            Debug.WriteLine("RepeatingTask object");
            Debug.WriteLine($"Description: '{repeatingTask.Description}', Notes: '{repeatingTask.Notes}', Task Priority: {repeatingTask.Task_Priority.Display_Priority()}, Task Repeat Cycle: {repeatingTask.RepeatCyclePeriod}, Task Status: {repeatingTask.Task_Status}, Task Creation Date: {repeatingTask.Task_Creation_Date}, Task Completion Date: {repeatingTask.Task_Completion_Date}, Task Overdue: {repeatingTask.Overdue}");
            Debug.WriteLine("");

        }

        private void Log_Debug_Message(Habit habit)
        {
            Debug.WriteLine("Habit object");
            Debug.WriteLine($"Description: '{habit.Description}', Notes: '{habit.Notes}', Habit Streak Count: {habit.StreakCount}, Task Priority: {habit.Task_Priority.Display_Priority()}, Task Repeat Cycle: {habit.RepeatCyclePeriod}, Task Status: {habit.Task_Status}, Task Creation Date: {habit.Task_Creation_Date}, Task Completion Date: {habit.Task_Completion_Date}, Task Overdue: {habit.Overdue}");
            Debug.WriteLine("");
        }

        private void Log_Debug_Message(TaskList taskList)
        {
            Debug.WriteLine($"Name of my list: '{taskList.Name}', List count: {taskList.Count}, Incomplete task count: {taskList.Count_Of_Incomplete_Tasks}");
            Debug.WriteLine("");
        }

        private void Log_Debug_Message(Project project)
        {
            Debug.WriteLine($"Project List - All Task Count: {project.Count}, Incomplete Task Count: {project.Count_Of_Incomplete_Tasks}, Project Completion %: {project.CompletionPercentage}");
            Debug.WriteLine("");
        }

        private void Log_Debug_Message(TaskCollection taskCollection)
        {
            Debug.WriteLine($"Task Collection - All Task Count: {taskCollection.Count}, Incomplete Task Count: {taskCollection.Count_Of_Incomplete_Tasks}");
            Debug.WriteLine("");
        }

    }
}
