using EFCore_Tasks.DataAccess;
using EFCore_Tasks.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EFCore_Tasks.GUI
{
    /// <summary>
    /// Okno szczegółów zadania.
    /// </summary>
    public partial class TaskDetailsWindow : Window
    {
        private TaskListWindow taskListWindow;

        /// <summary>
        /// Zdarzenie wywoływane po aktualizacji punktów zadania.
        /// </summary>
        public event EventHandler<IEnumerable<TaskPoint>> TaskPointsUpdated;

        /// <summary>
        /// Wybrane zadanie.
        /// </summary>
        public Tasks SelectedTask { get; set; }

        /// <summary>
        /// Kolekcja punktów zadania.
        /// </summary>
        public ObservableCollection<TaskPoint> TaskPoints { get; set; }


        /// <summary>
        /// Konstruktor TaskDetailsWindow.
        /// </summary>
        /// <param name="selectedTask">Wybrane zadanie.</param>
        public TaskDetailsWindow(Tasks selectedTask)
        {
            InitializeComponent();
            Closing += TaskDetailsWindow_Closing;
            SelectedTask = selectedTask;
            this.taskListWindow = taskListWindow;

            TaskPoints = new ObservableCollection<TaskPoint>(SelectedTask.TaskPoints);
            DataContext = this;
            LoadTaskPoints();

            foreach (var taskPoint in TaskPoints)
            {
                taskPoint.PropertyChanged += TaskPoint_PropertyChanged;
            }
        }

        /// <summary>
        /// Wczytuje punkty zadania.
        /// </summary>
        private void LoadTaskPoints()
        {
            try
            {
                using (var context = new TaskContext())
                {
                    var task = context.Tasks.Include(t => t.TaskPoints).FirstOrDefault(t => t.Id == SelectedTask.Id);

                    if (task != null)
                    {
                        TaskPoints = new ObservableCollection<TaskPoint>(task.TaskPoints);
                        DataContext = this;
                    }
                    else
                    {
                        MessageBox.Show("Nie znaleziono zadania w bazie danych.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas pobierania danych z bazy danych: {ex.Message}");
            }
        }

        /// <summary>
        /// Zapisuje punkty zadania.
        /// </summary>
        /// <param name="taskPoints">Kolekcja punktów zadania.</param>
        private void SaveTaskPoints(IEnumerable<TaskPoint> taskPoints)
        {
            using (var context = new TaskContext())
            {
                context.TaskPoints.UpdateRange(taskPoints);
                context.SaveChanges();

                // Odśwież listę zadań w TaskListWindow
                var taskListWindow = Application.Current.Windows.OfType<TaskListWindow>().FirstOrDefault();
                taskListWindow?.LoadTasks();
            }
        }

        /// <summary>
        /// Obsługa zdarzenia zamknięcia okna.
        /// </summary>
        /// <param name="sender">Obiekt, który wywołał zdarzenie.</param>
        /// <param name="e">Argumenty zdarzenia.</param>
        private void TaskDetailsWindow_Closing(object sender, CancelEventArgs e)
        {
            TaskPointsUpdated?.Invoke(this, TaskPoints);
            SaveTaskPoints(TaskPoints);
             
            // Odśwież listę zadań w TaskListWindow
            var taskListWindow = Application.Current.Windows.OfType<TaskListWindow>().FirstOrDefault();
            taskListWindow?.LoadTasks();
            taskListWindow.Show();
           
        }

        /// <summary>
        /// Obsługa zdarzenia zmiany właściwości punktu zadania.
        /// </summary>
        /// <param name="sender">Obiekt, który wywołał zdarzenie.</param>
        /// <param name="e">Argumenty zdarzenia.</param>
        private void TaskPoint_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SelectedTask.Progress = TaskPoints.Count(tp => tp.IsCompleted) * 100 / TaskPoints.Count;
        }
    }
}
