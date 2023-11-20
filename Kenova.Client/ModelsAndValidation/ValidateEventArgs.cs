
namespace Kenova.Client.Components
{

    public class ValidateEventArgs
    {

        private string _fieldName;
        private bool _isValid;
        private string _remarkText;

        internal ValidateEventArgs()
        {
        }

        public string FieldName
        {
            get { return _fieldName; }
            internal set { _fieldName = value; }
        }

        public bool IsValid
        {
            get { return _isValid; }
            set { _isValid = value; }
        }

        public string RemarkText
        {
            get { return _remarkText; }
            set { _remarkText = value; }
        }

    }


}
