using Prism.Mvvm;
using Prism.Regions;
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
    /// Übersicht über alle anstehenden ToDoes den Fortschritt des Teams und überfällige Aufgaben
    /// </summary>
    public class OverViewViewModel : BindableBase, INavigationAware
    {
        private List<ToDoViewModel> _allToDoes;
        private ObservableCollection<ToDoViewModel> _overdueToDoes;
        private ObservableCollection<ToDoViewModel> _teamToDoes;
        private int _progress;
        private readonly IDataService _dataService;
        public OverViewViewModel(IDataService dataService)
        {
            _dataService = dataService;
            _ = LoadAllToDoes();
        }

        /// <summary>
        /// Fortschritt des Teams in Prozent
        /// </summary>
        public int Progress
        {
            get => _progress;
            set => SetProperty(ref _progress, value);
        }
        /// <summary>
        /// Alle ToDoes die Überfällig sind
        /// </summary>
        public ObservableCollection<ToDoViewModel> OverdueToDoes
        {
            get => _overdueToDoes;
            set => SetProperty(ref _overdueToDoes, value);
        }

        /// <summary>
        /// Alle restlichen noch nicht erfüllten Todoes
        /// </summary>
        public ObservableCollection<ToDoViewModel> TeamToDoes
        {
            get => _teamToDoes;
            set => SetProperty(ref _teamToDoes, value);
        }

        /// <summary>
        /// Lädt alle Todoes von der Datenbank und Sortiert sie in die entprechenden Listen ein
        /// </summary>
        /// <returns></returns>
        private async Task LoadAllToDoes()
        {
            await Task.Delay(0);
            try
            {
                List<ToDoEntry> allEntries = _dataService.LoadAlltoDoes();
                _allToDoes = new List<ToDoViewModel>();
                foreach (ToDoEntry entry in allEntries)
                {
                    _allToDoes.Add(new ToDoViewModel(entry, _dataService));
                }
                List<ToDoViewModel> _overdueToDoes = new List<ToDoViewModel>();
                List<ToDoViewModel> _teamToDoes = new List<ToDoViewModel>();

                foreach (ToDoViewModel viewmodel in _allToDoes)
                {
                    if (!viewmodel.IsDone)
                    {
                        if (viewmodel.DueDateDateTime < DateTime.Now)
                        {
                            _overdueToDoes.Add(viewmodel);
                        }
                        else
                        {
                            _teamToDoes.Add(viewmodel);
                        }
                    }

                }
                Progress = (int)Math.Round((double)(100 * _allToDoes.Where(p => p.IsDone == true).Count()) / _allToDoes.Count());
                TeamToDoes = new ObservableCollection<ToDoViewModel>(_teamToDoes);
                OverdueToDoes = new ObservableCollection<ToDoViewModel>(_overdueToDoes);
            }

            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
        /// <summary>
        /// Wird aufgerufen wenn zur OverView navigiert wird, lädt den aktuellen stand von der Database
        /// </summary>
        /// <param name="navigationContext"></param>
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            _ = LoadAllToDoes();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        /// <summary>
        /// Wird aufgerufen wenn die View verlassen wird
        /// </summary>
        /// <param name="navigationContext"></param>
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            return;
        }
    }
}

