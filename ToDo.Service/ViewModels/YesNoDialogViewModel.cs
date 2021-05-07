using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;

namespace ToDo.Service.ViewModels
{
    /// <summary>
    /// DialogView Model für ja oder nein Abfragen
    /// </summary>
    public class YesNoDialogViewModel : BindableBase, IDialogAware
    {

        /// <summary>
        /// Delegate command zum Schlißen des Dialoges 
        /// </summary>
        private DelegateCommand<string> _closeDialogCommand;

        /// <summary>
        /// Dioalog command zum Schließen des Dialoges
        /// </summary>
        public DelegateCommand<string> CloseDialogCommand =>
            _closeDialogCommand ?? (_closeDialogCommand = new DelegateCommand<string>(CloseDialog));

        private string _message;

        /// <summary>
        /// Text der im Dialog angezeigt wird
        /// </summary>
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        private string _title = "Notification";

        /// <summary>
        /// Title der im dialogfenster angezeigt wird
        /// </summary>
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        /// <summary>
        /// event das beim Schlißen der Request aus gelöst wird
        /// </summary>
        public event Action<IDialogResult> RequestClose;

        /// <summary>
        /// Funktion die beim Schlißen des Dialogs ausgeführt wird, Gibt die Entsprechnede Antwort zurück
        /// </summary>
        /// <param name="parameter"></param>
        protected virtual void CloseDialog(string parameter)
        {
            ButtonResult result = ButtonResult.None;

            if (parameter?.ToLower() == "true")
                result = ButtonResult.OK;
            else if (parameter?.ToLower() == "false")
                result = ButtonResult.Cancel;

            RaiseRequestClose(new DialogResult(result));
        }

        /// <summary>
        /// Gibt die entsprechnede Result zurück
        /// </summary>
        /// <param name="dialogResult"></param>
        public virtual void RaiseRequestClose(IDialogResult dialogResult)
        {
            RequestClose?.Invoke(dialogResult);
        }

        /// <summary>
        /// Frägt ab ob der Dialog geschlossen werden kann
        /// </summary>
        /// <returns></returns>
        public virtual bool CanCloseDialog()
        {
            return true;
        }

        /// <summary>
        /// Wird Aufgerufen wen der Dialog geschlossen wird
        /// </summary>
        public virtual void OnDialogClosed()
        {

        }

        /// <summary>
        /// Wird aufgerufen, wenn der Dialog geöffnet wird
        /// </summary>
        /// <param name="parameters"></param>
        public virtual void OnDialogOpened(IDialogParameters parameters)
        {
            Message = parameters.GetValue<string>("message");
        }
    }
}
