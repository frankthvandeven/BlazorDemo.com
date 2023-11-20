using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace VenturaSQL
{
    public static class Hashing
    {
        public static byte[] ComputeHashFromString(string textstring)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] buffer = new ASCIIEncoding().GetBytes(textstring);
            for (int i = 0; i < buffer.Length; i++)
            {
                md5.TransformBlock(buffer, i, 1, buffer, i);
            }
            md5.TransformFinalBlock(buffer, buffer.Length, 0);
            
            return md5.Hash;
        }

        public static byte[] ComputeHashFromFile(string filename)
        {
            FileStream filestream = null;
            
            try
            {
                filestream = new FileStream(filename, FileMode.Open, FileAccess.Read);
                return ComputeHashFromStream(filestream);
            }
            finally
            {
                General.SmartClose(filestream);
            }
        }

        public static byte[] ComputeHashFromStream(Stream stream)
        {
            long origpos = stream.Position;
            
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            Int64 remainingbytes = stream.Length - stream.Position;
            Int32 buffersize = 262144;

            byte[] buffer = new byte[buffersize];

            while (remainingbytes > buffersize)
            {
                int read = stream.Read(buffer, 0, buffersize);
                md5.TransformBlock(buffer, 0, buffersize, buffer, 0);

                remainingbytes -= buffersize;
            }

            stream.Read(buffer, 0, (int)remainingbytes);
            md5.TransformFinalBlock(buffer, 0, (int)remainingbytes);

            stream.Position = origpos;
            
            return md5.Hash; //return BitConverter.ToString(MD5);
        }

        public static bool CompareHashes(byte[] first, byte[] second)
        {
            if (first.Length != second.Length)
                return false;

            for (int x = 0; x < first.Length; x++)
            {
                if (first[x] != second[x])
                    return false;
            }

            return true;
        }


    }
}
