using Prism.Events;
using Prism.Mvvm;
using System;

namespace ToDoApp.Module.PersonModule.ViewModels
{
    /// <summary>
    /// ViewModle for working on an PersonViewodel
    /// Subscribes to PubSubEvent from PersonListViewModel to get the Selected Person
    /// </summary>
    [Serializable]
    public class PersonDetailViewModel : BindableBase
    {
        private PersonViewModel _selectedPerson;
        /// <summary>
        /// Subscribe to PubSubEvent to get the Selcted PersonView Model
        /// </summary>
        /// <param name="eventAggregator"> Prism EventAggregator</param>
        public PersonDetailViewModel(IEventAggregator eventAggregator)
        {
            eventAggregator.GetEvent<PubSubEvent<PersonViewModel>>().Subscribe(model => SelectedPerson = model);
        }

        /// <summary>
        /// Selected PersonViewModel to Get or Set the SelectedPerson for binding with PersonDietailView
        /// </summary>
        public PersonViewModel SelectedPerson
        {
            get => _selectedPerson;
            set => SetProperty(ref _selectedPerson, value);
        }
    }


}
