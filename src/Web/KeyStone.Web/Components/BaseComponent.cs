using KeyStone.Web.Contracts;
using KeyStone.Web.Pages.Account;
using KeyStone.Web.StateFactory;
using Microsoft.AspNetCore.Components;

namespace KeyStone.Web.Components
{
    public class BaseComponent : ComponentBase
    {
        [Inject] protected NavigationManager NavigationManager { get; set; }
        [Inject] protected ILocalStorageProvider LocalStorage { get; set; }
        [Inject] protected IAccountManagement AccountManagement { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (this.Equals(typeof(Login)))
                return;

            bool isAuthenticated = await AccountManagement.CheckAuthenticatedAsync();
            if (!isAuthenticated)
            {
                await AccountManagement.LogoutAsync();
                NavigationManager.NavigateTo("/login");
            }
        }
    }
}