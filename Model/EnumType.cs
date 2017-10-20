using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentModel
{
    public class EnumType
    {
        public enum DeploymentType
        {
            文件路径布署 = 10,
            服务部署 = 20,
            Ftp布署 = 30
        }
        public enum DeployTaskStatus
        {
            新建 = 10,
            部署中 = 20,
            部署完成 = 30,
            部署异常 = 40
        }
    }
}
