using Kenova.Client.SystemComponents;
using Microsoft.AspNetCore.Components;

namespace Kenova.Client.Components
{
    public partial class PortalUserPanel : KenovaDialogBase
    {
		[Inject]
		private NavigationManager NavigationManager { get; set; }

		protected override void OnDialogInitialized()
		{
		}


		private async Task btnSignOutClickedAsync()
		{
			await LayerManager.AbortAllAsync();

			await KenovaClientConfig.AuthenticationStateProvider.SignOutAsync();
		}

		private async Task btnSignInClickedAsync()
		{
			await NavigationManager.PortalNavigateToAsync($"auth/login?returnto={Uri.EscapeDataString(NavigationManager.Uri)}");
		}

	}
}
