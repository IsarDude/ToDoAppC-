using System.Collections.Generic;

namespace ToDo.Core.Services
{
    /// <summary>
    /// Interface für alle datenbank services
    /// </summary>
    public interface IDataService
    {
        /// <summary>
        /// Fügt eine neue Person zur Datenbank hinzu und gibt diese mit Id zurück
        /// </summary>
        /// <param name="pers"></param>
        /// <returns></returns>
        public Person AddToDoToPerson(Person pers);

        /// <summary>
        /// Änder ein existierends ToDo in der Datenbank
        /// </summary>
        /// <param name="aToDo"></param>
        /// <returns></returns>
        public void ChangeToDoEntry(ToDoEntry aToDo);

        /// <summary>
        /// Löscht eine Person aus der Datenbank
        /// </summary>
        /// <param name="aPerson"></param>
        /// <returns></returns>
        public void DeletePersonAsync(Person aPerson);

        /// <summary>
        /// Änder eine person die bereits in der datenbank existiert
        /// </summary>
        /// <param name="pers"></param>
        /// <returns></returns>
        public void ChangePerson(Person pers);

        /// <summary>
        /// Löcht ein ToDoEntry aus der Datenbank
        /// </summary>
        /// <param name="aToDo"></param>
        /// <returns></returns>
        public void DeleteToDo(ToDoEntry aToDo);



        /// <summary>
        /// Lädt alle persononen aus der Datenbank asynchron
        /// </summary>
        /// <returns></returns>
        public List<Person> LoadPersons();

        /// <summary>
        /// Lädt alle ToDoes und gibt sie als liste zurück
        /// </summary>
        /// <returns></returns>
        public List<ToDoEntry> LoadAlltoDoes();

        /// <summary>
        /// Gibt eine Person aus der Datenbank zurück, die die selbe Id hat wie die, die übergeben wurde.
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        public Person GetPerson(int personId);

    }
}
