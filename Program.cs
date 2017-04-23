using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace LOSTDIRRecover
{
    public struct FileType
    {
        public string Extension;
        public byte[] Signature;

        public FileType(string e, byte[] s)
        {
            Extension = e; Signature = s;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage : LOSTDIRRecover.exe {LOST.DIR Path} {New Path}");
                return;
            }

            string LOSTDIRPath, newPath;
            LOSTDIRPath = args[0];
            newPath = args[1];

            if (!newPath.EndsWith(@"\")) newPath += "\\";
            if (!Directory.Exists(newPath)) Directory.CreateDirectory(newPath);

            List<FileType> list = new List<FileType>();
            initList(ref list);
            var files = Directory.GetFiles(LOSTDIRPath);
            foreach (var file in files)
            {
                FileStream fs = new FileStream(file, FileMode.Open);
                foreach (var type in list)
                {

                    byte[] sig = new byte[type.Signature.Length];
                    fs.Read(sig, 0, sig.Length);
                    if (type.Signature.SequenceEqual(sig))
                    {
                        fs.Close();
                        Console.WriteLine(string.Format("MATCH :: {0} is {1} file.", file, type.Extension));
                        File.Move(file, string.Format("{0}{1}.{2}", newPath, file.Substring(file.LastIndexOf(@"\")), type.Extension));
                        break;
                    }
                    fs.Seek(0, SeekOrigin.Begin);

                }
                fs.Close();
            }
            Console.WriteLine("Done!!");
        }

        static void initList(ref List<FileType> list)
        {
            list.Add(new FileType("mp3", new byte[] { 0x49, 0x44, 0x33 }));
            list.Add(new FileType("mp3", new byte[] { 0xff, 0xfb, 0x90, 0x64 })); //LAME
            list.Add(new FileType("flac", new byte[] { 0x66, 0x4C, 0x61, 0x43, 0, 0, 0, 0x22 }));
            list.Add(new FileType("m4a", new byte[] { 0, 0, 0, 0x20, 0x66, 0x74, 0x79, 0x70, 0x4D, 0x34, 0x41 }));
            list.Add(new FileType("wav", new byte[] { 0x52, 0x49, 0x46, 0x46 }));
            list.Add(new FileType("wma", new byte[] { 0x30, 0x26, 0xB2, 0x75, 0x8E, 0x66, 0xCF, 0x11 }));
            list.Add(new FileType("jpg", new byte[] { 0xff, 0xd8, 0xff, 0xdb }));
            list.Add(new FileType("jpg", new byte[] { 0xff, 0xd8, 0xff, 0xe0 }));
            list.Add(new FileType("jpg", new byte[] { 0xff, 0xd8, 0xff, 0xe1 }));
            list.Add(new FileType("jpg", new byte[] { 0xff, 0xd8, 0xff, 0xe2 }));
            list.Add(new FileType("jpg", new byte[] { 0xff, 0xd8, 0xff, 0xe3 }));
            list.Add(new FileType("png", new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }));
            list.Add(new FileType("gif", new byte[] { 0x47, 0x49, 0x46, 0x38 }));
            list.Add(new FileType("3gp", new byte[] { 0, 0, 0, 0x18, 0x66, 0x74, 0x79, 0x70 }));
            list.Add(new FileType("zip", new byte[] { 0x1f, 0x8b, 0x08 }));

        }
    }
}