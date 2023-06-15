using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore_Tasks.Models
{
    public class Users
    {
        /// <summary>
        /// Pobiera lub ustawia identyfikator użytkownika.
        /// </summary>
        [Key, Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// Pobiera lub ustawia imię użytkownika.
        /// </summary>
        [Required, MaxLength(50)]
        public string FirstName { get; set; }

        /// <summary>
        /// Pobiera lub ustawia nazwisko użytkownika.
        /// </summary>
        [Required, MaxLength(50)]
        public string LastName { get; set; }

        /// <summary>
        /// Pobiera lub ustawia hasło użytkownika.
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// Pobiera lub ustawia identyfikator roli użytkownika.
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Pobiera lub ustawia rolę użytkownika.
        /// </summary>
        public Role Role { get; set; }

        /// <summary>
        /// Pobiera lub ustawia zadania przypisane do użytkownika.
        /// </summary>
        public ICollection<Tasks> Tasks { get; set; }

        /// <summary>
        /// Pobiera lub ustawia postępy zadań użytkownika.
        /// </summary>
        public ICollection<TaskProgress> TaskProgress { get; set; }
    }
}

