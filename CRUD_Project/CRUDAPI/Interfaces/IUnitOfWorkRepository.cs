using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUDAPI.Interfaces
{
    public interface IUnitOfWorkRepository
    {
        IUserRepository UserRepository { get; }
        Task<bool> Complete();
        bool HasChanges();
    }
}