using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using ToDo.Core;
using ToDoApp.Module.PersonModule.Views;

namespace ToDoApp.ViewModels
{
    /// <summary>
    /// Main Window auf dem siche die die Verscheidenen RegionenBefinden
    /// </summary>
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "ToDO List";
        private readonly IRegionManager _regionManger;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        /// <summary>
        /// Constructor Initialiesiert die Person Detail view und ToDoList View und Navigiert zu Ihnen
        /// </summary>
        /// <param name="regionManager"></param>
        public MainWindowViewModel(IRegionManager regionManager)
        {
            _regionManger = regionManager;

        }
        /// <summary>
        /// elegateCommand um die Overview in der MainContentRegion anzuzeigen
        /// </summary>
        public DelegateCommand GoToOverView => new DelegateCommand(() => _regionManger.RequestNavigate(RegionNames.MainContentRegion, nameof(OverView)));
        /// <summary>
        /// DelegateCommand um die ToDoListView in der MainContentRegion anzuzeigen
        /// </summary>
        public DelegateCommand GoToToDoView => new DelegateCommand(() => _regionManger.RequestNavigate(RegionNames.MainContentRegion, nameof(ToDoListView)));
        /// <summary>
        /// Delegate Command um die PersonDietailView in der MainContetRegion anzuzeigen
        /// </summary>
        public DelegateCommand GoToPersonDetailView => new DelegateCommand(() => _regionManger.RequestNavigate(RegionNames.MainContentRegion, nameof(PersonDetailView)));
    }
}
