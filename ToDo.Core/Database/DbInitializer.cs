using System;
using System.Collections.Generic;
using System.Linq;

namespace ToDo.Core.Database
{
    /// <summary>
    /// Dient zum Initialiesierend der Datenbank
    /// </summary>
    class DbInitializer
    {
        /// <summary>
        /// Prüft ob die Datenbank vorhanden ist und erstellt eine neue, falls das nicht der fall sein sollte. Ist die Datenbank leer wird die methode SeedDatabase augerufen
        /// </summary>
        public static void InitializeDB()
        {
            using (var db = new PersonContext())
            {
                //db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
            }

            using (var db = new PersonContext())
            {
                if (!db.Persons.Any())
                {
                    SeedDatabase(db);
                }
            }
        }

        /// <summary>
        /// Füllt die Datenbank mit Beispielen
        /// </summary>
        /// <param name="context"></param>
        public static void SeedDatabase(PersonContext context)
        {
            List<Person> list = LoadSamplePersons();
            foreach (Person person in list)
            {
                context.Add(person);
            }
            context.SaveChanges();
        }


        /// <summary>
        /// Erstelt Beispiel Personen mit Beispiel ToDoes und gibt sie zurück
        /// </summary>
        /// <returns></returns>
        public static List<Person> LoadSamplePersons() =>
            new List<Person>
            {
                new Person
                {
                    FirstName = "Luaks",
                    LastName = "Fritsch",
                    Id = 1,
                    Position = "Werkstudent",
                    AssignedTasks =  new List<ToDoEntry>
                                        {
                                            new ToDoEntry
                                            {
                                                Id = 1,
                                                Description = "Task1",
                                                DueDate = new DateTime(2021,06,30),
                                                IsImportant = true,
                                                IsDone = false

                                            },
                                            new ToDoEntry {
                                                Id = 2,
                                                Description = "Task2",
                                                DueDate = DateTime.Now,
                                                IsImportant = false,
                                                IsDone = true
                                            }
                                        }

                },
                new Person
                {
                    FirstName = "Titus",
                    LastName = "Fritsch",
                    Id = 2,
                    Position = "Schüler",
                    AssignedTasks =  new List<ToDoEntry>
                                        {
                                            new ToDoEntry
                                            {
                                                Id = 3,
                                                Description = "Task3",
                                                DueDate = new DateTime(2021,07,30),
                                                IsImportant = true,
                                                IsDone = false

                                            },
                                            new ToDoEntry {
                                                Id = 4,
                                                Description = "Task4",
                                                DueDate = new DateTime(2021,01,30),
                                                IsImportant = false,
                                                IsDone = false
                                            }
                                        }
                },
                new Person
                {
                    FirstName = "Julie",
                    LastName = "Fritsch",
                    Id = 3,
                    Position = "Therater",
                    AssignedTasks = new List<ToDoEntry>
                                        {
                                            new ToDoEntry
                                            {
                                                Id = 5,
                                                Description = "Task5",
                                                DueDate = new DateTime(2021,06,30),
                                                IsImportant = true,
                                                IsDone = false

                                            },
                                            new ToDoEntry {
                                                Id = 6,
                                                Description = "Task6",
                                                DueDate = new DateTime(2021,01,20),
                                                IsImportant = false,
                                                IsDone = false
                                            }
                                        }
                },
            };
    }
}

