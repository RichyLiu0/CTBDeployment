using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeploymentUtility
{
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
        /// <summary>
        /// 浏览文件地址下的文件
        /// </summary>
        /// <param name="path">文件地址</param>
        /// <param name="dep">层（默认为1）</param>
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
                    if (!string.IsNullOrWhiteSpace(child.Name))
                    {
                        Children.Add(child);
                        Size += child.Size;
                    }
                }
            }

        }
    }
}
