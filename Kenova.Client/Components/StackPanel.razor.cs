using Microsoft.AspNetCore.Components;
using System.Text;

namespace Kenova.Client.Components
{
    public partial class StackPanel : KenovaComponentBase
    {
        private StringBuilder _container_style = new StringBuilder(100);

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public bool Horizontal { get; set; } = false;

        /// <summary>
        /// Gap between items in pixels.
        /// The default value is -1
        /// </summary>
        [Parameter]
        public double Gap { get; set; } = -1;

        /// <summary>
        /// The default value for this parameter is HeightMode.Container
        /// </summary>
        [Parameter]
        public HeightMode HeightMode { get; set; } = HeightMode.Content;

        [Parameter]
        public double Height { get; set; } = -1;

        [Parameter]
        public double MaxHeight { get; set; } = -1;

        [Parameter]
        public double Width { get; set; } = -1;

        [Parameter]
        public string AdditionalStyle { get; set; } = null;


    }


}
