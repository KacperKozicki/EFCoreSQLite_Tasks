using EFCoreSQLite_Tasks.DataAccess;
using EFCoreSQLite_Tasks.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.EntityFrameworkCore;
using System.Globalization;


namespace EFCoreSQLite_Tasks.GUI
{
    public partial class TaskListWindow : Window, INotifyPropertyChanged
    {
        /// <summary>
        /// Aktualnie zalogowany użytkownik.
        /// </summary>private Tasks selectedTask;
        private Tasks selectedTask;

        public Users CurrentUser { get; set; }
        private readonly TaskContext taskContext;

        private ObservableCollection<Tasks> taskList;

        /// <summary>
        /// Lista zadań.
        /// </summary>
        public ObservableCollection<Tasks> TaskList
        {
            get { return taskList; }
            set
            {
                taskList = value;
                OnPropertyChanged(nameof(TaskList));
            }
        }
        private ObservableCollection<TaskPoint> taskPointList;

        public ObservableCollection<TaskPoint> TaskPointList
        {
            get { return taskPointList; }
            set
            {
                taskPointList = value;
                OnPropertyChanged(nameof(TaskPointList));
            }
        }

        private ObservableCollection<Tasks> taskHistory;

        /// <summary>
        /// Historia zakończonych zadań.
        /// </summary>
        public ObservableCollection<Tasks> TaskHistory
        {
            get { return taskHistory; }
            set
            {
                taskHistory = value;
                OnPropertyChanged(nameof(TaskHistory));
            }
        }

        /// <summary>
        /// Postęp aktualnie wyświetlanego zadania.
        /// </summary>
        public int Progress { get; set; }
       

        private ObservableCollection<TaskProgress> taskProgresses;

        /// <summary>
        /// Lista postępów zadań.
        /// </summary>
        public ObservableCollection<TaskProgress> TaskProgresses
        {
            get { return taskProgresses; }
            set
            {
                taskProgresses = value;
                OnPropertyChanged(nameof(TaskProgresses));
            }
        }

        /// <summary>
        /// Zdarzenie wywoływane po zmianie wartości właściwości.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Konstruktor klasy TaskListWindow.
        /// </summary>
        /// <param name="user">Zalogowany użytkownik</param>
        public TaskListWindow(Users user)
        {
            InitializeComponent();
            CurrentUser = user;
            DataContext = this;
            TaskList = new ObservableCollection<Tasks>();
            taskContext = new TaskContext();
            TaskPointList = new ObservableCollection<TaskPoint>();
            TaskHistory = new ObservableCollection<Tasks>();
            TaskProgresses = new ObservableCollection<TaskProgress>();
            SubscribeToTaskPointsUpdatedEvent();
            LoadTasks();
        }

        /// <summary>
        /// Metoda wywoływana po zmianie wartości właściwości.
        /// </summary>
        /// <param name="propertyName">Nazwa zmienionej właściwości</param>
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Asynchronicznie wczytuje zadania z bazy danych i aktualizuje listy zadań.
        /// </summary>
        public async Task LoadTasks()
        {
            TaskList.Clear();
            TaskHistory.Clear();
            IQueryable<Tasks> tasksQuery;

            if (CurrentUser.RoleId == 1 || CurrentUser.RoleId == 3)
            {
                // Użytkownicy z RoleId 1 i 3 widzą wszystkie zadania
                tasksQuery = taskContext.Tasks;
            }
            else if (CurrentUser.RoleId == 2)
            {
                // Użytkownicy z RoleId 2 widzą tylko zadania przypisane do siebie
                tasksQuery = taskContext.Tasks
                    .Where(t => t.Users.Any(u => u.Id == CurrentUser.Id));
            }
            else
            {
                // Pozostałe role nie mają dostępu do żadnych zadań
                tasksQuery = taskContext.Tasks.Where(t => false);
            }

            var tasks = await tasksQuery
                .Include(t => t.TaskPriority)
                .Include(t => t.TaskStage)
                .Include(t => t.TaskPoints)
                .ToListAsync();

            foreach (var task in tasks)
            {
                TaskList.Add(task);
                task.PropertyChanged += Task_PropertyChanged;

                if (task.TaskStageId == 4 || (task.TaskPoints != null && task.TaskPoints.All(tp => tp.IsCompleted)))
                {
                    TaskHistory.Add(task);
                }

                // Przypisanie wartości postępu zadania z uwzględnieniem punktów
                if (task.TaskPoints != null || task.TaskPoints.Any())
                {
                    int completedPoints = task.TaskPoints.Count(tp => tp.IsCompleted);
                    int totalPoints = task.TaskPoints.Count;

                    if (totalPoints > 0)
                    {
                        task.Progress = (completedPoints * 100) / totalPoints;
                    }
                    else
                    {
                        task.Progress = 0; // lub inna wartość domyślna
                    }
                }
                else
                {
                    task.Progress = 0;
                }

                // Zapisanie wartości postępu zadania do bazy danych
                taskContext.SaveChanges();
            }
        }



        /// <summary>
        /// Obsługa zdarzenia kliknięcia przycisku dodawania zadania.
        /// Otwiera okno dialogowe AddTaskDialog.
        /// </summary>
        private void OpenAddTaskDialog_Click(object sender, RoutedEventArgs e)
        {
            if (UserHasPermissionToCreateTask())
            {
                AddTaskDialog dialog = new AddTaskDialog(this);
                dialog.ShowDialog();
            }
            else
            {
                MessageBox.Show("Nie masz uprawnień do dodawania nowych zadań.");
            }
        }

        /// <summary>
        /// Obsługa zdarzenia zmiany właściwości zadania.
        /// Wywołuje zdarzenie PropertyChanged dla właściwości TaskList.
        /// </summary>
        private void Task_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(TaskList));
        }

        /// <summary>
        /// Dodaje nowe zadanie do listy zadań.
        /// </summary>
        /// <param name="newTask">Nowe zadanie</param>
        public void AddNewTask(Tasks newTask)
        {
            TaskList.Add(newTask);
            MessageBox.Show("Nowe zadanie zostało dodane do listy.");
            OnPropertyChanged(nameof(TaskList));
        }


        public void AddNewTaskPoint(TaskPoint newTaskPoint)
        {
            taskContext.TaskPoints.Add(newTaskPoint);
            taskContext.SaveChanges();
            MessageBox.Show("Nowy punkt został dodany do zadania.");
            OnPropertyChanged(nameof(TaskPointList));
        }

        /// <summary>
        /// Obsługa zdarzenia kliknięcia przycisku wylogowania.
        /// Wyświetla okno MainWindow i zamyka bieżące okno.
        /// </summary>
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        /// <summary>
        /// Asynchronicznie usuwa zadanie z bazy danych i z listy zadań.
        /// </summary>
        /// <param name="task">Zadanie do usunięcia</param>
        private async void DeleteTask(Tasks task)
        {
            var confirmResult = MessageBox.Show("Czy na pewno chcesz usunąć to zadanie?", "Potwierdzenie usunięcia", MessageBoxButton.YesNo);
            if (confirmResult == MessageBoxResult.Yes)
            {
                taskContext.Tasks.Remove(task);
                await taskContext.SaveChangesAsync();
                TaskList.Remove(task);
                MessageBox.Show("Zadanie zostało usunięte.");
            }
        }

        /// <summary>
        /// Obsługa zdarzenia kliknięcia przycisku usuwania zadania.
        /// Usuwa zadanie z listy zadań i z bazy danych, jeśli użytkownik ma odpowiednie uprawnienia.
        /// </summary>
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentUser.RoleId == 1 || CurrentUser.RoleId == 3)
            {
                var button = (Button)sender;
                var task = (Tasks)button.DataContext;
                DeleteTask(task);
            }
            else
            {
                MessageBox.Show("Nie masz uprawnień do usuwania zadań.");
            }
        }


        

        /// <summary>
        /// Sprawdza, czy użytkownik ma uprawnienia do tworzenia nowego zadania.
        /// </summary>
        /// <returns>Prawda, jeśli użytkownik ma uprawnienia do tworzenia zadania; w przeciwnym razie fałsz</returns>
        private bool UserHasPermissionToCreateTask()
        {
            return CurrentUser.RoleId == 1 || CurrentUser.RoleId == 3;
        }

        

        /// <summary>
        /// Obsługa zdarzenia zmiany zaznaczenia w ListBoxie z zadaniami.
        /// Otwiera okno TaskDetailsWindow, wyświetlające szczegóły wybranego zadania.
        /// </summary>
        private void TaskListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = (ListBox)sender;
            var selectedTask = (Tasks)listBox.SelectedItem;

            if (selectedTask != null)
            {
                var taskDetailsWindow = new TaskDetailsWindow(selectedTask);
                taskDetailsWindow.ShowDialog();

                // Clear the selection after opening the task details window
                listBox.SelectedItem = null;
            }
        }

        private void NewUserButton_Click(object sender, RoutedEventArgs e)
        {
            string firstName = NewUserFirstNameTextBox.Text;
            string lastName = NewUserLastNameTextBox.Text;
            string password = NewUserPasswordBox.Password;
            int roleId = 2; // Domyślnie ustawiamy roleId na 2

            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Wprowadź wszystkie wymagane informacje.");
                return;
            }

            // Sprawdzamy, czy zalogowany użytkownik ma uprawnienia administratora lub kierownika
            if (CurrentUser.RoleId != 1 && CurrentUser.RoleId != 3)
            {
                MessageBox.Show("Nie masz uprawnień do dodawania nowych użytkowników.");
                return;
            }

            // Tworzymy nowego użytkownika
            Users newUser = new Users
            {
                FirstName = firstName,
                LastName = lastName,
                Password = PasswordHasher.HashPassword(password),
                RoleId = roleId
            };

            taskContext.Users.Add(newUser);
            taskContext.SaveChanges();

            MessageBox.Show("Nowy użytkownik został dodany.");

            // Czyszczenie pól formularza
            NewUserFirstNameTextBox.Text = string.Empty;
            NewUserLastNameTextBox.Text = string.Empty;
            NewUserPasswordBox.Password = string.Empty;
        }

        /// <summary>
        /// Obsługa zdarzenia ładowania przycisku usuwania.
        /// Ustawia widoczność przycisku w zależności od roli użytkownika.
        /// </summary>
        private void DeleteButton_Loaded(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var task = (Tasks)button.DataContext;

            if (CurrentUser.RoleId == 1 || CurrentUser.RoleId == 3)
            {
                button.Visibility = Visibility.Visible;
            }
            else
            {
                button.Visibility = Visibility.Collapsed;
            }
        }
        


        private void AddTaskPointButton_Click(object sender, RoutedEventArgs e)
        {
            if (UserHasPermissionToCreateTask())
            {
                var button = (Button)sender;
                selectedTask = (Tasks)button.DataContext;

                if (selectedTask != null)
                {
                    AddTaskPointDialog dialog = new AddTaskPointDialog(this, selectedTask);
                    dialog.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Nie masz uprawnień do dodawania nowych zadań.");
            }
        }




        private void TaskDetailsWindow_TaskPointsUpdated(object sender, IEnumerable<TaskPoint> taskPoints)
        {
            LoadTasks();
        }

        private void OpenTaskDetailsWindow(Tasks selectedTask)
        {
            var detailsWindow = new TaskDetailsWindow(selectedTask);
            detailsWindow.TaskPointsUpdated += TaskDetailsWindow_TaskPointsUpdated;
            detailsWindow.Show();
        }
        private void SubscribeToTaskPointsUpdatedEvent()
        {
            var detailsWindows = Application.Current.Windows.OfType<TaskDetailsWindow>();
            foreach (var detailsWindow in detailsWindows)
            {
                detailsWindow.TaskPointsUpdated += TaskPointsUpdatedEventHandler;
            }
        }

        private void TaskPointsUpdatedEventHandler(object sender, IEnumerable<TaskPoint> taskPoints)
        {
            LoadTasks();
        }
    }
}
