using System.IO;
using System.Threading.Tasks;

namespace VenturaSQL
{
    public static partial class Transactional
    {
        private static async Task ExecSql_HttpAsync(HttpConnector connector, params IRecordsetBase[] loaders)
        {
            MemoryStream memorystream = PrepareLoadRequest(connector, loaders);

            byte[] response_array = await ExecuteHttpRequestAsync(connector, memorystream);

            // TODO: Check if the response is valid. Is it binary? Is it recognized as frameData?

            new ClientFrameReader(loaders).Exec(response_array);
        }

        private static MemoryStream PrepareLoadRequest(HttpConnector connector, IRecordsetBase[] loaders)
        {
            // Prepare the request
            MemoryStream memorystream = new MemoryStream();
            FrameWriter framewriter = new FrameWriter(memorystream);

            // tell the remote to open the Sql Connection
            framewriter.StartFrame(FrameType.OpenSqlConnection);
            framewriter.WriteString16(connector.RemoteName);
            framewriter.EndFrame();

            framewriter.StartFrame(FrameType.StartTransaction);
            framewriter.EndFrame();

            for (int i = 0; i < loaders.Length; i++)
            {
                IRecordsetBase loader = loaders[i];

                framewriter.StartFrame(FrameType.InstantiateLoader);
                framewriter.Write(loader.Hash, 0, 16);
                framewriter.WriteString16(loader.GetType().FullName); // the full classname like Ventura.GeneratedRecordsets.T_PatientenRecords
                framewriter.Write(i);
                framewriter.EndFrame();

                LoadHttpProcessOneLoader(framewriter, loader);
            }

            framewriter.StartFrame(FrameType.CommitTransaction);
            framewriter.EndFrame();

            framewriter.StartFrame(FrameType.CloseSqlConnection);
            framewriter.EndFrame();

            framewriter.Close();

            return memorystream;
        }

        /// <summary>
        /// Send a "Load Recordset" instruction plus sql select parameter values to server.
        /// </summary>
        private static void LoadHttpProcessOneLoader(FrameWriter framewriter, IRecordsetBase loader)
        {

            if (loader.ParameterSchema != null)
                loader.ParameterSchema.ObjectArray2Frame(FrameType.SetInputParameters, framewriter, loader.InputParameterValues);

            for (int resultset_index = 0; resultset_index < loader.Resultsets.Length; resultset_index++)
            {
                framewriter.StartFrame(FrameType.SelectResultset);
                framewriter.Write(resultset_index);
                framewriter.EndFrame();

                //IResultsetBase resultset = loader.Resultsets[resultset_index];
            }

            framewriter.StartFrame(FrameType.ExecuteSqlScript);

            var i = loader as IRecordsetIncremental;

            if (i != null)
                framewriter.Write(i.IncrementalOffset);

            framewriter.Write(loader.RowLimit);
            framewriter.EndFrame();
        }

        private static async Task SaveChanges_HttpAsync(HttpConnector connector, params IRecordsetBase[] loaders)
        {
            MemoryStream memorystream = PrepareSaveRequest(connector, loaders);

            byte[] response_array = await ExecuteHttpRequestAsync(connector, memorystream);

            // TODO: Check if the response is valid. Is it binary? Is it recognized as frameData?

            new ClientFrameReader(loaders).Exec(response_array);
        }

        private static MemoryStream PrepareSaveRequest(HttpConnector connector, IRecordsetBase[] loaders)
        {
            // Prepare the request.
            MemoryStream memorystream = new MemoryStream();
            FrameWriter framewriter = new FrameWriter(memorystream);

            // Tell the remote to open the Sql Connection.
            framewriter.StartFrame(FrameType.OpenSqlConnection);
            framewriter.WriteString16(connector.RemoteName);
            framewriter.EndFrame();

            framewriter.StartFrame(FrameType.StartTransaction);
            framewriter.EndFrame();

            for (int i = 0; i < loaders.Length; i++)
            {
                IRecordsetBase loader = loaders[i];

                framewriter.StartFrame(FrameType.InstantiateLoader);
                framewriter.Write(loader.Hash, 0, 16);
                framewriter.WriteString16(loader.GetType().FullName); // the full classname like VenturaSQL.GeneratedRecordsets.T_PatientRecords
                framewriter.Write(i);
                framewriter.EndFrame();

                SaveHttpProcessOneLoader(framewriter, loader);
            }

            framewriter.StartFrame(FrameType.CommitTransaction);
            framewriter.EndFrame();

            framewriter.StartFrame(FrameType.CloseSqlConnection);
            framewriter.EndFrame();

            framewriter.Close();

            return memorystream;
        }

        /// <summary>
        /// Send a "Save Recordset" instruction to server.
        /// </summary>
        private static void SaveHttpProcessOneLoader(FrameWriter framewriter, IRecordsetBase loader)
        {
            for (int resultset_index = 0; resultset_index < loader.Resultsets.Length; resultset_index++)
            {
                IResultsetBase resultset = loader.Resultsets[resultset_index];

                TrackArray trackarray = new TrackArray(resultset.Schema);

                framewriter.StartFrame(FrameType.SelectResultset);
                framewriter.Write(resultset_index);
                framewriter.EndFrame();

                framewriter.StartFrame(FrameType.Instantiate_RowSaver_and_TrackArray);
                framewriter.EndFrame();

                for (int index = 0; index < resultset.Length; index++)
                {
                    trackarray.Reset(); // Reset the TrackArray. Sets the status to Empty.
                    resultset[index].WriteChangesToTrackArray(trackarray);
                    trackarray.WriteToFrame(index, framewriter);
                }

            }
        }

    } // end of class
} // end of namespace
