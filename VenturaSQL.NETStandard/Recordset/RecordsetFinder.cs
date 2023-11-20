using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

//using RecordsetBase = Ventura.RecordsetBase<Ventura.RecordBase>;

namespace VenturaSQL
{

    /* 
    Note: A generic list can safely be read by multiple threads.

    FYI: Locking boils down to the Intel CPU's CMPXCHG instruction.
    
    Locking related functions in .Net:
    Interlocked.CompareExchange, Thread.SpinWait, Thread.Yield, Thread.Sleep(0) and Thread.Sleep(1)

    Interesting info on locking: http://blog.coverity.com/2014/03/12/can-skip-lock-reading-integer/#.VZ9chssVgpo

    Always lock every access to a field that can be shared across threads. Code which avoids locks is just too difficult to maintain.
    */

    public static class RecordsetFinder
    {
        private static volatile List<LoaderInfo> _recordsets = new List<LoaderInfo>();
        private static readonly object _syncroot = new object();
        private static volatile bool _scancompleted = false;

        private static string _scanResult = "Scan not started yet.";

        private enum CompareMode
        {
            Exact = 0,
            BeginsWith = 1
        }

        public static LoaderInfo[] GetRecordsetListAsArray()
        {
            if (_scancompleted == false)
                PerformScan();

            return _recordsets.ToArray();
        }

        public static string Scanresult
        {
            get { return _scanResult; }
        }

        public static IRecordsetBase FindLoader(byte[] hash, string fullclassname)
        {
            if (_scancompleted == false)
            {
                PerformScan();
            }

            Type classtype = null;

            for (int i = 0; i < _recordsets.Count; i++)
            {
                LoaderInfo loaderinfo = _recordsets[i];

                if (Hashing.CompareHashes(hash, loaderinfo.Hash) == true)
                {
                    classtype = loaderinfo.ClassType;
                    break;
                }
            }

            if (classtype == null)
            {
                for (int i = 0; i < _recordsets.Count; i++)
                {
                    LoaderInfo loaderinfo = _recordsets[i];

                    if (fullclassname == loaderinfo.FullClassname)
                    {
                        classtype = loaderinfo.ClassType;
                        break;
                    }
                }
            }

            if (classtype == null)
            {
                int x = fullclassname.LastIndexOf('.');

                string shortclassname;

                if (x != -1)
                    shortclassname = fullclassname.Substring(x + 1);
                else
                    shortclassname = fullclassname;

                for (int i = 0; i < _recordsets.Count; i++)
                {
                    LoaderInfo recordsetinfo = _recordsets[i];

                    if (shortclassname == recordsetinfo.ShortClassname)
                    {
                        classtype = recordsetinfo.ClassType;
                        break;
                    }
                }
            }

            if (classtype == null)
                return null;

            return (IRecordsetBase)Activator.CreateInstance(classtype);

        } // end of method

        private static int _stats_assemblies_scanned = 0;

        public static void PerformScan()
        {
            if (_scancompleted)
                throw new Exception("RecordsetFinder.PerformScan() should only be called once.");

            lock (_syncroot)
            {
                if (_scancompleted == false)
                {
                    var stopwatch = Stopwatch.StartNew();

                    IterateAllAssemblies();

                    stopwatch.Stop();

                    _scanResult = $"Scanned {_stats_assemblies_scanned} assemblies and found {_recordsets.Count} recordsets. Scan took {stopwatch.ElapsedMilliseconds} milliseconds.";

                    _scancompleted = true;
                }
            }

        }



        private static void IterateAllAssemblies()
        {

            Assembly[] all_assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly assembly in all_assemblies)
            {
                ScanInsideAssembly(assembly);
                _stats_assemblies_scanned++;
            }
        }

        private static void ScanInsideAssembly(Assembly assembly)
        {

            // Skip this assembly due to "SqlGuidCaster" problem
            if (assembly.ManifestModule.Name == "Microsoft.Data.SqlClient.dll")
            {
                return;
            }

            Type[] types = assembly.GetTypes();

            // Determine which classes implement Ventura.IRecordsetBase
            foreach (Type type in types)
            {
                if (type.IsClass && typeof(IRecordsetBase).IsAssignableFrom(type))
                {
                    LoaderInfo loaderinfo = new LoaderInfo();
                    loaderinfo.FullClassname = type.FullName;

                    int x = loaderinfo.FullClassname.LastIndexOf('.');

                    if (x != -1)
                        loaderinfo.ShortClassname = loaderinfo.FullClassname.Substring(x + 1);
                    else
                        loaderinfo.ShortClassname = loaderinfo.FullClassname;

                    loaderinfo.ClassType = type;

                    IRecordsetBase rs = (IRecordsetBase)Activator.CreateInstance(type);

                    loaderinfo.Hash = rs.Hash;

                    _recordsets.Add(loaderinfo);
                }
            }
        }

    } // end of static class

    public struct LoaderInfo
    {
        public string FullClassname;
        public string ShortClassname;
        public byte[] Hash;
        public Type ClassType;

        public string GetHashString()
        {
            byte[] bytes = Hash;

            string hexString = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                hexString += bytes[i].ToString("X2");
            }
            return hexString;
        }
    }

} // end of namespace
