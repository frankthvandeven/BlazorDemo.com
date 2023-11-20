using System;
using System.Collections.Generic;
using System.Text;

namespace VenturaSQL
{
    public interface IRecordBase
    {

        DataRecordStatus RecordStatus
        {
            get;
            set;
        }

        int ModifiedColumnCount();

        bool PendingChanges();

        void ValidateBeforeSaving(int record_index_to_display);

        void WriteChangesToTrackArray(TrackArray track_array);

        void ResetToUnmodified();

        void ResetToUnmodifiedExisting();

        void ResetToExisting();

        void SetIdentityColumnValue(object value);

    }
}
