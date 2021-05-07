using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
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
    /// List View for Team members, Funktions for adding deleting and switching persons, and filtering the List
    /// </summary>
    [Serializable]
    public class PersonListViewModel : BindableBase
    {
        private List<PersonViewModel> _allPersons;
        private ObservableCollection<PersonViewModel> _persons;
        private PersonViewModel _selectedPerson;
        private string _filter;
        private readonly IEventAggregator _eventAggregator;
        private readonly IDialogService _dialogService;
        private readonly IDataService _dataService;

        /// <summary>
        /// Initailisiere das PersonListViewModel hier werden die Personen im Team verwaltet und In einer Liste gespeichert. Neue Personen können hinzugefügt, geändert und gelöscht werden
        /// </summary>
        /// <param name="eventAggregator"> Um die Ausgewähte Person an die Detaiansicht und die toDoListe zu schicken</param>
        /// <param name="dialogService">Um den Diologservice beim Löschen benutzten zu könnnen</param>
        public PersonListViewModel(IEventAggregator eventAggregator, IDialogService dialogService, IDataService dataService)
        {
            _eventAggregator = eventAggregator;
            _dialogService = dialogService;
            _dataService = dataService;
            _filter = "";
            Task.Run(LoadDataAsync);

            DeleteCommand = new DelegateCommand(DeleteCommandExecute);
            NewCommand = new DelegateCommand(NewCommandExecute);
        }

        /// <summary>
        /// Delegate Command für die nutzung des DelteCommands Über einen Button, damit wird eine Person aus der liste gelöscht
        /// </summary>
        public DelegateCommand DeleteCommand { get; set; }
        /// <summary>
        /// Delegate Command zum erstellen einer neuen Person mittels eines Buttons
        /// Eine Neue Person wird der Liste hinzugefügt
        /// </summary>
        public DelegateCommand NewCommand { get; set; }

        /// <summary>
        /// Liste aller zu verfügung stehender Team mitglieder, die angezeigt werden
        /// </summary>
        public ObservableCollection<PersonViewModel> Persons
        {
            get => _persons;
            set => SetProperty(ref _persons, value);
        }

        /// <summary>
        /// Viewmodel der ausgewählten Person
        /// Wird dieses verändert, wird das neue ViewModel mit der SendSelectedPerson Methode an andere ViewModels weitergegeben
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
                        _ = CheckForPersonUpdate(SelectedPerson);
                    }
                   
                    SendSelectedPerson();
                }

            }
        }

        /// <summary>
        /// Filter um die Liste nach einem bestimmten Namen zu druchsuchen
        /// </summary>
        public string Filter
        {
            get => _filter;
            set
            {
                if (SetProperty(ref _filter, value))
                {
                    ApplyFilter();
                }
            }
        }

        /// <summary>
        /// Wendet den Filter string an und durchsucht die Liste nach dem angegebenen Namen. die Persons Liste wird dann durch die gefilterte liste ausgetauscht und die gefilterten
        /// personen werden in der View angezeigt
        /// </summary>
        private void ApplyFilter()
        {
            if (Filter != null)
            {
                Persons = new ObservableCollection<PersonViewModel>(_allPersons);
                List<PersonViewModel> filteredList = new List<PersonViewModel>();
                filteredList = Persons.Where(p => p.GetFullName().Contains(Filter)).ToList();
                filteredList.Sort((a, b) =>
                            a.GetFullName() == b.GetFullName() ? 0 : a.GetFullName().CompareTo(b.GetFullName())
                );
                Persons = new ObservableCollection<PersonViewModel>(filteredList);
            }
            else
            {
                Filter = "";
            }

        }

        /// <summary>
        /// Ein Dialogfenster wird aufgerufen, nach der User bestätigung wird
        /// die ausgewählte Person aus der Persons und _allPersons Liste gelöscht und die DelteMethode im Dataservice wird ausgeführt
        /// Bei verneinen passiert nichts
        /// </summary>
        private void DeleteCommandExecute()
        {
            var selectedPerson = SelectedPerson;
            if (selectedPerson == null) return;
            String description =
                $"Soll das Teammitglied '{selectedPerson.LastName}, { selectedPerson.FirstName}' wirklich gelöscht werden?";

            _dialogService.ShowDialog("yesNoDialog", new DialogParameters($"message={description}"), async result =>
                {
                    await Task.Delay(0);
                    if (result.Result == ButtonResult.OK)
                    {
                        try
                        {
                            _dataService.DeletePersonAsync(selectedPerson.ConvertToModel());
                            _allPersons.Remove(selectedPerson);
                            Persons.Remove(selectedPerson);
                            SelectedPerson = Persons[0];
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }

                    }

                });

        }

        /// <summary>
        /// Eine neue Person wird erstellt und der Persons List hinzugefügt
        /// </summary>
        private void NewCommandExecute()
        {
            var newId = _allPersons.Max(p => p.Id) + 1;
            var NewPersonModel = new Person
            {
                Id = newId,
                FirstName = "",
                LastName = "",
                Position = "",
                AssignedTasks = new List<ToDoEntry>(),
            };
            var newPerson = new PersonViewModel(NewPersonModel, _dataService);

            _allPersons.Add(newPerson);
            Persons = new ObservableCollection<PersonViewModel>(_allPersons);
            SelectedPerson = newPerson;
        }

        /// <summary>
        /// Überprüft ob die ToDoes der übergebenn Person mit denen in der Datenbank übereinstimmen. Falls dies nicht zutrifft wird die Geupdatete ToDoListe hinzugfügt
        /// </summary>
        /// <param name="selected"></param>
        /// <returns></returns>
        public async Task CheckForPersonUpdate(PersonViewModel selected)
        {
            await Task.Delay(0);
            try
            {
                PersonViewModel person = new PersonViewModel(_dataService.GetPerson(selected.Id), _dataService);
                if (person != null)
                {
                    List<ToDoViewModel> databaseToDoes = person.AssignedTask;
                    List<ToDoViewModel> selectedToDoes = selected.AssignedTask;
                    var databaseCheck = databaseToDoes.Except(selectedToDoes).ToList();
                    var selectedCheck = selectedToDoes.Except(databaseToDoes).ToList();
                    if (!databaseCheck.Any() && selectedCheck.Any())
                    {

                    }
                    else
                    {
                        foreach (PersonViewModel p in _allPersons)
                        {
                            if (p.Id == person.Id)
                            {
                                p.AssignedTask = person.AssignedTask;
                                SelectedPerson = p;
                            }
                        }
                        Persons = new ObservableCollection<PersonViewModel>(_allPersons);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
        /// <summary>
        /// Die ausgewählte person wird als PubSubEvent gesendet um für andere ViewModels benutzbar zu sein.
        /// </summary>
        private void SendSelectedPerson()
        {
            _eventAggregator.GetEvent<PubSubEvent<PersonViewModel>>().Publish(_selectedPerson);

        }


        /// <summary>
        /// Die Methode wird zum Start aufgerufen
        /// alle Daten werden von der Datenbank abgefragt und als _allPersons so wie Persons List angelegt
        /// </summary>
        /// <returns></returns>
        private async Task LoadDataAsync()
        {
            try
            {
                await Task.Delay(0);
                _allPersons = new List<PersonViewModel>();
                List<ToDo.Core.Person> persons = _dataService.LoadPersons();
                persons.ToList().ForEach(c => _allPersons.Add(new PersonViewModel(c, _dataService)));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            Persons = new ObservableCollection<PersonViewModel>(_allPersons);
            if (Persons.Count > 0)
            {
                SelectedPerson = Persons[0];


            }
        }

    }
}
