using MultiRoleAuthentication.Areas.Identity.Data;
using MultiRoleAuthentication.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiRoleAuthentication.DAL
{
    public class UserRepository : IUserRepository, IDisposable
    {
        private readonly DBContext _dbContext;

        public UserRepository(DBContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
