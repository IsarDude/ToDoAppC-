using System;
using System.Collections.Generic;
using System.Linq;

namespace ToDo.Core
{
    /// <summary>
    /// Person Model Räpresentiert Teammitglieder in der ToDoApp
    /// </summary>
    public class Person
    {
        /// <summary>
        /// Lerer Constructor für Initialisierung
        /// </summary>
        public Person() { }

        /// <summary>
        /// Contructor um Model mit entsprechenden Properties anzulegen
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="position"></param>
        /// <param name="id"></param>
        /// <param name="assignedTasks"></param>
        public Person(string firstName, string lastName, string position, int id, List<ToDoEntry> assignedTasks)
        {
            FirstName = firstName;
            LastName = lastName;
            Position = position;
            Id = id;
            AssignedTasks = assignedTasks;
        }

        /// <summary>
        /// Setzten und zurückgeben des Vornamen für diese Person
        /// </summary>
        public String FirstName { get; set; }

        /// <summary>
        /// Setzten und zurückgeben des Nachnamen für diese Person
        /// </summary>
        public String LastName { get; set; }

        /// <summary>
        /// Setzten und zurückgeben der Position inerhalb des Teams dieser Person
        /// </summary>
        public String Position { get; set; }

        /// <summary>
        /// Setzten und Zurückgeben einer eindeutigen Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Setzten und zurückgeben der Zugewiesenen ToDoes als Liste 
        /// </summary>
        public List<ToDoEntry> AssignedTasks { get; set; }

        /// <summary>
        /// Vorname und Nachmname als ein String object zurückgeben
        /// </summary>
        /// <returns></returns>
        public override String ToString()
        {
            return FirstName + " " + LastName;
        }

        /// <summary>
        /// Gibt den prozentualen Anteil der erledigten ToDoes zurück
        /// </summary>
        /// <returns></returns>
        public int GetPercentageDone()
        {
            IEnumerable<ToDoEntry> query = from aTask in AssignedTasks
                                           where aTask.IsDone == true
                                           select aTask;

            return (AssignedTasks.Count() / query.Count()) * 100;
        }

    }
}