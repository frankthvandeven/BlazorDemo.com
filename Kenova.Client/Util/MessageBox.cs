using System.Threading.Tasks;

namespace Kenova.Client.Components
{
    public static class MessageBox
    {

        public static async Task ShowAsync(string caption, string message)
        {
            var ld = new LayerDefinition<MessageBoxComponent>
            {
                Kind = LayerKind.ModalWindow,
                [i => i.Caption] = caption,
                [i => i.Message] = message
            };

            var result = await ld.OpenThenWaitForCloseAsync();

            //if (result.Cancelled)
            //    return;



        }



    }
}
