using System;

namespace ToDo.Core
{
    /// <summary>
    /// Representiert einen Eintrag in die ToDoListe mit allen Informationen
    /// </summary>
    public class ToDoEntry
    {
        /// <summary>
        /// Lehrer Constructor zum erstellen eine Lehren Objectes 
        /// </summary>
        public ToDoEntry() { }

        /// <summary>
        /// Erstellen eines neuen ToDoEntries mit gesetzter Description, DueDate, IsDone, IsImportant, PersonId und Person als besitzer
        /// </summary>
        /// <param name="description"></param>
        /// <param name="dueDate"></param>
        /// <param name="isDone"></param>
        /// <param name="isImportant"></param>
        /// <param name="personId"></param>
        /// <param name="owner"></param>
        public ToDoEntry(string description, DateTime dueDate, bool isDone, bool isImportant, int personId, Person owner)
        {
            Description = description;
            DueDate = dueDate;
            IsDone = isDone;
            IsImportant = isImportant;
            PersonId = personId;
            Person = owner;

        }

        /// <summary>
        /// Setzen und zurückgeben der ein deutigen Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Setzen und zurückgeben der Beschreibung der Aufgabe
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Setzten und zurückgeben des Fälligkeitsdatums
        /// </summary>
        public DateTime DueDate { get; set; }

        /// <summary>
        /// Setzten und zurückgeben eine Booleans der beschreibt ob die aufgabe Abgeschlossen ist
        /// </summary>
        public bool IsDone { get; set; }

        /// <summary>
        /// Setzten und zurückgeben eines Booleans, der beschreibt ob ein ToDo Wichtig ist
        /// </summary>
        public bool IsImportant { get; set; }

        /// <summary>
        /// Setzt und zurückgeben einer Id nummer die die dieses ToDo besitzende Person identifiziert
        /// </summary>
        public int PersonId { get; set; }

        /// <summary>
        /// Object der Person, die diese Instantz besitzt
        /// </summary>
        public Person Person { get; set; }


    }
}