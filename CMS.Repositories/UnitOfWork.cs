using CMS.DataAccess;
using CMS.Repositories.Implementations;
using CMS.Repositories.Interfaces;


namespace CMS.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        DatabaseContext _db;
        public UnitOfWork(DatabaseContext db)
        {
            _db = db;
        }

        private IUserRepository _UserRepo;
        public IUserRepository UserRepo
        {
            get
            {
                if (_UserRepo == null)
                    _UserRepo = new UserRepository(_db);
                return _UserRepo;
            }
        }

        private IRepository<User> _UserGenericRepo;
        public IRepository<User> UserGenericRepo
        {
            get
            {
                if (_UserGenericRepo == null)
                    _UserGenericRepo = new Repository<User>(_db);
                return _UserRepo;
            }
        }


        private IRepository<Country> _CountryRepo;
        public IRepository<Country> CountryRepo
        {
            get
            {
                if (_CountryRepo == null)
                    _CountryRepo = new Repository<Country>(_db);
                return _CountryRepo;
            }
        }

        public int SaveChanges()
        {
            return _db.SaveChanges();
        }
    }
}
