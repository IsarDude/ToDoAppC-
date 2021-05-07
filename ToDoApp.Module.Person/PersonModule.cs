using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;
using ToDo.Core;
using ToDo.Core.Services;
using ToDo.Service.ViewModels;
using ToDo.Service.Views;
using ToDoApp.Module.PersonModule.ViewModels;
using ToDoApp.Module.PersonModule.Views;

namespace ToDoApp.Module.PersonModule
{
    /// <summary>
    /// Module, dass alle Views und ViewModels von Personen und ihren ToDoes verwaltet
    /// </summary>
    public class PersonModule : IModule
    {
        private readonly IRegionManager _regionManager;
        /// <summary>
        /// Setzt den Prism Region manager
        /// </summary>
        /// <param name="regionManager"></param>
        public PersonModule(IRegionManager regionManager) => _regionManager = regionManager;

        /// <summary>
        /// Initialiesiert die Views und weist ihnen entsprechende Regionen auf der MainWindowView zu
        /// </summary>
        /// <param name="containerProvider"></param>
        public void OnInitialized(IContainerProvider containerProvider)
        {

            _regionManager.RequestNavigate(RegionNames.MainContentRegion, nameof(PersonDetailView));
            _regionManager.RequestNavigate(RegionNames.MainContentRegion, nameof(ToDoListView));
            _regionManager.RequestNavigate(RegionNames.LeftContentRegion, nameof(PersonListView));
            _regionManager.RequestNavigate(RegionNames.MainContentRegion, nameof(OverView));

        }

        /// <summary>
        /// Registriet alle Views in diesem Module für die nutzung mit Navigationsfunktionen von Prism
        /// </summary>
        /// <param name="containerRegistry"></param>
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IDataService, DataService>();
            containerRegistry.RegisterDialog<YesNoDialogView, YesNoDialogViewModel>("yesNoDialog");
            ViewModelLocationProvider.Register<OverView, OverViewViewModel>();
            containerRegistry.RegisterForNavigation<PersonListView>();
            containerRegistry.RegisterForNavigation<PersonDetailView>();
            containerRegistry.RegisterForNavigation<ToDoListView>();
            containerRegistry.RegisterForNavigation<OverView>();

        }
    }
}