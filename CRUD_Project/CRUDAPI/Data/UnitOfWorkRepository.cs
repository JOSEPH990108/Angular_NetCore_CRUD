using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CRUDAPI.Interfaces;

namespace CRUDAPI.Data
{
    public class UnitOfWorkRepository : IUnitOfWorkRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public UnitOfWorkRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public IUserRepository UserRepository => new UserRepository(_db, _mapper);

        public async Task<bool> Complete()
        {
            return await _db.SaveChangesAsync() > 0;
        }

        public bool HasChanges()
        {
            _db.ChangeTracker.DetectChanges();
            var changes = _db.ChangeTracker.HasChanges();

            return changes;
        }
    }
}