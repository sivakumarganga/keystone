using KeyStone.Web.Components.Dialogs;
using MudBlazor;

namespace KeyStone.Web.Services
{
    public class CustomDialogService
    {
        private readonly IDialogService _dialogService;

        public CustomDialogService(IDialogService dialogService)
        {
            this._dialogService = dialogService;
        }

        public async Task ShowPdfViewerAsync(string title, string documentId)
        {
            var options = new DialogOptions
            {
                MaxWidth = MaxWidth.Medium,
                FullWidth = true,
                CloseButton = true,
                BackdropClick = false,
            };
            var parameters = new DialogParameters<PdfViewer>
            {
                { x => x.Title, title },
                { x => x.DocumentId, documentId }
            };
            await _dialogService.ShowAsync<PdfViewer>(title, parameters, options);
        }


        public async Task<bool> ShowConfirmationDialog(string title, string message)
        {
            DialogOptions options = new()
            {
                MaxWidth = MaxWidth.Medium,
                FullWidth = false,
                BackdropClick = false
            };
            var parameters = new DialogParameters<ConfirmationDialog>
            {
                { x => x.Title, title },
                { x => x.ContentText, message },
            };
            var dialog = await _dialogService.ShowAsync<ConfirmationDialog>(title, parameters, options);
            var result = await dialog.Result;
            return !result.Canceled;
        }

        /// <summary>
        /// Shows an auto closable info dialog  
        /// </summary>
        /// <param name="title">The title</param>
        /// <param name="message">The message</param>
        /// <param name="autoCloseInterval">Auto-close interval in milliseconds</param>
        /// <returns><see cref="Task"/></returns>
        public async Task ShowInfoDialog(string title, string message, int autoCloseInterval = -1)
        {
            DialogOptions options = new() { MaxWidth = MaxWidth.Small, FullWidth = false };
            var parameters = new DialogParameters<InfoDialog>
            {
                { x => x.Title, title },
                { x => x.ContentText, message }
            };
            var dialog = await _dialogService.ShowAsync<InfoDialog>(title, parameters, options);
            if (autoCloseInterval > 0)
            {
                await Task.Delay(autoCloseInterval);
                dialog.Close();
            }
        }
    }
}
