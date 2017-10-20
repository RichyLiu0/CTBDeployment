using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ucsmy.Framework;
using Ucsmy.Framework.Common;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace DeploymentUtility
{
    public class FileScanCommon
    {

        public static Dictionary<string, string> RemoteUsers = new Dictionary<string, string>() {
        {"local","0"},//0为本地
        {"01","1,ucs2gyjj,UCS,Ymxt52066"},//1开头为远程
        {"02","1,ucswebctl,UCS,2015@ucsmy"}, };//1开头为远程

        public static class WinLogonHelper
        {
            [System.Runtime.InteropServices.DllImport("advapi32.DLL", SetLastError = true)]
            public static extern int LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);
        }
        public static T WinLogonDo<T>(string userType, Func<T> doSomthing)
        {
            var result = default(T);
            var userInfos = RemoteUsers[userType].Split(',');
            if (userInfos[0] == "1")
            {
                IntPtr admin_token = default(IntPtr);
                System.Security.Principal.WindowsIdentity wid_admin = null;
                System.Security.Principal.WindowsImpersonationContext wic = null;

                //在程序中模拟域帐户登录
                //if (WinLogonHelper.LogonUser("uid", "serverdomain", "pwd", 9, 0, ref admin_token) != 0)
                //if (WinLogonHelper.LogonUser("ucs2gyjj", "UCS", "Ymxt52066", 9, 0, ref admin_token) != 0)
                if (WinLogonHelper.LogonUser(userInfos[1], userInfos[2], userInfos[3], 9, 0, ref admin_token) != 0)
                {
                    using (wid_admin = new System.Security.Principal.WindowsIdentity(admin_token))
                    {
                        using (wic = wid_admin.Impersonate())
                        {
                            result = doSomthing();
                        }
                    }
                }
            }
            else
            {
                result = doSomthing();
            }
            return result;
        }

        public static string FileScanCopy(FileScanInfo fsInfo, string newPath)
        {
            if (newPath.IsNullOrWhiteSpace())
            {
                throw new Exception("目标文件路径不正确");
            }

            if (!fsInfo.IsLeaf)
                System.IO.Directory.CreateDirectory(newPath);
            else
            {
                System.IO.File.Copy(fsInfo.FullName, newPath);
            }
            if (fsInfo.Children.Any())
            {
                foreach (var item in fsInfo.Children)
                {
                    FileScanCopy(item, newPath + "\\" + item.Name);
                }
            }
            return newPath;
        }

        public class FileScanInfo
        {
            public string Name { get; set; }
            public string FullName { get; set; }
            public string Extension { get; set; }
            public DateTime CreateTime { get; set; }
            public DateTime UpdateTime { get; set; }
            public bool IsLeaf { get; set; }
            public List<FileScanInfo> Children { get; set; }
            public long Size { get; set; }
            public int Level { get; set; }//深度
            public FileScanInfo(int dep = 1)
            {
                Children = new List<FileScanInfo>();
            }
            public FileScanInfo(string path, int dep = 1)
                : this(path, dep, "")
            {
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="path"></param>
            /// <param name="dep"></param>
            /// <param name="ignoreFiles">[文件名|深度] 多个字段有逗号隔开</param>
            public FileScanInfo(string path, int dep, string ignoreFiles)
                : this(dep)
            {
                var DRoot = new System.IO.DirectoryInfo(path);

                Name = DRoot.Name;
                FullName = DRoot.FullName;
                CreateTime = DRoot.CreationTime;
                UpdateTime = DRoot.LastWriteTime;


                if (DRoot.Attributes != System.IO.FileAttributes.Directory)
                {
                    IsLeaf = true;
                    Extension = DRoot.Extension;
                    var file = new System.IO.FileInfo(DRoot.FullName);
                    if (file != null && file.Exists)
                        Size = file.Length;
                    return;
                }

                IsLeaf = false;

                if (dep > 0)
                {
                    var chiDir = DRoot.GetFileSystemInfos();
                    foreach (var item in chiDir.OrderBy(t => t.FullName))
                    {
                        var filterName = item.Name + "|" + (Level + 1);
                        if (!string.IsNullOrEmpty(ignoreFiles) && ignoreFiles.Contains(filterName))
                        {
                            continue;
                        }

                        FileScanInfo child = new FileScanInfo(item.FullName, dep - 1, ignoreFiles);
                        child.Level = Level + 1;
                        if (!child.Name.IsNullOrEmpty())
                        {
                            Children.Add(child);
                            Size += child.Size;
                        }
                    }
                }

            }
        }


        /// <summary>
        /// FileCopy
        /// </summary>
        /// <param name="srcFilePath">源路径</param>
        /// <param name="destFilePath">目标路径</param>
        public static void FileCopy(string srcFilePath, string destFilePath)
        {
            File.Copy(srcFilePath, destFilePath);
        }
        /// <summary>
        /// FileMove
        /// </summary>
        /// <param name="srcFilePath">源路径</param>
        /// <param name="destFilePath">目标路径</param>
        public static void FileMove(string srcFilePath, string destFilePath)
        {
            File.Move(srcFilePath, destFilePath);
        }
        /// <summary>
        /// FileDelete
        /// </summary>
        /// <param name="delFilePath"></param>
        public static void FileDelete(string delFilePath)
        {
            File.Delete(delFilePath);
        }
        /// <summary>
        /// 删除文件夹及文件夹中的内容
        /// </summary>
        /// <param name="delFolderPath"></param>
        public static void FolderDelete(string delFolderPath)
        {
            if (delFolderPath[delFolderPath.Length - 1] != Path.DirectorySeparatorChar)
                delFolderPath += Path.DirectorySeparatorChar;
            //string[] fileList = Directory.GetFileSystemEntries(delFolderPath);
            foreach (string item in Directory.GetFileSystemEntries(delFolderPath))
            {
                if (File.Exists(item))
                {
                    FileInfo fi = new FileInfo(item);
                    if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)//改变只读文件属性，否则删不掉
                        fi.Attributes = FileAttributes.Normal;
                    File.Delete(item);
                }//删除其中的文件
                else
                    FolderDelete(item);//递归删除子文件夹
            }
            Directory.Delete(delFolderPath);//删除已空文件夹
        }
        /// <summary>
        /// 文件夹拷贝
        /// </summary>
        /// <param name="fromFolderPath"></param>
        /// <param name="toFolderPath"></param>
        public static void FolderCopy(string fromFolderPath, string toFolderPath)
        {
            //检查目标目录是否以目标分隔符结束，如果不是则添加之
            if (toFolderPath[toFolderPath.Length - 1] != Path.DirectorySeparatorChar)
                toFolderPath += Path.DirectorySeparatorChar;
            //判断目标目录是否存在，如果不在则创建之
            if (!Directory.Exists(toFolderPath))
                Directory.CreateDirectory(toFolderPath);
            string[] fileList = Directory.GetFileSystemEntries(fromFolderPath);
            foreach (string file in fileList)
            {
                if (Directory.Exists(file))
                    FolderCopy(file, toFolderPath + Path.GetFileName(file));
                else
                {
                    FileInfo fi = new FileInfo(file);
                    if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)//改变只读文件属性，否则删不掉
                        fi.Attributes = FileAttributes.Normal;
                    try
                    { File.Copy(file, toFolderPath + Path.GetFileName(file), true); }
                    catch (Exception e)
                    {
                    }
                }

            }
        }


        public static void CreateZipFile(string filesPath, string zipFilePath)
        {

            if (!System.IO.Directory.Exists(filesPath))
            {
                Console.WriteLine("Cannot find directory '{0}'", filesPath);
                return;
            }

            try
            {
                string[] filenames = System.IO.Directory.GetFiles(filesPath);
                using (ICSharpCode.SharpZipLib.Zip.ZipOutputStream s = new ICSharpCode.SharpZipLib.Zip.ZipOutputStream(System.IO.File.Create(zipFilePath)))
                {

                    s.SetLevel(9); // 压缩级别 0-9
                    //s.Password = "123"; //Zip压缩文件密码
                    byte[] buffer = new byte[4096]; //缓冲区大小
                    foreach (string file in filenames)
                    {
                        ICSharpCode.SharpZipLib.Zip.ZipEntry entry = new ICSharpCode.SharpZipLib.Zip.ZipEntry(System.IO.Path.GetFileName(file));
                        entry.DateTime = DateTime.Now;
                        s.PutNextEntry(entry);
                        using (System.IO.FileStream fs = System.IO.File.OpenRead(file))
                        {
                            int sourceBytes;
                            do
                            {
                                sourceBytes = fs.Read(buffer, 0, buffer.Length);
                                s.Write(buffer, 0, sourceBytes);
                            } while (sourceBytes > 0);
                        }
                    }
                    s.Finish();
                    s.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception during processing {0}", ex);
            }
        }

        public static void UnZipFile(string zipFilePath, string SaveFilePath)
        {
            if (!File.Exists(zipFilePath))
            {
                Console.WriteLine("Cannot find file '{0}'", zipFilePath);
                return;
            }


            // create directory
            if (!Directory.Exists(SaveFilePath))
            {
                Directory.CreateDirectory(SaveFilePath);
            }
            using (ZipInputStream s = new ZipInputStream(File.OpenRead(zipFilePath)))
            {

                ZipEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)
                {

                    Console.WriteLine(theEntry.Name);

                    string directoryName = Path.GetDirectoryName(theEntry.Name);
                    string fileName = Path.GetFileName(theEntry.Name);

                    // create directory
                    if (directoryName.Length > 0)
                    {
                        Directory.CreateDirectory(SaveFilePath + "\\" + directoryName);
                    }

                    if (fileName != String.Empty)
                    {
                        using (FileStream streamWriter = File.Create(SaveFilePath + "\\" + theEntry.Name))
                        {

                            int size = 2048;
                            byte[] data = new byte[2048];
                            while (true)
                            {
                                size = s.Read(data, 0, data.Length);
                                if (size > 0)
                                {
                                    streamWriter.Write(data, 0, size);
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

    }
}
