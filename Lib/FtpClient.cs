using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;


namespace MyLibs.Utility.IO
{
    public class FtpClient
    {
        private FtpClientCfg _cfg { get; set; }
        public FtpClient(FtpClientCfg cfg)
        {
            this._cfg = cfg;
        }
        public void UpLoad(string serverPath, Stream stream)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(new UriBuilder("ftp", _cfg.HostName, _cfg.EndPoint ?? 21, serverPath).Uri);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.UseBinary = true;
                request.UsePassive = true;
                request.Credentials = new NetworkCredential(this._cfg.UserName, this._cfg.UserPwd);

                using (var requestStream = request.GetRequestStream())
                {
                    //using (stream)
                    //{
                    //Byte[] buffer = new Byte[4096];
                    //var offset = 0;
                    //offset = stream.Read(buffer, 0, buffer.Length);
                    //while (offset > 0)
                    //{
                    //    requestStream.Write(buffer, 0, offset);
                    //    offset = stream.Read(buffer, 0, buffer.Length);
                    //}
                    stream.CopyTo(requestStream);
                    //}
                }
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            }
            catch (Exception ex)
            {
                throw new Exception("ftp文件上传失败：" + ex.Message);
            }
        }
        public Stream Open(string serverPath)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(new UriBuilder("ftp", _cfg.HostName, _cfg.EndPoint ?? 21, serverPath).Uri);
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.UseBinary = true;
                request.UsePassive = true;
                request.Credentials = new NetworkCredential(this._cfg.UserName, this._cfg.UserPwd);

                MemoryStream stream = new MemoryStream();
                using (var responseStream = request.GetResponse().GetResponseStream())
                {
                    responseStream.CopyTo(stream);
                }
                stream.Position = 0;
                return stream;
            }
            catch (Exception ex)
            {
                throw new Exception("ftp文件获取失败：" + ex.Message);
            }
        }
        public void DownLoad(string serverPath, string localPath)
        {
            using (var stream = Open(serverPath))
            {
                using (var fileStream = File.Create(localPath))
                {
                    stream.CopyTo(fileStream);
                }
            }
        }
    }
    public class FtpClientCfg
    {
        public string HostName { get; set; }
        public int? EndPoint { get; set; }
        public string UserName { get; set; }
        public string UserPwd { get; set; }
    }

}
