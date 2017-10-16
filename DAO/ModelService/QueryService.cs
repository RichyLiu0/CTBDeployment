using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ucsmy.Framework.DAL;

namespace DeploymentDAO
{
    /// <summary>
    /// 数据库查询对象
    /// </summary>
    public static class QueryService
    {
        
        public static QueryHelper CTBDeployment
        {
            get { return new QueryHelper(DbEntities.CTBDeploymentEntity); }
        }
        
    }
}
