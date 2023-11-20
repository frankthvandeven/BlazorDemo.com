using System;
using System.Reflection;
using System.Text;

namespace VenturaSQL
{
    public abstract class FrameReaderBase
    {
        /// <summary>
        /// The Buffer.
        /// </summary>
        protected byte[] _buffer;

        /// <summary>
        /// The position in the buffer.
        /// </summary>
        protected int _position;

        private byte[] m_buffer = new byte[16];

        private VenturaSqlPlatform _remote_vsql_platform;
        private Version _remote_vsql_version;

        private string _classname;


        public FrameReaderBase()
        {
            _classname = GetType().FullName;
        }

        public void Exec(byte[] buffer)
        {
            try
            {
                _buffer = buffer;
                _position = 0;

                // Read the header
                if (buffer.Length < 15)
                    throw new VenturaSqlException($"The received data is too short. Expected at least 15 header bytes, but got {_buffer.Length} bytes.");

                byte typeF = this.ReadByte();
                byte typeV = this.ReadByte();

                if (typeF != 70 || typeV != 86)
                    throw new VenturaSqlException("FrameStream data invalid.");

                _remote_vsql_platform = (VenturaSqlPlatform)this.ReadByte();

                // Remote VenturaSQL version.
                int major_version = this.ReadInt32();
                int minor_version = this.ReadInt32();
                int build_version = this.ReadInt32();

                _remote_vsql_version = new Version(major_version, minor_version, build_version);

                // Start processing the frames here, the frames are guaranteed to be integral.
                int expectedposition = _position;

                this.Init();

                while (_position < _buffer.Length)
                {
                    FrameType frametype = (FrameType)this.ReadByte();
                    expectedposition += 1;

                    int PayLoadLength = this.ReadInt32();
                    expectedposition += 4;

                    FrameHandler(frametype, PayLoadLength);

                    expectedposition += PayLoadLength;

                    if (_position < expectedposition)
                        throw new VenturaSqlException($"Under-reading frame. Expected position is {expectedposition} but real position is {_position}. Class {_classname}.");

                    if (_position > expectedposition)
                        throw new VenturaSqlException($"Over-reading frame. Expected position is {expectedposition} but real position is {_position}. Class {_classname}. ");

                } // end while

            }
            finally
            {
                this.Dispose();
            }

        }

        /// <summary>
        /// RemoteVenturaSqlPlatform and RemoteVenturaSqlVersion are already set when Init() is called.
        /// </summary>
        protected abstract void Init();

        protected abstract void FrameHandler(FrameType frametype, int payloadlength);

        protected abstract void Dispose();

        protected VenturaSqlPlatform RemoteVenturaSqlPlatform
        {
            get { return _remote_vsql_platform; }
        }

        protected Version RemoteVenturaSqlVersion
        {
            get { return _remote_vsql_version; }
        }

        /// <summary>
        /// _position will move +1
        /// </summary>
        protected byte ReadByte()
        {
            int readpos = _position;

            _position++;

            return _buffer[readpos];
        }

        /// <summary>
        /// _position will move +count
        /// </summary>
        protected void Read(byte[] dst, int index, int count)
        {
            Buffer.BlockCopy(_buffer, _position, dst, index, count);
            _position += count;
            return;
        }

        /// <summary>
        /// _position will move +count
        /// </summary>
        protected byte[] ReadBytes(int count)
        {
            byte[] dst = new byte[count];
            Buffer.BlockCopy(_buffer, _position, dst, 0, count);
            _position += count;
            return dst;
        }


        /// <summary>
        /// _position will move +4
        /// </summary>
        protected unsafe int ReadInt32()
        {
            fixed (byte* pbyte = &_buffer[_position])
            {
                _position += 4;
                return (*pbyte) | (*(pbyte + 1) << 8) | (*(pbyte + 2) << 16) | (*(pbyte + 3) << 24);
            }
        }

        /// <summary>
        /// _position will move +2
        /// </summary>
        protected unsafe ushort ReadUInt16()
        {
            fixed (byte* pbyte = &_buffer[_position])
            {
                _position += 2;
                return (ushort)((*pbyte) | (*(pbyte + 1) << 8));
            }
        }

        /// <summary>
        /// _position will move +2
        /// </summary>
        protected unsafe short ReadInt16()
        {
            fixed (byte* pbyte = &_buffer[_position])
            {
                _position += 2;
                return (short)((*pbyte) | (*(pbyte + 1) << 8));
            }
        }

        /// <summary>
        /// _position will move +8
        /// </summary>
        protected unsafe long ReadInt64()
        {

            fixed (byte* pbyte = &_buffer[_position])
            {
                _position += 8;

                int i1 = (*pbyte) | (*(pbyte + 1) << 8) | (*(pbyte + 2) << 16) | (*(pbyte + 3) << 24);
                int i2 = (*(pbyte + 4)) | (*(pbyte + 5) << 8) | (*(pbyte + 6) << 16) | (*(pbyte + 7) << 24);
                return (uint)i1 | ((long)i2 << 32);
            }

        }


        /// <summary>
        /// Reads 8 bytes and converts it to a DateTime.
        /// </summary>
        protected DateTime ReadDateTime()
        {
            return new DateTime(ReadInt64());
        }

        protected string ReadString8()
        {
            byte len = this.ReadByte();

            byte[] temp = new byte[len];

            this.Read(temp, 0, len);

            return Encoding.UTF8.GetString(temp, 0, len);

        }

        protected string ReadString16()
        {
            short len = this.ReadInt16();

            byte[] temp = new byte[len];

            this.Read(temp, 0, len);

            return Encoding.UTF8.GetString(temp, 0, len);
        }

        protected string ReadString32()
        {
            int len = this.ReadInt32();

            byte[] temp = new byte[len];

            this.Read(temp, 0, len);

            return Encoding.UTF8.GetString(temp, 0, len);
        }

        protected Version ReadVersion()
        {
            int major = this.ReadInt32();
            int minor = this.ReadInt32();
            int build = this.ReadInt32();
            int revision = this.ReadInt32();

            if (build == -1)
                return new Version(major, minor);

            if (revision == -1)
                return new Version(major, minor, build);

            return new Version(major, minor, build, revision);
        }

        /// <summary>
        /// Reads 16 bytes and converts it to a Guid.
        /// </summary>
        protected Guid ReadGuid()
        {

            return new Guid(this.ReadBytes(16));
        }

        protected RemoteException ReadRemoteException()
        {
            byte flag;
            string message;
            string stacktrace;
            string source;
            RemoteException exception;
            RemoteException innerexception = null;

            flag = this.ReadByte();

            if (flag != 0)
            {
                message = this.ReadString32();
                stacktrace = this.ReadString32();
                source = this.ReadString32();
                innerexception = new RemoteException(message, source, stacktrace, null);
            }

            message = this.ReadString32();
            stacktrace = this.ReadString32();
            source = this.ReadString32();
            exception = new RemoteException(message, source, stacktrace, innerexception);

            return exception;
        }

    } // end of class
} // end of namespace
