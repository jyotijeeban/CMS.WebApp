using CMS.DataAccess;
using CMS.Repositories.Interfaces;

namespace CMS.Repositories
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepo { get; }
        IRepository<User> UserGenericRepo { get; }

        IRepository<Country> CountryRepo { get; }
        int SaveChanges();
    }
}
