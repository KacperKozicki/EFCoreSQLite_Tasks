using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreSQLite_Tasks.Models
{
    public class TaskStage
    {
        /// <summary>
        /// Pobiera lub ustawia identyfikator etapu zadania.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Pobiera lub ustawia nazwę etapu zadania.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Pobiera lub ustawia zadania powiązane z etapem.
        /// </summary>
        public List<Tasks> Tasks { get; set; }
    }
}

