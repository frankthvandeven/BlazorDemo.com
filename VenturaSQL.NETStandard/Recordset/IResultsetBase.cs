namespace VenturaSQL
{
    public interface IResultsetBase
    {

        VenturaSqlSchema Schema
        {
            get;
            set;
        }

        string UpdateableTablename
        {
            get;
            set;
        }

        void Clear();

        void IncreaseCapacity(int additional_capacity);

        void OptimizedCreateAndAppendExistingRecord(object[] columnvalues);
        void OptimizedCreateAndSetExistingRecord(int index, object[] columnvalues);

        void OnAfterExecSql();
        void OnAfterSaveChanges();

        IRecordBase this[int index] { get; }

        int Length { get; }


    }
}
