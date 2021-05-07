using Prism.Mvvm;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using ToDo.Core;
using ToDo.Core.Services;

namespace ToDoApp.Module.PersonModule.ViewModels
{
    /// <summary>
    /// ViewModel für ToDoEntry Properties können bearbeitet werden und VieModel kann wieder in ein Model umgewandelt werden
    /// </summary>
    public class ToDoViewModel : BindableBase
    {
        private int _id;
        private string _description;
        private string _dueDate;
        private DateTime _dueDateDateTime;
        private bool _isDone;
        private bool _isImportant;
        private Person _person;
        private int _personId;
        private readonly IDataService _dataService;

        /// <summary>
        /// Properties werden anhand des des Übergebenen ToDoEntries gesetzt
        /// </summary>
        /// <param name="toDo"></param>
        public ToDoViewModel(ToDo.Core.ToDoEntry toDo, IDataService dataService)
        {
            _dataService = dataService;
            Id = toDo.Id;
            Description = toDo.Description;
            DueDateDateTime = toDo.DueDate;
            IsDone = toDo.IsDone;
            IsImportant = toDo.IsImportant;
            Person = toDo.Person;
            PersonId = toDo.PersonId;
        }

        /// <summary>
        /// Datum an dem as ToDo Abgeschlossen sein muss
        /// Wird es Gesetzt wird auch ein String object dieser Propertie gesetzt
        /// </summary>
        public DateTime DueDateDateTime
        {
            get => _dueDateDateTime;
            set
            {
                if (SetProperty(ref _dueDateDateTime, value))
                {
                    DueDate = DueDateDateTime.ToString("dd/MM/yyyy");
                }
            }

        }

        /// <summary>
        /// Eine Id zur Identifikation des ToDoes
        /// </summary>
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        /// <summary>
        /// Beschreibung der zu erledigenden Aufgabe mit Getter und Setter
        /// </summary>
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        /// <summary>
        /// String Object des Fälligkeitsdatums mit Getter und Setter, dient zur Anzeige in der View
        /// </summary>
        public string DueDate
        {
            get => _dueDate;
            set => SetProperty(ref _dueDate, value);
        }

        /// <summary>
        /// Besagt ob das toDo Abgeschlossen ist, wird dieser Wert verändert, wird die änderung an die Datenbank weitergereicht
        /// </summary>
        public bool IsDone
        {
            get => _isDone;
            set
            {
                if (SetProperty(ref _isDone, value))
                {
                    if (Person != null && PersonId > -1)
                    {
                        _ = ChangeTodoViewModelAsync();
                    }

                }
            }
        }

        /// <summary>
        /// Markiert besonders wichtige ToDoes
        /// </summary>
        public bool IsImportant
        {
            get => _isImportant;
            set => SetProperty(ref _isImportant, value);
        }

        /// <summary>
        /// Pointer auf die Person, die das ToDo enthällt
        /// </summary>
        public Person Person
        {
            get => _person;
            set => SetProperty(ref _person, value);
        }

        /// <summary>
        /// Pointer auf die Id der person, die das ToDo enthällt
        /// </summary>
        public int PersonId
        {
            get => _personId;
            set => SetProperty(ref _personId, value);
        }

        /// <summary>
        /// Convertiert das ViewModelr in ein ToDoEntry und gibt es Zurück
        /// </summary>
        /// <returns></returns>
        public ToDoEntry ConvertToModel()
        {
            ToDoEntry temp = new ToDoEntry(Description, DueDateDateTime, IsDone, IsImportant, PersonId, Person);
            temp.Id = Id;
            return temp;
        }

        /// <summary>
        /// Ändert die Property IsDone von einem ToDoentry in der Datenbank
        /// </summary>
        /// <returns></returns>
        private async Task ChangeTodoViewModelAsync()
        {
            await Task.Delay(0);
            try
            {
                _dataService.ChangeToDoEntry(this.ConvertToModel());
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                IsDone = !IsDone;
            }
        }
    }
}
