using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiRoleAuthentication.DAL
{
    public interface IUserRepository : IDisposable
    {
        void Save();
    }
}
