namespace Kenova.Client.Components
{
    public delegate void HyperDataEventHandler<ItemType>(HyperDataEventArgs<ItemType> e);


    public class HyperDataEventArgs<ItemType>
    {
        private HyperDataEventAction _action;

        public HyperDataEventArgs(HyperDataEventAction action)
        {
            _action = action;
        }

        public HyperDataEventAction Action
        {
            get { return _action; }
        }

    }

    public enum HyperDataEventAction
    {
        //
        // Summary:
        // An item was added to the collection.
        Add = 0,

        //
        // Summary:
        // An item was removed from the collection.
        Remove = 1,

        //
        // Summary:
        // An item was replaced in the collection.
        Replace = 2,

        //
        // Summary:
        // An item was moved within the collection.
        Move = 3,

        //
        // Summary:
        // The content of the collection was cleared.
        Reset = 4
    }

}

