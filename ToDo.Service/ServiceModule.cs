using Prism.Ioc;
using Prism.Modularity;
using ToDo.Service.ViewModels;
using ToDo.Service.Views;

namespace ToDo.Service
{
    public class ServiceModule : IModule
    {
        /// <summary>
        /// Informiert das Mudule das Initialisiert wurde 
        /// </summary>
        /// <param name="containerProvider"></param>
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        /// <summary>
        /// Registriert DialogViewModel und View als Dialog
        /// </summary>
        /// <param name="containerRegistry"></param>
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialog<YesNoDialogView, YesNoDialogViewModel>("yesNoDialog");
        }
    }
}