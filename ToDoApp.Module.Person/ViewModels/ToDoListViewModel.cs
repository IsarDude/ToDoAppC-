using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ToDo.Core;
using ToDo.Core.Services;

namespace ToDoApp.Module.PersonModule.ViewModels
{
    /// <summary>
    /// Alle ToDoes einer Person werden hier gespeichert verwaltet und zur Anzeige und Bearbeitung bereitgestellt
    /// </summary>
    public class ToDoListViewModel : BindableBase
    {
        private PersonViewModel _selectedPerson;
        private List<ToDoViewModel> _toDoList;
        private ObservableCollection<ToDoViewModel> _toDoes;
        private ToDoViewModel _selectedToDo;
        private ToDoViewModel _newToDo;
        private readonly IDataService _dataService;

        /// <summary>
        /// Prism EventAgragtor wird gesetzt und so eingerichtet, dass die Locale SelectedPerson, die SlectedPerson aus dem PersonListViewModel ist
        /// DelegateCommands werden erstellt
        /// </summary>
        /// <param name="eventAggregator"></param>
        public ToDoListViewModel(IEventAggregator eventAggregator, IDataService dataService)
        {
            eventAggregator.GetEvent<PubSubEvent<PersonViewModel>>().Subscribe(model => SelectedPerson = model);
            _dataService = dataService;
            CreateNewToDo();
            DeleteCommand = new DelegateCommand(DeleteCommandExecute);
            AddNewCommand = new DelegateCommand(AddCommandExecute);
            SortByDueDate = new DelegateCommand(SortByDueDateExecute);
            SortByImportant = new DelegateCommand(SortByImportantExecute);
            SortByIsDone = new DelegateCommand(SortByIsDoneExecute);
        }

        /// <summary>
        /// DelegateCommand zum Löschen eines ToDoes aus der Liste
        /// </summary>
        public DelegateCommand DeleteCommand { get; set; }

        /// <summary>
        /// DelegateCommand zum hinzufügen eines neuen ToDos zur Liste
        /// </summary>
        public DelegateCommand AddNewCommand { get; set; }

        /// <summary>
        /// DelegateCommand um die Liste nach dem Datum aufsteigend zu sortieren
        /// </summary>
        public DelegateCommand SortByDueDate { get; set; }

        /// <summary>
        /// DelegateCommand um die Liste nach IsDone zu sortieren
        /// </summary>
        public DelegateCommand SortByIsDone { get; set; }

        /// <summary>
        /// DelegateCommand um die Liste nach IsImportant zu sortieren
        /// </summary>
        public DelegateCommand SortByImportant { get; set; }

        /// <summary>
        ///ObservableList von allen AssignedTasks der ausgewähltern Person
        /// </summary>
        public ObservableCollection<ToDoViewModel> ToDoes
        {
            get => _toDoes;
            set => SetProperty(ref _toDoes, value);
        }

        /// <summary>
        /// Ein neues ToDo wird hier angelegt und bearbeitet bevor es der ToDoListe einer Person hinzugefügt Wird
        /// </summary>
        public ToDoViewModel NewToDo
        {
            get => _newToDo;
            set => SetProperty(ref _newToDo, value);
        }

        /// <summary>
        /// Sets und Gets das ausgewählte Todo aus der ToDoListe
        /// </summary>
        public ToDoViewModel SelectedToDo
        {
            get => _selectedToDo;
            set => SetProperty(ref _selectedToDo, value);
        }

        /// <summary>
        /// Setzt und holt die ausgewählte Person. Wird eine neue Peron gesetzt wird die ToDoListe ausgetauscht und nach DueDate und IsDone sortiert
        /// </summary>
        public PersonViewModel SelectedPerson
        {
            get => _selectedPerson;
            set
            {
                if (SetProperty(ref _selectedPerson, value))
                {
                    if (SelectedPerson != null)
                    {
                        _toDoList = SelectedPerson.AssignedTask;
                        ToDoes = new ObservableCollection<ToDoViewModel>(_toDoList);
                        SortByDueDateExecute();
                        SortByIsDoneExecute();
                    }
                }
            }
        }

        /// <summary>
        /// Sortiert die ToDoes nach dem DueDate ausfsteigend
        /// </summary>
        private void SortByDueDateExecute()
        {
            _toDoList = ToDoes.ToList();
            _toDoList.Sort((a, b) =>
                  a.DueDateDateTime == b.DueDateDateTime ? 0 : (a.DueDateDateTime > b.DueDateDateTime ? 1 : -1));
            ToDoes = new ObservableCollection<ToDoViewModel>(_toDoList);

        }

        /// <summary>
        /// Sortiert die ToDoes nach ihrer Wichtigkeit
        /// </summary>
        private void SortByImportantExecute()
        {
            _toDoList = ToDoes.ToList();
            _toDoList.Sort((a, b) => a.IsImportant || b.IsImportant ? (a.IsImportant && b.IsImportant ? 0 : (a.IsImportant ? 1 : -1)) : 0);
            ToDoes = new ObservableCollection<ToDoViewModel>(_toDoList);
        }

        /// <summary>
        /// Sortiert die todoes nach Abgeschlossen oder nicht Abgeschlossen
        /// </summary>
        private void SortByIsDoneExecute()
        {
            _toDoList = ToDoes.ToList();
            _toDoList.Sort((a, b) => a.IsDone || b.IsDone ? (a.IsDone && b.IsDone ? 0 : (a.IsDone ? 1 : -1)) : 0);
            ToDoes = new ObservableCollection<ToDoViewModel>(_toDoList);
        }

        /// <summary>
        /// Erstellt eine Neues "Leeres" ToDoviewModel und setzt dieses als NewToDo
        /// </summary>
        private void CreateNewToDo()
        {
            var newItem = new ToDoViewModel(new ToDo.Core.ToDoEntry
            {
                Description = "",
                DueDate = DateTime.Now,
                IsDone = false,
                IsImportant = false
            },
            _dataService);
            NewToDo = newItem;
        }

        /// <summary>
        /// Fügt der ToDoListe ein neues ToDo hinzu und speichert die änderung in der Datenbank
        /// </summary>
        private async void AddCommandExecute()
        {
            await Task.Delay(0);
            if (NewToDo.Description != "")
            {
                NewToDo.Person = SelectedPerson.ConvertToModel();
                NewToDo.PersonId = SelectedPerson.Id;
                SelectedPerson.AssignedTask.Add(NewToDo);
                _toDoList.Add(NewToDo);
                ToDoes = new ObservableCollection<ToDoViewModel>(_toDoList);
                try
                {
                    Person temp = _dataService.AddToDoToPerson(SelectedPerson.ConvertToModel());
                    List<ToDoViewModel> list = new List<ToDoViewModel>();
                    foreach (ToDoEntry entry in temp.AssignedTasks)
                    {
                        list.Add(new ToDoViewModel(entry, _dataService));
                    }
                    _toDoList = list;
                    SortByDueDateExecute();
                    SortByIsDoneExecute();
                    ToDoes = new ObservableCollection<ToDoViewModel>(_toDoList);
                    SelectedPerson = new PersonViewModel(temp, _dataService);
                    SelectedToDo = NewToDo;

                    CreateNewToDo();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }

        }

        /// <summary>
        /// Das Ausgewählte ToDo wird aus der Liste und der Datenbank gelöscht
        /// </summary>
        private async void DeleteCommandExecute()
        {
            await Task.Delay(0);
            try
            {
                _dataService.DeleteToDo(SelectedToDo.ConvertToModel());
                _toDoList.Remove(SelectedToDo);
                ToDoes.Remove(SelectedToDo);


                if (ToDoes.Count() > 0)
                {
                    SelectedToDo = ToDoes[0];
                }
                else
                {
                    SelectedToDo = null;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

        }

    }
}


