using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public static class Extend
    {
        public static bool IsValidFile(this String path)
        {
            return File.Exists(path);
        }

        public static bool IsValidDirectory(this String path)
        {
            return Directory.Exists(path);
        }

    }

    public interface IDecryptFile
    {

    }

    public abstract class AbstractDecryptFile
    {
        [DllImport("user32.dll")]
        static extern IntPtr FindWindow(string strClass, string strWindow);
        [DllImport("user32.dll", EntryPoint = "FindWindowEx", SetLastError = true)]
        private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
        [DllImport("user32.dll", EntryPoint = "GetDlgItemTextA")]
        private static extern int GetDlgItemText(IntPtr hDlg, int nIDDlgItem, [Out]StringBuilder lpString, int nMaxCount);
        [DllImport("User32.dll")]
        private static extern Int32 SendMessage(IntPtr hWnd, int Msg, int wParam, StringBuilder lParam);
        [DllImport("User32.dll", EntryPoint = "GetWindowTextA")]
        private static extern Int32 GetWindowText(IntPtr hwnd, [Out]StringBuilder lpString, int nMaxCount);
        const int WM_GETTEXT = 0x000D;

        protected string DefaultRootPath { get; set; }
        protected string CopyDefaultRootPath { get; set; }
        protected String EncrpyFilePath { get; set; }
        protected string ExtendName { get; set; } = ".decr";

        #region construtor/setter
        public AbstractDecryptFile()
        {
        }

        public AbstractDecryptFile(String path)
        {
            this.EncrpyFilePath = path;
        }

        public AbstractDecryptFile(String path, String extendName)
        {
            this.EncrpyFilePath = path;
            this.ExtendName = extendName;
        }


        public void SetEncrpyFilePath(String path)
        {
            this.EncrpyFilePath = path;
        }

        public void SetExtendName(String extendName)
        {
            this.ExtendName = extendName;
        }
        #endregion


        public void CheckedParameterNotNull(Object param)
        {
            if (param == null)
            {
                throw new ArgumentNullException();
            }
        }

        /// <summary>
        /// 获取该path的目录路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected String GetDirPath(string path)
        {
            if (path.IsValidFile())
            {
                int pos = path.LastIndexOf('\\');
                return path.Substring(0, pos + 1);
            }
            else if (path.IsValidDirectory())
            {
                return path;
            }
            else
            {
                throw new Exception($"{path} is not exist!");
            }
        }

        protected void Init()
        {
            CheckedParameterNotNull(this.EncrpyFilePath);
            CheckedParameterNotNull(this.ExtendName);

            this.DefaultRootPath = GetDirPath(EncrpyFilePath);
            this.CopyDefaultRootPath = DefaultRootPath + "_enc";
        }

        protected virtual String GetFileDecrpyData(String filename)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo("notepad.exe");
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Arguments = filename;
            //startInfo.StandardOutputEncoding = Encoding.UTF8;
            //startInfo.RedirectStandardOutput = true;
            //startInfo.UseShellExecute = false;
            //Console.WriteLine("Open notepad:");

            using (Process process = Process.Start(startInfo))
            {
                IntPtr hwnd = IntPtr.Zero;
                IntPtr textHwnd = IntPtr.Zero;
                String title = new StringBuilder()
                    .Append(filename.Substring(filename.LastIndexOf('\\') + 1))
                    .Append(" - 记事本")
                    .ToString();
                while (true)
                {
                    if ((hwnd = FindWindow("Notepad", title)) == IntPtr.Zero)
                    {
                        Thread.Sleep(10);
                        continue;
                    }
                    else
                    {
                        //Console.WriteLine($"Get Notepad hwnd:{hwnd}");
                        while ((textHwnd = FindWindowEx(hwnd, IntPtr.Zero, "Edit", null)) == IntPtr.Zero)
                        {
                            Thread.Sleep(10);
                        }
                        //Console.WriteLine($"Get Notepad edit control hwnd:{textHwnd}");
                        break;
                    }
                }
                StringBuilder sbData = new StringBuilder();
                int bufferSize = 1024 * 1024;
                StringBuilder sb = new StringBuilder(bufferSize);
                //Console.WriteLine("Begin read data:");
                SendMessage(textHwnd, WM_GETTEXT, bufferSize, sb);
                sbData.Append(sb);
                process.Kill();
                return sbData.ToString();
            }
        }

        protected String GetNewPath(String path, String basePath, String extendName)
        {
            if (File.Exists(path))
                return path.Replace(DefaultRootPath, CopyDefaultRootPath) + ExtendName;
            else if (Directory.Exists(path))
                return path.Replace(DefaultRootPath, CopyDefaultRootPath);
            else
                throw new Exception($"Invalid path {path}!");
        }

        protected virtual void DecrpyFile(String filePath)
        {
            //创建文件
            string newFilePath = GetNewPath(filePath, this.DefaultRootPath, this.ExtendName);
            //获取数据
            String data = GetFileDecrpyData(filePath);
            File.AppendAllText(newFilePath, data);
            Console.WriteLine($"Create File:{newFilePath}");
        }

        protected virtual void DecrpyDirectory(String dirPath)
        {
            string[] childFiles = Directory.GetFiles(dirPath);
            string[] childDirs = Directory.GetDirectories(dirPath);
            //创建目录
            string newDirPath = GetNewPath(dirPath, this.DefaultRootPath, this.CopyDefaultRootPath);
            Directory.CreateDirectory(newDirPath);
            Console.WriteLine($"Create Direcotry:{newDirPath}");

            //遍历子文件
            foreach (string file in childFiles)
            {
                DecrpyFile(file);
            }

            //遍历子目录
            foreach (string file in childDirs)
            {
                DecrpyDirectory(file);
            }
        }

        public virtual void Decrpy()
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
        }


        protected void RenameFile(String filename)
        {
            String pathExtend = filename.Substring(filename.Length - ExtendName.Length, ExtendName.Length);
            if (pathExtend == ExtendName)
            {
                File.Move(filename, filename.Substring(0, filename.Length - ExtendName.Length));
            }
        }

        protected void RenameDir(String dirName)
        {
            //如果dirName是被修改过的 还原
            String pathExtend = dirName.Substring(dirName.Length - ExtendName.Length, ExtendName.Length);
            if (pathExtend == ExtendName)
            {
                String newDirName = dirName.Substring(0, dirName.Length - ExtendName.Length);
                Directory.Move(dirName, newDirName);
                dirName = newDirName;
            }
            String[] childFiles = Directory.GetFiles(dirName);
            foreach (String file in childFiles)
            {
                RenameFile(file);
            }

            String[] childDirs = Directory.GetDirectories(dirName);
            foreach (String dir in childDirs)
            {
                RenameDir(dir);
            }
        }

        public virtual void Backup(String path = "")
        {
            if (path == "")
                path = CopyDefaultRootPath;
            if (path.IsValidDirectory())
            {
                RenameDir(path);
            }
            else if (path.IsValidFile())
            {
                RenameFile(path);
            }
            else
            {
                throw new ArgumentException($"{path} is a invalid path!");
            }
        }

    }
}
