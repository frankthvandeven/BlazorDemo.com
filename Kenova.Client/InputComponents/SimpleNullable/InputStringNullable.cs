namespace Kenova.Client.Components
{
    public class InputStringNullable : InputString
    {

        protected override void ConvertText2Value()
        {
            string completedtext = AutoCompletionTools.AutoCompleteString(this.Text, this.Formatting);

            if (completedtext.Length == 0)
            {
                FieldLink.Value = null;
                return;
            }

            FieldLink.Value = completedtext;
        }

    }
}
