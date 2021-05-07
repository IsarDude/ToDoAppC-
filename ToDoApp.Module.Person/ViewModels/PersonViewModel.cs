using Prism.Mvvm;
using System.Collections.Generic;
using System.ComponentModel;
using ToDo.Core;
using ToDo.Core.Services;

namespace ToDoApp.Module.PersonModule.ViewModels
{
    /// <summary>
    /// ViewModel für das Person model
    /// Die person kann bearbeitet und verändert werden, name, Vorname und Position könnnen angepasst werden.
    /// Zusätzliche Funktionen um ToDoes, oder das ViewModel zum Model umzuwandeln
    /// </summary>
    public class PersonViewModel : BindableBase
    {
        private string _lastName;
        private string _firstName;
        private string _position;
        private int _id;
        private List<ToDoViewModel> _assignedTasks;
        private readonly IDataService _dataService;

        /// <summary>
        /// Erstellt das passende ModelView zum mitgegebenen Model
        /// </summary>
        /// <param name="person"> Person Model zur umwandlung in ViewModel</param>
        public PersonViewModel(ToDo.Core.Person person, IDataService dataService)
        {
            _dataService = dataService;
            LastName = person.LastName;
            FirstName = person.FirstName;
            Position = person.Position;
            Id = person.Id;
            AssignedTask = ConvertToDoes(person.AssignedTasks);
        }


        /// <summary>
        /// String object des Nachnamens mit Getter und Setter Funktion
        /// </summary>
        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }

        /// <summary>
        /// String Objekt der Vornamens mit Getter und Setter Funktion
        /// </summary>
        public string FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }

        /// <summary>
        /// String Objekt der Position inerhalb des Teams 
        /// </summary>
        public string Position
        {
            get => _position;
            set => SetProperty(ref _position, value);
        }

        /// <summary>
        /// Id der Peron zur individuellen Identifikation
        /// </summary>
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        /// <summary>
        /// Liste aller zugewiesenen Aufgaben
        /// </summary>
        public List<ToDoViewModel> AssignedTask
        {
            get => _assignedTasks;
            set => SetProperty(ref _assignedTasks, value);
        }

        /// <summary>
        /// Nimmt eine Liste mit ToDoEntry Objekten, und gibt eine Umgewandelte Liste mit ToDoViewModels zurück
        /// </summary>
        /// <param name="toDoList">Liste mit ToDoEntry Objekten</param>
        /// <returns></returns>
        private List<ToDoViewModel> ConvertToDoes(List<ToDo.Core.ToDoEntry> toDoList)
        {
            List<ToDoViewModel> toDoViewModelList = new List<ToDoViewModel>();
            toDoList.ForEach(c => toDoViewModelList.Add(new ToDoViewModel(c, _dataService)));
            return toDoViewModelList;
        }

        /// <summary>
        /// Wird ausgelöst wenn siche eine Property verändert.
        /// Änderung wird an die Datenbank weitergegeben
        /// </summary>
        /// <param name="args"></param>
        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
            if (AssignedTask != null && Position != null && FirstName != null && LastName != null && Id > 0)

                _dataService.ChangePerson(ConvertToModel());

        }

        /// <summary>
        /// Wandelt dieses ViewModel Object wieder in ein Model um.
        /// </summary>
        /// <returns></returns>
        public Person ConvertToModel()
        {
            List<ToDoEntry> toDoEntries = new List<ToDoEntry>();
            foreach (ToDoViewModel viewmodel in AssignedTask)
            {
                toDoEntries.Add(viewmodel.ConvertToModel());
            }
            Person temp = new Person(FirstName, LastName, Position, Id, toDoEntries);
            return temp;
        }

        /// <summary>
        /// Gibt FirstName und Last name, als kompletten String zurürck
        /// </summary>
        /// <returns></returns>
        public string GetFullName()
        {
            return FirstName + " " + LastName;
        }
    }
}
