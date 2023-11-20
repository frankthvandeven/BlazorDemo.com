using System;
using System.Threading.Tasks;

namespace Kenova.Client.Components
{
    public class ModelFieldInfo
    {
        public string PropertyName { get; internal set; }
        public string RemarkText { get; internal set; }
        public bool IsValid { get; internal set; }
        public bool IsValidating { get; internal set; }

        internal Func<ValidateEventArgs, Task> Callback { get; set; }
    }
}
