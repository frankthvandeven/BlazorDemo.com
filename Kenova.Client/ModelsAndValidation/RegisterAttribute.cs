
#if SKIP_THIS

namespace Kenova.Client.Components
{


    /// <summary>
    /// Register a member (property or field) for the validation process.
    /// The validation event will go to the "protected override async Task ValidateEventAsync(...)" method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class RegisterAttribute : Attribute
    {
        public RegisterAttribute()
        {
        }

    }
}

#endif