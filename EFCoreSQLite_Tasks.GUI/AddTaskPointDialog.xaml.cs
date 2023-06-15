using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using EFCore_Tasks.DataAccess;
using EFCore_Tasks.Models;

namespace EFCore_Tasks.GUI
{
    public partial class AddTaskPointDialog : Window
    {
        private TaskPoint newTaskPoint; 
        private readonly TaskContext taskContext;
        private readonly TaskListWindow taskListWindow;
        private readonly Tasks currentTask;
        public AddTaskPointDialog(TaskListWindow taskListWindow, Tasks currentTask)
        {
            InitializeComponent();
            taskContext = new TaskContext();
            this.taskListWindow = taskListWindow; 
            this.currentTask = currentTask;

        }
       

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {



            TaskPoint newTaskPoint = new TaskPoint
            {
                Content = TaskTitleTextBox.Text,
                IsCompleted = false,
                TaskId=currentTask.Id,
            };

            // Dodawanie nowego zadania do głównego okna listy zadań


            // Dodaj nowy taskpoint do zadania
            taskListWindow.AddNewTaskPoint(newTaskPoint);
            Close();
           
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Zamknij okno dialogowe bez zapisywania nowego TaskPointa
            this.Close();
        }

       
    }
}
