using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace EFCoreSQLite_Tasks.GUI
{
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Tworzenie instancji kontekstu bazy danych
            using (var taskContext = new TaskContext())
            {
                // Usunięcie bazy danych, jeśli istnieje
                //bool deleted = await taskContext.Database.EnsureDeletedAsync();
                //MessageBox.Show($"Baza skasowana. {deleted}");

                // Utworzenie bazy danych, jeśli nie istnieje
                bool created = await taskContext.Database.EnsureCreatedAsync();
                MessageBox.Show($"Baza została utworzona. {created}");



                //string createScript = taskContext.Database.GenerateCreateScript();
                //MessageBox.Show($"Skrypt tworzenia bazy danych:\n\n{createScript}");
            }


        }
    }
}
