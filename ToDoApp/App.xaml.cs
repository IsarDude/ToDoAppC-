using Prism.Ioc;
using Prism.Modularity;
using System.Windows;
using ToDo.Core;
using ToDo.Core.Services;
using ToDoApp.Module.PersonModule;
using ToDoApp.Views;

namespace ToDoApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        /// <summary>
        /// Creates Shell and Sets Container to MainWindow;
        /// </summary>
        /// <returns></returns>
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        /// <summary>
        /// Register Dataservice As SingletonType
        /// </summary>
        /// <param name="containerRegistry"></param>
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<DataService>();
        }
        /// <summary>
        /// Registriere die Modules für ihre Initaliesierung und wietere verwendung durch Prism
        /// </summary>
        /// <param name="moduleCatalog"></param>
        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<PersonModule>();
            moduleCatalog.AddModule<CoreModule>();
        }
    }
}
