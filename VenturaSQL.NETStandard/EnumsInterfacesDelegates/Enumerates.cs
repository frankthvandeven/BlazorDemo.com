
namespace VenturaSQL
{

    public enum VenturaSqlPlatform : byte 
    {
        NETStandard = 0
    }

    /// <summary>
    /// Indicates the type of frame received/sent
    /// </summary>
    public enum FrameType : byte
    {
        /// <summary>
        /// Tells the server that a transaction starts. 
        /// Frame also contains connector information.
        /// </summary>
        OpenSqlConnection = 10,
        CloseSqlConnection = 12,

        StartTransaction = 20,
        CommitTransaction = 22,

        /// <summary>
        /// Instructs the server to create a Loader class based on Hash and ClassName.
        /// </summary>
        InstantiateLoader = 30,

        Instantiate_RowSaver_and_TrackArray = 34,

        ExecuteSqlScript = 40,
        SetInputParameters = 42,
        SetOutputParameters = 44,
        SetRowOffset = 46,

        SelectLoader = 50,
        SelectResultset = 52,
        IncreaseResultsetCapacity = 54,

        UnselectResultset = 62,

        Record_Unchanged = 70,
        TrackArray = 72,
        IdentityColumnValue = 74,

        Exception = 80,
        SuccessMessage = 81,
        InfoMessage = 82,
        WarningMessage = 83,
        ErrorMessage = 84

    } // end of enum

    public enum DataRecordStatus : byte
    {
        New = 0,
        Existing = 1,
        ExistingDelete = 2
    }

    public enum TrackArrayStatus : byte
    {
        Empty = 0,
        DataForUPDATE = 1,
        DataForINSERT = 2,
        DataForDELETE = 3
    }

    public enum LogType
    {
        Error = 1,
        Warning = 2,
        Information = 4,
        Audit = 8
    }

} // end of namespace
