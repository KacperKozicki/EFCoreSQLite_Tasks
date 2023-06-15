using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore_Tasks.Models
{
    public class Tasks : INotifyPropertyChanged
    {
        /// <summary>
        /// Pobiera lub ustawia identyfikator zadania.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Pobiera lub ustawia tytuł zadania.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Pobiera lub ustawia opis zadania.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Pobiera lub ustawia identyfikator priorytetu zadania.
        /// </summary>
        public int TaskPriorityId { get; set; }

        /// <summary>
        /// Pobiera lub ustawia datę utworzenia zadania.
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Pobiera lub ustawia termin zadania.
        /// </summary>
        public DateTime DueDate { get; set; }

        /// <summary>
        /// Pobiera lub ustawia identyfikator statusu zadania.
        /// </summary>
        public int StatusId { get; set; }

        /// <summary>
        /// Pobiera lub ustawia użytkowników powiązanych z zadaniem.
        /// </summary>
        public ICollection<Users> Users { get; set; }

        /// <summary>
        /// Pobiera lub ustawia priorytet zadania.
        /// </summary>
        public TaskPriority TaskPriority { get; set; }

        /// <summary>
        /// Pobiera lub ustawia postępy zadania.
        /// </summary>
        public ICollection<TaskProgress> TaskProgresses { get; set; }

        /// <summary>
        /// Pobiera lub ustawia identyfikator etapu zadania.
        /// </summary>
        public int TaskStageId { get; set; }

        /// <summary>
        /// Pobiera lub ustawia etap zadania.
        /// </summary>
        public TaskStage TaskStage { get; set; }

        /// <summary>
        /// Pobiera lub ustawia punkty zadania.
        /// </summary>
        public ICollection<TaskPoint> TaskPoints { get; set; }

        private int progress;

        /// <summary>
        /// Pobiera lub ustawia postęp zadania.
        /// </summary>
        public int Progress
        {
            get { return progress; }
            set
            {
                if (progress != value)
                {
                    progress = value;
                    OnPropertyChanged(nameof(Progress));
                }
            }
        }

        public Tasks()
        {
            TaskProgresses = new List<TaskProgress>();
            TaskPoints = new ObservableCollection<TaskPoint>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
