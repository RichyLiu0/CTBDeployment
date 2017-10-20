using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;


namespace DeploymentUtility
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
                FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(GetUri(serverPath));
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.UseBinary = true;
                request.UsePassive = true;
                request.Credentials = new NetworkCredential(this._cfg.Win_User, this._cfg.Win_Pwd);

                using (var requestStream = request.GetRequestStream())
                {
                    stream.CopyTo(requestStream);
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

                FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(GetUri(serverPath));
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.UseBinary = true;
                request.UsePassive = true;
                request.Credentials = new NetworkCredential(this._cfg.Win_User, this._cfg.Win_Pwd);

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
        private Uri GetUri(string pathName)
        {
            var uriBuilder = new UriBuilder("ftp", _cfg.HostName, _cfg.EndPoint ?? 21, pathName);
            return uriBuilder.Uri;
        }
    }
    public class FtpClientCfg
    {
        public string HostName { get; set; }
        public int? EndPoint { get; set; }
        /// <summary>
        /// window用户
        /// </summary>
        public string Win_User { get; set; }
        /// <summary>
        /// window用户密码
        /// </summary>
        public string Win_Pwd { get; set; }
    }

}
