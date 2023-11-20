using System;

namespace VenturaSQL
{
    /// <summary>
    /// Contains the Release Date and time in UTC format.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    public sealed class ReleaseDateAttribute : Attribute
    {
        private DateTime _release_date;

        /// <summary>
        /// Specify the release date and time in UTC format.
        /// </summary>
        public ReleaseDateAttribute(int year, int month, int day, int hour, int minute, int second)
        {
            _release_date = new DateTime(year, month, day, hour, minute, second, DateTimeKind.Utc);
        }

        public DateTime ReleaseDate
        {
            get { return _release_date; }
        }

    }
}