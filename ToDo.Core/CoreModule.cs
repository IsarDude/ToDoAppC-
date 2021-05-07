using Prism.Ioc;
using Prism.Modularity;
using ToDo.Core.Services;

namespace ToDo.Core
{
    /// <summary>
    /// Module für alle Core und Service Klassen
    /// </summary>
    public class CoreModule : IModule
    {
        /// <summary>
        /// Wird bei Initialiesierung ausgelöst
        /// </summary>
        /// <param name="containerProvider"></param>
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        /// <summary>
        /// REgistreit Klassen für die Benutzung in anderen Modules 
        /// </summary>
        /// <param name="containerRegistry"></param>
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IDataService, DataService>();
        }
    }
}