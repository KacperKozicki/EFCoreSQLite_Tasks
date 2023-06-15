using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreSQLite_Tasks.Models
{
    public class TaskPriority
    {
        /// <summary>
        /// Pobiera lub ustawia identyfikator priorytetu zadania.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Pobiera lub ustawia nazwę priorytetu zadania.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Pobiera lub ustawia kolekcję zadań.
        /// </summary>
        public ICollection<Tasks> Tasks { get; set; }
    }
}

