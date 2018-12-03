using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class DecryptFile : AbstractDecryptFile
    {
        private List<Task> Tasks { get; set; } = new List<Task>();

        public DecryptFile()
            :base()
        {
        }

        public DecryptFile(String path)
            :base(path)
        {
        }

        public DecryptFile(String path, String extendName)
            :base(path, extendName)
        {
        }

        protected override String GetFileDecrpyData(String filename)
        {
            return base.GetFileDecrpyData(filename);
        }

        protected override void DecrpyFile(String filePath)     //耗时操作，交由线程处理
        {
            //base.DecrpyFile(filePath);
            Tasks.Add(Task.Factory.StartNew(() => base.DecrpyFile(filePath)));
        }

        protected override void DecrpyDirectory(String dirPath)
        {         
                String newPath = GetNewPath(dirPath, DefaultRootPath, ExtendName);
                Directory.CreateDirectory(newPath);
                Console.WriteLine($"Create Direcotry:{newPath}");
                //遍历子文件
                Directory.GetFiles(dirPath).ToList().ForEach(filename => DecrpyFile(filename));
                //遍历子目录
                Directory.GetDirectories(dirPath).ToList()
                    .ForEach(dirname =>  DecrpyDirectory(dirname));
        }

        public override void Decrpy()
        {
            Init();
            if (EncrpyFilePath.IsValidFile())
            {
                DecrpyFile(EncrpyFilePath);
            }
            else if (EncrpyFilePath.IsValidDirectory())
            {
                DecrpyDirectory(EncrpyFilePath);
            }
            Task.WaitAll(Tasks.ToArray());
            Console.WriteLine("Decrpy...");
        }

        public bool IsFinish()
        {
            Task.WaitAll(Tasks.ToArray());
            //Task.Factory.ContinueWhenAll(Tasks.ToArray(), tasks => Console.WriteLine("Decrpy finish!"));
            return true;
        }

        public override void Backup(String path)
        {
            base.Backup(path);
        }

    }
}
