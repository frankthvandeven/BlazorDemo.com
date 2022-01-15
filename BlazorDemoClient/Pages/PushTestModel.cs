using Kenova.Client.Components;
using System.Threading.Tasks;

namespace BlazorDemo.Client.Pages
{

    [ViewModel]
    public partial class PushTestModel
    {

        private string __Message;

        public PushTestModel()
        {
            Register(m => m.Message);

            Message = "Message from Client";
        }

        protected override async Task ValidateEventAsync(ValidateEventArgs<PushTestModel> e)
        {
            if (e.IsMember(m => m.Message))
            {
                await Task.CompletedTask;
                //e.RemarkText = $"Timer ticks {DateTime.Now.Ticks}";
                return;
            }

        }

    }
}
