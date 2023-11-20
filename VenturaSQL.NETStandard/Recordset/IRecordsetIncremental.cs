using System;
using System.Threading.Tasks;

namespace VenturaSQL
{
    public interface IRecordsetIncremental
    {

        Connector IncrementalConnector
        {
            get;
            set;
        }

        int IncrementalOffset
        {
            get;
            set;
        }

        int LastExecCount
        {
            get;
            set;
        }

        int LastExecStartIndex
        {
            get;
            set;
        }

        bool HasMoreRows
        {
            get;
            set;
        }

        Task ExecSqlIncrementalAsync();
        void ExecSqlIncremental();

        int RowLimit
        {
            get;
            set;
        }
    }

}
