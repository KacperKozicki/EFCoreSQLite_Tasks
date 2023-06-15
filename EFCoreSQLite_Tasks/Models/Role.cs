using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreSQLite_Tasks.Models
{
    /// <summary>
    /// Reprezentuje rolę w systemie.
    /// </summary>
    public class Role
    {
        /// <summary>
        /// Pobiera lub ustawia identyfikator roli.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Pobiera lub ustawia nazwę roli.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Pobiera lub ustawia kolekcję użytkowników należących do tej roli.
        /// </summary>
        public ICollection<Users> Users { get; set; }
    }
}

