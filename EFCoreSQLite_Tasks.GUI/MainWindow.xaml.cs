using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using EFCoreSQLite_Tasks.Models;
using EFCoreSQLite_Tasks;
using EFCoreSQLite_Tasks.DataAccess;
using Microsoft.EntityFrameworkCore;
using EFCoreSQLite_Tasks.GUI;

namespace EFCoreSQLite_Tasks.GUI
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TaskContext taskContext;
        private Users currentUser;

        public MainWindow()
        {
            InitializeComponent();
            taskContext = new TaskContext();

        }

        /// <summary>
        /// Obsługa zdarzenia kliknięcia przycisku logowania.
        /// </summary>
        /// <param name="sender">Obiekt, który wywołał zdarzenie.</param>
        /// <param name="e">Argumenty zdarzenia.</param>
        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text;
            string password = PasswordBox.Password;

            Users currentUser = await taskContext.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.FirstName == login);

            if (currentUser != null && PasswordHasher.VerifyPassword(password, currentUser.Password))
            {
                //MessageBox.Show($"Zalogowano jako {currentUser.FirstName} {currentUser.LastName}.");
                OpenTaskListWindow(currentUser);
            }
            else
            {
                MessageBox.Show("Niepoprawny login lub hasło.");
            }
        }

        /// <summary>
        /// Otwiera okno listy zadań dla danego użytkownika.
        /// </summary>
        /// <param name="user">Zalogowany użytkownik.</param>
        /// <returns>Zadanie asynchroniczne reprezentujące operację otwarcia okna listy zadań.</returns>
        private async Task OpenTaskListWindow(Users user)
        {
            currentUser = user;

            TaskListWindow taskListWindow = new TaskListWindow(user);
            taskListWindow.Show();
            this.Close();

            //TaskListWindow taskListWindow = new TaskListWindow(currentUser);
            //taskListWindow.Show();
            //this.Close();
        }

    }
}
