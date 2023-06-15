using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreSQLite_Tasks.Models
{
    public class TaskProgress
    {
        /// <summary>
        /// Pobiera lub ustawia identyfikator postępu zadania.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Pobiera lub ustawia identyfikator zadania.
        /// </summary>
        public int TaskId { get; set; }

        /// <summary>
        /// Pobiera lub ustawia identyfikator punktu zadania.
        /// </summary>
        public int TaskPointId { get; set; }

        /// <summary>
        /// Pobiera lub ustawia identyfikator użytkownika.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Pobiera lub ustawia datę postępu zadania.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Pobiera lub ustawia wartość postępu zadania.
        /// </summary>
        public int Progress { get; set; }

        /// <summary>
        /// Pobiera lub ustawia zadanie powiązane z postępem.
        /// </summary>
        [ForeignKey("TaskId")]
        public Tasks Task { get; set; }

        /// <summary>
        /// Pobiera lub ustawia użytkownika powiązanego z postępem.
        /// </summary>
        [ForeignKey("UserId")]
        public Users User { get; set; }

        /// <summary>
        /// Pobiera lub ustawia punkt zadania powiązany z postępem.
        /// </summary>
        [ForeignKey("TaskPointId")]
        public TaskPoint TaskPoint { get; set; }
    }
}

