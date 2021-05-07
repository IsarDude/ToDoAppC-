using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Collections.Generic;
using System.Linq;
using ToDo.Core.Database;

namespace ToDo.Core.Services
{
    public class DataService : IDataService
    {
        public DataService()
        {
            DbInitializer.InitializeDB();
        }
        /// <summary>
        /// Fügt eine neue Person zur Datenbank hinzu und gibt diese mit Id zurück
        /// </summary>
        /// <param name="pers"></param>
        /// <returns></returns>
        public Person AddToDoToPerson(Person pers)
        {

            using (var db = new PersonContext())
            {
                var existing = db.Persons.Include(p => p.AssignedTasks).Single(p => p.Id == pers.Id);
                if (existing != null)
                {
                    existing.AssignedTasks = pers.AssignedTasks;
                    db.SaveChanges();
                }
                
            }
            using (var db = new PersonContext())
            {
                var person = db.Persons.Include(p => p.AssignedTasks).Single(p => p.Id == pers.Id);
                db.SaveChanges();
                return person;
            }

        }

        /// <summary>
        /// Änder ein existierends ToDo in der Datenbank
        /// </summary>
        /// <param name="aToDo"></param>
        /// <returns></returns>
        public void ChangeToDoEntry(ToDoEntry aToDo)
        {
            if (aToDo.PersonId > -1)
            {
                using (var db = new PersonContext())
                {
                    var existing = db.Persons.Include(p => p.AssignedTasks).Where(p => p.Id == aToDo.PersonId);

                    if (existing != null)
                    {
                        foreach (Person pers in existing)
                        {

                            foreach (ToDoEntry entry in pers.AssignedTasks)
                            {
                                if (entry.Id == aToDo.Id)
                                {
                                    entry.IsDone = aToDo.IsDone;
                                }
                            }
                        }
                    }
                    db.SaveChanges();

                }
            }

        }

        /// <summary>
        /// Löscht eine Person aus der Datenbank
        /// </summary>
        /// <param name="aPerson"></param>
        /// <returns></returns>
        public void DeletePersonAsync(Person aPerson)
        {
            using (var db = new PersonContext())
            {
                List<ToDoEntry> toBeDeleted = aPerson.AssignedTasks;
                var personToDelete = db.Persons.Find(aPerson.Id);
                if (personToDelete != null)
                {
                    db.Remove(db.Persons.Single(p => p.Id == aPerson.Id));
                    db.SaveChanges();
                }

            }
        }

        /// <summary>
        /// Änder eine person die bereits in der datenbank existiert
        /// </summary>
        /// <param name="pers"></param>
        /// <returns></returns>
        public void ChangePerson(Person pers)
        {
            using (var db = new PersonContext())
            {
                var existing = db.Persons.Find((pers.Id));

                if (existing != null)
                {
                    var changedPerson = db.Persons.Include(p => p.AssignedTasks).Single(p => p.Id == pers.Id);
                    changedPerson.FirstName = pers.FirstName;
                    changedPerson.LastName = pers.LastName;
                    changedPerson.Position = pers.Position;
                    changedPerson.AssignedTasks = pers.AssignedTasks;
                }
                else
                {
                    db.Persons.Add(pers);
                }
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Gibt eine Person aus der Datenbank zurück, die die selbe Id hat wie die, die übergeben wurde.
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        public Person GetPerson(int personId)
        {
            using (var db = new PersonContext())
            {
                var person = db.Persons.Include(p => p.AssignedTasks).Single(p => p.Id == personId);
                return person;
            }

            
        }

        /// <summary>
        /// Löcht ein ToDoEntry aus der Datenbank
        /// </summary>
        /// <param name="aToDo"></param>
        /// <returns></returns>
        public void DeleteToDo(ToDoEntry aToDo)
        {
            using (var db = new PersonContext())
            {
                var existing = db.Persons.Include(p => p.AssignedTasks).Single(p => p.Id == aToDo.PersonId);
                if (existing != null)
                {
                    var toDo = existing.AssignedTasks.Single(t => t.Id == aToDo.Id);
                    existing.AssignedTasks.Remove(toDo);
                    db.SaveChanges();
                }
            }
        }



        /// <summary>
        /// Lädt alle persononen aus der Datenbank asynchron
        /// </summary>
        /// <returns></returns>
        public List<Person> LoadPersons()
        {
            using (var db = new PersonContext())
            {
                IIncludableQueryable<Person, List<ToDoEntry>> persons =
                    db.Persons.Include(person => person.AssignedTasks);
                return persons.ToList();
            }
        }

        /// <summary>
        /// Lädt alle ToDoes und gibt sie als Liste zurück
        /// </summary>
        /// <returns></returns>
        public List<ToDoEntry> LoadAlltoDoes()
        {
            List<ToDoEntry> allToDoes = new List<ToDoEntry>();
            using (var db = new PersonContext())
            {
                IIncludableQueryable<Person, List<ToDoEntry>> persons =
                    db.Persons.Include(person => person.AssignedTasks);
                var personlist = persons.ToList();
                foreach (Person person in personlist)
                {
                    foreach (ToDoEntry entry in person.AssignedTasks)
                    {
                        allToDoes.Add(entry);
                    }
                }
            }
            return allToDoes;
        }

    }
}
