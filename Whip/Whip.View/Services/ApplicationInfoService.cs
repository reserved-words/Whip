using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using Whip.Services.Interfaces.Utilities;

namespace Whip.Services
{
    // Build Date method from Stack Overflow: https://stackoverflow.com/questions/1600962/displaying-the-build-date
    public class ApplicationInfoService : IApplicationInfoService
    {
        private const string VersionNumberFormat = "{0}.{1}.{2}.{3}";

        private readonly Lazy<Assembly> _executingAssembly;
        private readonly Lazy<string> _applicationVersion;
        private readonly Lazy<DateTime> _applicationBuildDate;
        
        public ApplicationInfoService()
        {
            _executingAssembly = new Lazy<Assembly>(() => typeof(App).Assembly);
            _applicationVersion = new Lazy<string>(GetVersion);
            _applicationBuildDate = new Lazy<DateTime>(GetBuildDateTime);
        }

        public string Version => _applicationVersion.Value;
        public DateTime PublishDate => _applicationBuildDate.Value;

        private string GetVersion()
        {
            var version = _executingAssembly.Value.GetName().Version;

            return string.Format(VersionNumberFormat, 
                version.Major,
                version.Minor,
                version.Build,
                version.Revision);
        }
        
        private DateTime GetBuildDateTime()
        {
            var path = _executingAssembly.Value.Location;

            if (!File.Exists(path))
                return new DateTime();

            var buffer = new byte[Math.Max(Marshal.SizeOf(typeof(ImageFileHeader)), 4)];
            using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                fileStream.Position = 0x3C;
                fileStream.Read(buffer, 0, 4);
                fileStream.Position = BitConverter.ToUInt32(buffer, 0); // COFF header offset
                fileStream.Read(buffer, 0, 4); // "PE\0\0"
                fileStream.Read(buffer, 0, buffer.Length);
            }
            var pinnedBuffer = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            try
            {
                var coffHeader = (ImageFileHeader)Marshal.PtrToStructure(pinnedBuffer.AddrOfPinnedObject(), typeof(ImageFileHeader));

                return TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1) + new TimeSpan(coffHeader.TimeDateStamp * TimeSpan.TicksPerSecond));
            }
            finally
            {
                pinnedBuffer.Free();
            }
        }

        private struct ImageFileHeader
        {
            public ushort Machine;
            public ushort NumberOfSections;
            public uint TimeDateStamp;
            public uint PointerToSymbolTable;
            public uint NumberOfSymbols;
            public ushort SizeOfOptionalHeader;
            public ushort Characteristics;
        }
    }
}
