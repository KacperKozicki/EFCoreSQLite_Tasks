using EFCoreSQLite_Tasks.DataAccess;
using EFCoreSQLite_Tasks.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EFCoreSQLite_Tasks.GUI
{
    /// <summary>
    /// Okno dialogowe do dodawania nowego zadania.
    /// </summary>
    public partial class AddTaskDialog : Window
    {
        private readonly TaskContext taskContext;
        private readonly TaskListWindow taskListWindow;

        /// <summary>
        /// Inicjalizuje nowe wystąpienie klasy <see cref="AddTaskDialog"/>.
        /// </summary>
        /// <param name="taskListWindow">Okno główne listy zadań</param>
        public AddTaskDialog(TaskListWindow taskListWindow)
        {
            InitializeComponent();
            taskContext = new TaskContext();
            this.taskListWindow = taskListWindow;
            LoadUsers();
        }

        /// <summary>
        /// Asynchronicznie zapisuje nowe zadanie do bazy danych.
        /// </summary>
        /// <returns>Prawda, jeśli zadanie zostało pomyślnie zapisane; w przeciwnym razie fałsz</returns>
        private async Task<bool> SaveTask()
        {
            // Pobranie danych z formularza
            string title = TaskTitleTextBox.Text;
            string description = TaskDescriptionTextBox.Text;
            DateTime dueDate = DueDatePicker.SelectedDate ?? DateTime.MinValue;
            int taskPriorityId;

            // Wybór priorytetu zadania na podstawie indeksu w ComboBox
            switch (TaskPriorityComboBox.SelectedIndex)
            {
                case 0:
                    taskPriorityId = 1; // Niski
                    break;
                case 1:
                    taskPriorityId = 2; // Średni
                    break;
                case 2:
                    taskPriorityId = 3; // Wysoki
                    break;
                default:
                    MessageBox.Show("Nieprawidłowy priorytet zadania.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
            }

            // Sprawdzenie poprawności wybranego priorytetu zadania
            if (!taskContext.TaskPriorities.Any(tp => tp.Id == taskPriorityId))
            {
                MessageBox.Show("Nieprawidłowy priorytet zadania.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            try
            {
                // Tworzenie nowego zadania
                Tasks newTask = new Tasks
                {
                    Title = title,
                    Description = description,
                    DueDate = dueDate,
                    TaskPriorityId = taskPriorityId,
                    TaskStageId = 1, // ID dla etapu "Nowe"
                    CreatedDate = DateTime.Now
                };

                // Przypisanie wybranych użytkowników do zadania
                foreach (var checkBox in UsersCheckBoxList.Children.OfType<CheckBox>())
                {
                    if (checkBox.IsChecked == true)
                    {
                        int userId = (int)checkBox.Tag;
                        var user = taskContext.Users.FirstOrDefault(u => u.Id == userId);
                        if (user != null)
                        {
                            if (newTask.Users == null)
                            {
                                newTask.Users = new List<Users>(); // Inicjalizacja listy użytkowników, jeśli jest null
                            }
                            newTask.Users.Add(user);
                        }
                    }
                }

                // Zapis nowego zadania do bazy danych
                taskContext.Tasks.Add(newTask);
                await taskContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas dodawania zadania do bazy danych: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        /// <summary>
        /// Ładuje użytkowników z bazy danych i tworzy CheckBoxy dla każdego z nich.
        /// </summary>
        private void LoadUsers()
        {
            var users = taskContext.Users.Where(u => u.RoleId == 2).ToList();

            foreach (var user in users)
            {
                var checkBox = new CheckBox
                {
                    Content = user.FirstName, // Poprawka: odwołanie do właściwości "FirstName"
                    Tag = user.Id,
                    Margin = new Thickness(5)
                };

                UsersCheckBoxList.Children.Add(checkBox);
            }
        }

        /// <summary>
        /// Obsługa zdarzenia kliknięcia przycisku "Zapisz".
        /// Wywołuje metodę SaveTask() i dodaje nowe zadanie do głównego okna listy zadań.
        /// </summary>
        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (await SaveTask())
            {
               

                // Tworzenie nowego zadania na podstawie danych z formularza
                var newTask = new Tasks
                {
                    Title = TaskTitleTextBox.Text,
                    Description = TaskDescriptionTextBox.Text,
                    DueDate = DueDatePicker.SelectedDate ?? DateTime.MinValue,
                    TaskPriorityId = TaskPriorityComboBox.SelectedIndex + 1,
                    TaskStageId = 1, // ID dla etapu "Nowe"
                    CreatedDate = DateTime.Now
                };

                // Dodawanie nowego zadania do głównego okna listy zadań
                taskListWindow.AddNewTask(newTask);
                MessageBox.Show("Zadanie zostało dodane do bazy danych.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
        }

        


        /// <summary>
        /// Obsługa zdarzenia kliknięcia przycisku "Anuluj".
        /// Zamyka okno dialogowe.
        /// </summary>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
