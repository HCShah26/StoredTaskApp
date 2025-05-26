using StoredTaskApp.Enums;
using StoredTaskApp.Model;
using StoredTaskApp.Model.Comparer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
        }

        /// <summary>
        /// Called when page is navigated to. Loads existing task collection or creates test data in none exists 
        /// </summary>
        /// <param name="e"></param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            TaskCollection my_TaskCollection = null;

            // Call function to load data from binary file
            my_TaskCollection = await LoadTaskCollection();

            if (my_TaskCollection == null)
            {
                // No data found - create sample data and store it to binary file
                my_TaskCollection = CreateTestData();
                StoreDataAsync(my_TaskCollection);
                DisplayTaskCollection(my_TaskCollection);
            }
            else
            {
                //Display loaded data from binary file
                DisplayTaskCollection(my_TaskCollection);
            }

            // Create an index of all sorted by description

            Debug.WriteLine("Create two indexes for all tasks by priority and description");
            List<StoredTaskApp.Model.Task> priorityIndexForAllTasks = new List<StoredTaskApp.Model.Task>();
            List<StoredTaskApp.Model.Task> descriptionIndexForAllTasks = new List<StoredTaskApp.Model.Task>();
            foreach (TaskList taskList in my_TaskCollection.TaskLists)
            {
                priorityIndexForAllTasks.AddRange(taskList.ReturnTasks.ToList());
                descriptionIndexForAllTasks.AddRange(taskList.ReturnTasks.ToList());
            }

            Debug.WriteLine("Display Priority Index unsorted");
            foreach (var currTask in priorityIndexForAllTasks)
            {
                Log_Debug_Message(currTask);
            }

            // Sort all tasks by Priority
            PriorityComparer priorityComparer = new PriorityComparer();
            priorityIndexForAllTasks.Sort(priorityComparer);

            Debug.WriteLine("Display Priority Index sorted");
            foreach (var currTask in priorityIndexForAllTasks)
            {
                Log_Debug_Message(currTask);
            }

            Debug.WriteLine("Display Description Index unsorted");
            foreach (var currTask in descriptionIndexForAllTasks)
            {
                Log_Debug_Message(currTask);
            }

            // Sort all tasks by Description
            DescriptionComparer descriptionComparer = new DescriptionComparer();
            descriptionIndexForAllTasks.Sort(descriptionComparer);

            Debug.WriteLine("Display Description Index sorted");
            foreach (var currTask in descriptionIndexForAllTasks)
            {
                Log_Debug_Message(currTask);
            }

            // Test Search functionality
            Priority tempPriority = new Priority();
            string dateString = "5/27/2025 12:00:00 AM";
            DateTime parsedDate;

            bool success = DateTime.TryParseExact(dateString, "M/d/yyyy hh:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out parsedDate);
            StoredTaskApp.Model.Task dueDatenPriorityToSearchFor = new Model.Task("", "", false, tempPriority, DateTime.Now, parsedDate);
            var index = descriptionIndexForAllTasks.BinarySearch(dueDatenPriorityToSearchFor);
            //Todo Continue here
        }

        /// <summary>
        /// Create sample test data including Tasks, RepeatingTasks, Habits, TaskList and Project
        /// </summary>
        /// <returns></returns>
        private TaskCollection CreateTestData()
        {
            StoredTaskApp.Model.Task my_Task0, my_Task1, my_Task2, my_Task3;
            RepeatingTask my_RepeatingTask0, my_RepeatingTask1, my_RepeatingTask2;
            Habit habit0, habit1, habit2;
            TaskList my_TaskList0, my_TaskList1, my_TaskList2;
            Project preConstructionPhase;
            TaskCollection my_TaskCollection;

            Debug.WriteLine("-----------------------------------------------------------------------------------------------------------------");
            Debug.WriteLine("Testing TaskApp functionality - Started");

            // Test priority functionality first
            Test_Priority_Functionality();

            Debug.WriteLine("");
            Debug.WriteLine("Test TaskApp Funtionality - Started");
            Debug.WriteLine("");

            // Initialise all objects of the StoredTaskApp.Model
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

            return my_TaskCollection;
        }

        /// <summary>
        /// Tests the increment and decrement behaviour of the Priority Class
        /// </summary>
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

        /// <summary>
        /// This functions will test all the functionality of the StoredTaskApp classes including 
        /// Task, RepeatingTask & Habit objects
        /// TaskList and Project object that will hold a collection of Tasks / RepeatingTasks / Habits
        /// TaskCollection which holds a collection of TaskLists or Projects
        /// </summary>
        /// <param name="task0"></param>
        /// <param name="task1"></param>
        /// <param name="task2"></param>
        /// <param name="task3"></param>
        /// <param name="repeatingTask0"></param>
        /// <param name="repeatingTask1"></param>
        /// <param name="repeatingTask2"></param>
        /// <param name="habit0"></param>
        /// <param name="habit1"></param>
        /// <param name="habit2"></param>
        /// <param name="tasklist0"></param>
        /// <param name="tasklist1"></param>
        /// <param name="tasklist2"></param>
        /// <param name="project"></param>
        /// <param name="taskcollection"></param>
        private void Test_TaskApp_Functionality(out StoredTaskApp.Model.Task task0, out StoredTaskApp.Model.Task task1, 
            out StoredTaskApp.Model.Task task2, out StoredTaskApp.Model.Task task3,
            out RepeatingTask repeatingTask0, out RepeatingTask repeatingTask1, out RepeatingTask repeatingTask2,
            out Habit habit0, out Habit habit1, out Habit habit2,
            out TaskList tasklist0, out TaskList tasklist1, out TaskList tasklist2, out Project project,
            out TaskCollection taskcollection)
        {
            CreateTasks(out task0, out task1, out task2, out task3);

            CreateRepeatingTasks(out repeatingTask0, out repeatingTask1, out repeatingTask2);

            CreateHabitTasks(out habit0, out habit1, out habit2);

            Debug.WriteLine("Creating all TaskList Classes");
            CreateTaskLists(out tasklist0, out tasklist1, out tasklist2, out project,
                task0, task1, task2, task3, repeatingTask0,
                repeatingTask1, repeatingTask2, habit0, habit1, habit2);
            Debug.WriteLine("TaskList creation complete");
            Debug.WriteLine("");

            Debug.WriteLine("Creating TaskCollection Class");
            CreateCollection(out taskcollection, tasklist0, tasklist1, tasklist2, project);
            Debug.WriteLine("TaskCollection creation complete");

            //StoreDataAsync(taskcollection);
        }

        /// <summary>
        /// Saves the TaskCollection object created into a binary file
        /// </summary>
        /// <param name="taskCollection"></param>
        private async void StoreDataAsync(TaskCollection taskCollection)
        {
            Debug.WriteLine("Saving the newly created TaskCollection Class to binary file");
            bool savedSuccessfully = await TaskCollectionSerializer.SaveAsync(taskCollection);
            Debug.WriteLine($"The call SaveTaskCollectionAsyn was successfull? {savedSuccessfully}");
        }

        /// <summary>
        /// Loads the TaskCollection from stored binary file
        /// </summary>
        /// <returns></returns>
        private async Task<StoredTaskApp.Model.TaskCollection> LoadTaskCollection()
        {
            TaskCollection taskCollectionLoaded = null;

            taskCollectionLoaded = await TaskCollectionSerializer.LoadAsync();

            return taskCollectionLoaded;
        }

        /// <summary>
        /// This function will create Task objects
        /// </summary>
        /// <param name="task0"></param>
        /// <param name="task1"></param>
        /// <param name="task2"></param>
        /// <param name="task3"></param>
        private void CreateTasks(out StoredTaskApp.Model.Task task0, out StoredTaskApp.Model.Task task1, 
                                 out StoredTaskApp.Model.Task task2, out StoredTaskApp.Model.Task task3)
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

        /// <summary>
        /// This function will create 3 RepeatTask objects
        /// </summary>
        /// <param name="repeattask0"></param>
        /// <param name="repeattask1"></param>
        /// <param name="repeattask2"></param>
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

        /// <summary>
        /// This function will create 3 Habit objects
        /// </summary>
        /// <param name="habit0"></param>
        /// <param name="habit1"></param>
        /// <param name="habit2"></param>

        private void CreateHabitTasks(out Habit habit0, out Habit habit1, out Habit habit2)
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
            habit2 = new Habit("Update resume", "", RepeatCycle.Weekly);
            Log_Debug_Message(habit2);
            habit2.Change_Task_Status();

            Debug.WriteLine("");
            Debug.WriteLine("Test Habit Class Funtionality - Started");
            Debug.WriteLine("");
        }

        /// <summary>
        /// Creates 3 TaskList objects and uses the Task, RepeatingTask & Habit objects
        /// </summary>
        /// <param name="tasklist0"></param>
        /// <param name="tasklist1"></param>
        /// <param name="tasklist2"></param>
        /// <param name="project"></param>
        /// <param name="task0"></param>
        /// <param name="task1"></param>
        /// <param name="task2"></param>
        /// <param name="task3"></param>
        /// <param name="repeatingTask0"></param>
        /// <param name="repeatingTask1"></param>
        /// <param name="repeatingTask2"></param>
        /// <param name="habit0"></param>
        /// <param name="habit1"></param>
        /// <param name="habit2"></param>
        private void CreateTaskLists(out TaskList tasklist0, out TaskList tasklist1, out TaskList tasklist2, out Project project,
            StoredTaskApp.Model.Task task0, StoredTaskApp.Model.Task task1, StoredTaskApp.Model.Task task2, StoredTaskApp.Model.Task task3,
            RepeatingTask repeatingTask0, RepeatingTask repeatingTask1, RepeatingTask repeatingTask2,
            Habit habit0, Habit habit1, Habit habit2)
        {
            //Create Tasklist
            tasklist0 = CreateTaskList0(task0, task1, task2, task3, repeatingTask0);

            tasklist1 = CreateTaskList1(task0, task1, task2, task3, habit0, habit1, habit2, repeatingTask0);

            tasklist2 = CreateTaskList2(task0, task3);

            //Create Project (Tasklist)
            project = CreateProjectTaskList(habit0, repeatingTask0, task0, task1, task2, task3);
        }

        /// <summary>
        /// Create a TaskCollection object that has 3 TaskList and 1 project object 
        /// </summary>
        /// <param name="taskCollection"></param>
        /// <param name="taskList0"></param>
        /// <param name="taskList1"></param>
        /// <param name="taskList2"></param>
        /// <param name="project"></param>
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
            //taskCollection.Remove_All_Completed_Tasks();
            Log_Debug_Message(taskCollection);

            Debug.WriteLine($"Adding Project (Pre-Construction Phase) to Collection - Count = {project.Count} & Incomplete count = {project.Count_Of_Incomplete_Tasks}");
            taskCollection.Add_TaskListToCollection(project);
            Log_Debug_Message(taskCollection);
        }

        /// <summary>
        /// Creates the first TaskList object
        /// </summary>
        /// <param name="task0"></param>
        /// <param name="task1"></param>
        /// <param name="task2"></param>
        /// <param name="task3"></param>
        /// <param name="repeatTask"></param>
        /// <returns>TaskList</returns>
        private TaskList CreateTaskList0(StoredTaskApp.Model.Task task0, StoredTaskApp.Model.Task task1, 
                                         StoredTaskApp.Model.Task task2, StoredTaskApp.Model.Task task3, 
                                         RepeatingTask repeatTask)
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

        /// <summary>
        /// Creates the second TaskList object
        /// </summary>
        /// <param name="task0"></param>
        /// <param name="task1"></param>
        /// <param name="task2"></param>
        /// <param name="task3"></param>
        /// <param name="habit0"></param>
        /// <param name="habit1"></param>
        /// <param name="habit2"></param>
        /// <param name="repeatingTask"></param>
        /// <returns>TaskList</returns>
        private TaskList CreateTaskList1(StoredTaskApp.Model.Task task0, StoredTaskApp.Model.Task task1,
                                         StoredTaskApp.Model.Task task2, StoredTaskApp.Model.Task task3, 
                                         Habit habit0, Habit habit1, Habit habit2, RepeatingTask repeatingTask)
        {
            string errorMsg;
            TaskList tasklist;
            Priority currPriority;

            Debug.WriteLine("Create new TaskList (Another List)");
            tasklist = Add_New_Task_List("Another List");

            Debug.WriteLine("Add Task 1 to TaskList (Another List)");
            errorMsg = "Failed to add Task 1 to TaskList (Another List)";
            currPriority = task0.Task_Priority;
            currPriority--;
            task0.Task_Priority = currPriority; 
            tasklist = AddTaskToList(tasklist, task0, errorMsg);

            Debug.WriteLine("Add Task 2 to TaskList (Another List)");
            errorMsg = "Failed to add Task 2 to TaskList (Another List)";
            currPriority = task1.Task_Priority;
            currPriority++;
            task1.Task_Priority = currPriority;
            tasklist = AddTaskToList(tasklist, task1, errorMsg);

            Debug.WriteLine("Add Task 3 to TaskList (Another List)");
            errorMsg = "Failed to add Task 3 to TaskList (Another List)";
            currPriority = task2.Task_Priority;
            currPriority--;
            currPriority--;
            task2.Task_Priority = currPriority;
            tasklist = AddTaskToList(tasklist, task2, errorMsg);

            Debug.WriteLine("Add Task 4 to TaskList (Another List)");
            errorMsg = "Failed to add Task 4 to TaskList (Another List)";
            currPriority = task3.Task_Priority;
            currPriority++;
            currPriority++; 
            task3.Task_Priority = currPriority++;
            tasklist = AddTaskToList(tasklist, task3, errorMsg);

            Debug.WriteLine("Add a Habit 1 to TaskList (Another List)");
            errorMsg = "Failed to add Habit 1 to TaskList (Another List)";
            tasklist = AddTaskToList(tasklist, habit0, errorMsg);

            Debug.WriteLine("Add a Habit 2 to TaskList (Another List)");
            errorMsg = "Failed to add Habit 2 to TaskList (Another List)";
            tasklist = AddTaskToList(tasklist, habit1, errorMsg);

            Debug.WriteLine("Add a Habit 3 to TaskList (Another List)");
            errorMsg = "Failed to add Habit 3 to TaskList (Another List)";
            tasklist = AddTaskToList(tasklist, habit2, errorMsg);

            Debug.WriteLine("Add a Repeating Task 4 to TaskList (Another List)");
            errorMsg = "Failed to add Task 4 to TaskList (Another List)";
            tasklist = AddTaskToList(tasklist, repeatingTask, errorMsg);

            Debug.WriteLine("Displaying all the tasks for Tasklist (Another List)");
            tasklist.Display_Tasks();
            Debug.WriteLine("");

            return tasklist;
        }

        /// <summary>
        /// Creates the third TaskList object
        /// </summary>
        /// <param name="task0"></param>
        /// <param name="task3"></param>
        /// <returns>TaskList</returns>
        private TaskList CreateTaskList2(StoredTaskApp.Model.Task task0, StoredTaskApp.Model.Task task3)
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

        /// <summary>
        /// Creates the Project object
        /// </summary>
        /// <param name="habit0"></param>
        /// <param name="repeatingTask0"></param>
        /// <param name="task0"></param>
        /// <param name="task1"></param>
        /// <param name="task2"></param>
        /// <param name="task3"></param>
        /// <returns>Project</returns>
        private Project CreateProjectTaskList(Habit habit0, RepeatingTask repeatingTask0,
                                              StoredTaskApp.Model.Task task0, StoredTaskApp.Model.Task task1, 
                                              StoredTaskApp.Model.Task task2, StoredTaskApp.Model.Task task3)
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
            task0.Change_Task_Status();
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

        /// <summary>
        /// This function takes two inputs and returns a new Task object
        /// </summary>
        /// <param name="description"></param>
        /// <param name="notes"></param>
        /// <returns>Task</returns>
        private StoredTaskApp.Model.Task Add_New_Task(string description, string notes)
        {
            StoredTaskApp.Model.Task myTask = new StoredTaskApp.Model.Task(description, notes);
            Log_Debug_Message(myTask);
            return myTask;
        }

        /// <summary>
        /// This function takes three inputs and creates a new RepeatingTask object
        /// </summary>
        /// <param name="description"></param>
        /// <param name="notes"></param>
        /// <param name="repeatCycle"></param>
        /// <returns>RepeatingTask</returns>
        private RepeatingTask Add_New_Task(string description, string notes, RepeatCycle repeatCycle)
        {
            RepeatingTask myTask = new RepeatingTask(description, notes, repeatCycle);
            Log_Debug_Message(myTask);
            return myTask;
        }

        /// <summary>
        /// This function takes one input and creates a new TaskList
        /// </summary>
        /// <param name="name"></param>
        /// <returns>TaskList</returns>
        private TaskList Add_New_Task_List(string name)
        {
            TaskList myTaskList = new TaskList(name);
            Log_Debug_Message(myTaskList);
            return myTaskList;
        }

        /// <summary>
        /// This function takes one input and creates a new Project object
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Project</returns>
        private Project Add_New_Project(string name)
        {
            Project myProject = new Project(name);
            Log_Debug_Message(myProject);
            return myProject;
        }

        /// <summary>
        /// This function adds a Task object to a TaskList object
        /// </summary>
        /// <param name="taskList"></param>
        /// <param name="task"></param>
        /// <param name="errorMsg"></param>
        /// <returns>TaskList</returns>
        private TaskList AddTaskToList(TaskList taskList, StoredTaskApp.Model.Task task, string errorMsg)
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

        /// <summary>
        /// This function adds a Habit object to a TaskList object
        /// </summary>
        /// <param name="taskList"></param>
        /// <param name="habit"></param>
        /// <param name="errorMsg"></param>
        /// <returns>TaskList</returns>
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

        /// <summary>
        /// This function add a RepeatingTask object to a TaskList object
        /// </summary>
        /// <param name="taskList"></param>
        /// <param name="repeatTask"></param>
        /// <param name="errorMsg"></param>
        /// <returns>TaskList</returns>
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

        /// <summary>
        /// This function adds a Task object to a Project object
        /// </summary>
        /// <param name="project"></param>
        /// <param name="task"></param>
        /// <param name="errorMsg"></param>
        /// <returns>Project</returns>
        private Project AddTaskToList(Project project, StoredTaskApp.Model.Task task, string errorMsg)
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

        /// <summary>
        /// This function add a Habit task to a Project object
        /// (This is not possible, as Project can only store a Task object)
        /// </summary>
        /// <param name="project"></param>
        /// <param name="habit"></param>
        /// <param name="errorMsg"></param>
        /// <returns>Project</returns>
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

        /// <summary>
        /// This function add a Habit task to a Project object
        /// (This is not possible, as Project can only store a Task object)
        /// </summary>
        /// <param name="project"></param>
        /// <param name="repeatTask"></param>
        /// <param name="errorMsg"></param>
        /// <returns>Project</returns>
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

        /// <summary>
        /// This function displays the Task object data
        /// </summary>
        /// <param name="task"></param>
        private void Log_Debug_Message(StoredTaskApp.Model.Task task)
        {
            Debug.WriteLine("Task object");
            Debug.WriteLine($"Description: '{task.Description}', " +
                $"Notes: '{task.Notes}', Task Priority: {task.Task_Priority.Display_Priority()}, " +
                $"Task Status: {task.Task_Status}, Task Creation Date: {task.Task_Creation_Date}, " +
                $"Task Completion Date: '{task.Task_Completion_Date}'");
            Debug.WriteLine("");
        }

        /// <summary>
        /// This function displays the RepeatingTask object data
        /// </summary>
        /// <param name="repeatingTask"></param>
        private void Log_Debug_Message(RepeatingTask repeatingTask)
        {
            Debug.WriteLine("RepeatingTask object");
            Debug.WriteLine($"Description: '{repeatingTask.Description}', Notes: '{repeatingTask.Notes}', Task Priority: {repeatingTask.Task_Priority.Display_Priority()}, Task Repeat Cycle: {repeatingTask.RepeatCyclePeriod}, Task Status: {repeatingTask.Task_Status}, Task Creation Date: {repeatingTask.Task_Creation_Date}, Task Completion Date: {repeatingTask.Task_Completion_Date}, Task Overdue: {repeatingTask.Overdue}");
            Debug.WriteLine("");

        }

        /// <summary>
        /// This function displays the Habit object data
        /// </summary>
        /// <param name="habit"></param>
        private void Log_Debug_Message(Habit habit)
        {
            Debug.WriteLine("Habit object");
            Debug.WriteLine($"Description: '{habit.Description}', Notes: '{habit.Notes}', Habit Streak Count: {habit.StreakCount}, Task Priority: {habit.Task_Priority.Display_Priority()}, Task Repeat Cycle: {habit.RepeatCyclePeriod}, Task Status: {habit.Task_Status}, Task Creation Date: {habit.Task_Creation_Date}, Task Completion Date: {habit.Task_Completion_Date}, Task Overdue: {habit.Overdue}");
            Debug.WriteLine("");
        }

        /// <summary>
        /// This function displays the TaskList object data
        /// </summary>
        /// <param name="taskList"></param>
        private void Log_Debug_Message(TaskList taskList)
        {
            Debug.WriteLine($"Name of my list: '{taskList.Name}', List count: {taskList.Count}, Incomplete task count: {taskList.Count_Of_Incomplete_Tasks}");
            Debug.WriteLine("");
        }

        /// <summary>
        /// This functions displays the Project object data
        /// </summary>
        /// <param name="project"></param>
        private void Log_Debug_Message(Project project)
        {
            Debug.WriteLine($"Project Name: '{project.Name}' - All Task Count: {project.Count}, Incomplete Task Count: {project.Count_Of_Incomplete_Tasks}, Project Completion %: {project.CompletionPercentage}");
            Debug.WriteLine("");
        }

        /// <summary>
        /// This function displays the TaskCollection object data
        /// </summary>
        /// <param name="taskCollection"></param>
        private void Log_Debug_Message(TaskCollection taskCollection)
        {
            Debug.WriteLine($"Task Collection - All Task Count: {taskCollection.Count}, Incomplete Task Count: {taskCollection.Count_Of_Incomplete_Tasks}");
            Debug.WriteLine("");
        }

        /// <summary>
        /// This function displays all the data of the TaskCollection object
        /// </summary>
        /// <param name="taskCollection"></param>
        private void DisplayTaskCollection(TaskCollection taskCollection)
        {
            Log_Debug_Message(taskCollection);
            foreach (var tasklist in taskCollection.TaskLists)
            {
                switch (tasklist.GetType().ToString())
                {
                    case "StoredTaskApp.Model.TaskList":
                        {
                            StoredTaskApp.Model.TaskList tempTaskList = (StoredTaskApp.Model.TaskList)tasklist;
                            DisplayAllTasks(tasklist);
                            break;
                        }
                    case "StoredTaskApp.Model.Project":
                        {
                            StoredTaskApp.Model.Project tempTaskLiist = (StoredTaskApp.Model.Project)tasklist;
                            DisplayAllTasks((Project)tasklist);
                            break;
                        }
                }
            }
        }

        /// <summary>
        /// Displays all the Tasks / RepeatingTasks / Habits in a TaskList object
        /// </summary>
        /// <param name="tasklist"></param>
        private void DisplayAllTasks(TaskList tasklist)
        {
            Log_Debug_Message(tasklist);
            foreach (var taskItem in tasklist.ReturnTasks)
            {
                switch (taskItem.GetType().ToString())
                {
                    case "StoredTaskApp.Model.Task":
                        {
                            Log_Debug_Message((StoredTaskApp.Model.Task)taskItem);
                            break;
                        }
                    case "StoredTaskApp.Model.Habit":
                        {
                            Log_Debug_Message((StoredTaskApp.Model.Habit)taskItem);
                            break;
                        }
                    case "StoredTaskApp.Model.RepeatingTask":
                        {
                            Log_Debug_Message((StoredTaskApp.Model.RepeatingTask)taskItem);
                            break;
                        }
                }
            }
        }

        /// <summary>
        /// Displays all the Tasks / RepeatingTasks / Habits in a Project object
        /// </summary>
        /// <param name="tasklist"></param>
        private void DisplayAllTasks(Project project)
        {
            Log_Debug_Message(project);
            foreach (var taskItem in project.ReturnTasks)
            {
                switch (taskItem.GetType().ToString())
                {
                    case "StoredTaskApp.Model.Task":
                        {
                            Log_Debug_Message((StoredTaskApp.Model.Task)taskItem);
                            break;
                        }
                    case "StoredTaskApp.Model.Habit":
                        {
                            Log_Debug_Message((StoredTaskApp.Model.Habit)taskItem);
                            break;
                        }
                    case "StoredTaskApp.Model.RepeatingTask":
                        {
                            Log_Debug_Message((StoredTaskApp.Model.RepeatingTask)taskItem);
                            break;
                        }
                }
            }
        }

    }
}
