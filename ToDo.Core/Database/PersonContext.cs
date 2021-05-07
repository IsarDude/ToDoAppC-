using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;

namespace ToDo.Core.Database
{
    /// <summary>
    /// Definiert einen PersonContext um auf der Datenbank mit EFcorezu arbeiten
    /// </summary>
    class PersonContext : DbContext
    {
        /// <summary>
        /// Definiert ein DatenbankSet Persons bestehend aus Person Models
        /// </summary>
        public DbSet<Person> Persons { get; set; }

        /// <summary>
        /// Legt die Benötigten Properties und ihre erlaubte größe für die Datenbank fest
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().Property(p => p.Id).IsRequired();
            modelBuilder.Entity<Person>().Property(p => p.FirstName).IsRequired();
            modelBuilder.Entity<Person>().Property(p => p.FirstName).HasMaxLength(25);
            modelBuilder.Entity<Person>().Property(p => p.LastName).IsRequired();
            modelBuilder.Entity<Person>().Property(p => p.LastName).HasMaxLength(25);
        }

        /// <summary>
        /// Legt die Art der Datenbank, den Namen und ihren Output fest
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=Persons.db");
            optionsBuilder.LogTo(Console.Write);
            optionsBuilder.LogTo(message => Debug.WriteLine(message));

        }

    }
}
