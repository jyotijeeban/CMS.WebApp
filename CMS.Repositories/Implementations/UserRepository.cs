
using Microsoft.EntityFrameworkCore;
using CMS.Repositories.Interfaces;
using CMS.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;
using CMS.DataAccess;

namespace CMS.Repositories.Implementations
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public DatabaseContext context
        {
            get
            {
                return _dbContext as DatabaseContext;
            }
        }
        public UserRepository(DbContext _db) : base(_db)
        {

        }
        public PagingModel<UserModel> GetUsers(int page, int pageSize)
        {
            var data = (from usr in context.Users
                        join cnt in context.Countries
                        on usr.CountryId equals cnt.CountryId
                        select new UserModel
                        {
                            Id = usr.Id,
                            Name = usr.Name,
                            Image = usr.Image,
                            Designation = usr.Designation,
                            Country = cnt.CountryName,
                            Gender = usr.Gender,
                            Email = usr.Email,
                            MobileNo = usr.MobileNo
                        });

            int count = data.Count();
            var dataList = data.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            PagingModel<UserModel> model = new PagingModel<UserModel>();
            if (dataList.Count > 0)
            {
                model.Data = new StaticPagedList<UserModel>(dataList, page, pageSize, count);
                model.Page = page;
                model.PageSize = pageSize;
                model.TotalPages = count;
            }
            return model;
        }
    }
}
