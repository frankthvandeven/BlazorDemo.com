using Kenova.WebAssembly.Client.Components;
using System.Collections.Generic;

namespace BlazorDemo.Client
{

    public static class UnitedStates
    {
        private static List<State> _list = null;
        private static ListItemCollection<string> _collection = null;

        static UnitedStates()
        {
            _list = new List<State>(52);
            _list.Add(new State() { Abbreviation = "AL", Name = "Alabama" });
            _list.Add(new State() { Abbreviation = "AK", Name = "Alaska" });
            _list.Add(new State() { Abbreviation = "AR", Name = "Arkansas" });
            _list.Add(new State() { Abbreviation = "AZ", Name = "Arizona" });
            _list.Add(new State() { Abbreviation = "CA", Name = "California" });
            _list.Add(new State() { Abbreviation = "CO", Name = "Colorado" });
            _list.Add(new State() { Abbreviation = "CT", Name = "Connecticut" });
            _list.Add(new State() { Abbreviation = "DC", Name = "District of Columbia" });
            _list.Add(new State() { Abbreviation = "DE", Name = "Delaware" });
            _list.Add(new State() { Abbreviation = "FL", Name = "Florida" });
            _list.Add(new State() { Abbreviation = "GA", Name = "Georgia" });
            _list.Add(new State() { Abbreviation = "HI", Name = "Hawaii" });
            _list.Add(new State() { Abbreviation = "ID", Name = "Idaho" });
            _list.Add(new State() { Abbreviation = "IL", Name = "Illinois" });
            _list.Add(new State() { Abbreviation = "IN", Name = "Indiana" });
            _list.Add(new State() { Abbreviation = "IA", Name = "Iowa" });
            _list.Add(new State() { Abbreviation = "KS", Name = "Kansas" });
            _list.Add(new State() { Abbreviation = "KY", Name = "Kentucky" });
            _list.Add(new State() { Abbreviation = "LA", Name = "Louisiana" });
            _list.Add(new State() { Abbreviation = "ME", Name = "Maine" });
            _list.Add(new State() { Abbreviation = "MD", Name = "Maryland" });
            _list.Add(new State() { Abbreviation = "MA", Name = "Massachusetts" });
            _list.Add(new State() { Abbreviation = "MI", Name = "Michigan" });
            _list.Add(new State() { Abbreviation = "MN", Name = "Minnesota" });
            _list.Add(new State() { Abbreviation = "MS", Name = "Mississippi" });
            _list.Add(new State() { Abbreviation = "MO", Name = "Missouri" });
            _list.Add(new State() { Abbreviation = "MT", Name = "Montana" });
            _list.Add(new State() { Abbreviation = "NE", Name = "Nebraska" });
            _list.Add(new State() { Abbreviation = "NH", Name = "New Hampshire" });
            _list.Add(new State() { Abbreviation = "NJ", Name = "New Jersey" });
            _list.Add(new State() { Abbreviation = "NM", Name = "New Mexico" });
            _list.Add(new State() { Abbreviation = "NY", Name = "New York" });
            _list.Add(new State() { Abbreviation = "NC", Name = "North Carolina" });
            _list.Add(new State() { Abbreviation = "NV", Name = "Nevada" });
            _list.Add(new State() { Abbreviation = "ND", Name = "North Dakota" });
            _list.Add(new State() { Abbreviation = "OH", Name = "Ohio" });
            _list.Add(new State() { Abbreviation = "OK", Name = "Oklahoma" });
            _list.Add(new State() { Abbreviation = "OR", Name = "Oregon" });
            _list.Add(new State() { Abbreviation = "PA", Name = "Pennsylvania" });
            _list.Add(new State() { Abbreviation = "RI", Name = "Rhode Island" });
            _list.Add(new State() { Abbreviation = "SC", Name = "South Carolina" });
            _list.Add(new State() { Abbreviation = "SD", Name = "South Dakota" });
            _list.Add(new State() { Abbreviation = "TN", Name = "Tennessee" });
            _list.Add(new State() { Abbreviation = "TX", Name = "Texas" });
            _list.Add(new State() { Abbreviation = "UT", Name = "Utah" });
            _list.Add(new State() { Abbreviation = "VT", Name = "Vermont" });
            _list.Add(new State() { Abbreviation = "VA", Name = "Virginia" });
            _list.Add(new State() { Abbreviation = "WA", Name = "Washington" });
            _list.Add(new State() { Abbreviation = "WV", Name = "West Virginia" });
            _list.Add(new State() { Abbreviation = "WI", Name = "Wisconsin" });
            _list.Add(new State() { Abbreviation = "WY", Name = "Wyoming" });

            _collection = new();

            foreach (var state in _list)
                _collection.Add(state.Abbreviation, state.Name);

        }

        /// <summary>
        /// Return a generic List of all U.S. states
        /// </summary>
        public static List<State> GenericList
        {
            get { return _list; }
        }

        /// <summary>
        /// Return a ListItemCollection of all U.S. states
        /// </summary>
        public static ListItemCollection<string> ItemCollection
        {
            get { return _collection; }
        }


    }

    public class State
    {
        public string Name { get; set; }
        public string Abbreviation { get; set; }
    }

}
