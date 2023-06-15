using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore_Tasks.Models
{
    public class TaskPoint : INotifyPropertyChanged
    {
        /// <summary>
        /// Pobiera lub ustawia identyfikator punktu zadania.
        /// </summary>
        [Key]
        public int Id { get; set; }

        private string _content;

        /// <summary>
        /// Pobiera lub ustawia treść punktu zadania.
        /// </summary>
        public string Content
        {
            get { return _content; }
            set
            {
                _content = value;
                OnPropertyChanged("Content");
            }
        }

        private bool _isCompleted;

        /// <summary>
        /// Pobiera lub ustawia wartość określającą, czy punkt zadania jest ukończony.
        /// </summary>
        public bool IsCompleted
        {
            get { return _isCompleted; }
            set
            {
                _isCompleted = value;
                OnPropertyChanged("IsCompleted");
            }
        }

        /// <summary>
        /// Pobiera lub ustawia identyfikator zadania, do którego należy punkt zadania.
        /// </summary>
        public int TaskId { get; set; }

        /// <summary>
        /// Pobiera lub ustawia zadanie, do którego należy punkt zadania.
        /// </summary>
        public Tasks Task { get; set; }

        /// <summary>
        /// Pobiera lub ustawia kolekcję zadań.
        /// </summary>
        public ICollection<Tasks> Tasks { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

