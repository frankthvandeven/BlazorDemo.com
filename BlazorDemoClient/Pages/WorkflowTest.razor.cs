using Kenova.Client.Components;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace BlazorDemo.Client.Pages
{
    public partial class WorkflowTest : KenovaDialogBase
    {

        protected override void OnDialogInitialized()
        {
            Breadcrumb = "WorkflowTest";

        }

    }

}


