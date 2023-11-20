using System;

namespace VenturaSQL
{
    public interface IRecordsetBase
    {
        byte[] Hash
        {
            get;
        }

        string HashString
        {
            get;
        }

        VenturaSqlPlatform GeneratorTarget
        {
            get;
        }

        Version GeneratorVersion
        {
            get;
        }

        DateTime GeneratorTimestamp
        {
            get;
        }

        string GeneratorProviderInvariantName
        {
            get;
        }

        IResultsetBase[] Resultsets
        {
            get;
        }

        string SqlScript
        {
            get;
        }

        VenturaSqlSchema ParameterSchema
        {
            get;
        }

        object[] InputParameterValues
        {
            get;
        }

        object[] OutputParameterValues
        {
            get;
        }

        int RowLimit
        {
            get;
            set;
        }

    }

}
