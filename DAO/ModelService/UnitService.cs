using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ucsmy.Framework.DAL;

namespace DeploymentDAO
{
    public class UnitService
    {
        private class UnitOfWorkRepository : UnitOfWorkRepositoryBase
        {
            public UnitOfWorkRepository(string dbKey) : base(dbKey) { }
        }

        public static IUnitOfWorkRepository CTBDeployment
        {
            get { return new UnitOfWorkRepository(DbEntities.CTBDeploymentEntity); }
        }
        
    }
}
