using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Retail.DataAccess.Repository.IRepository;
using RetailWeb.DataAccess.Data;

namespace Retail.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        // Dependency Injuction

        private readonly ApplicationDbContext _db;

        internal DbSet<T> dbSet;
        public Repository(ApplicationDbContext db)
        {
            _db = db;

            this.dbSet = _db.Set<T>(); // This is equvalent to _db.Categories == dbSet

        }
        public void Add(T entity)
        {

            dbSet.Add(entity);//_db.Categories.Add(some entity);
        }

        public T Get(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = dbSet;

            query = query.Where(filter);

            return query.FirstOrDefault(); // This is equvalent to //Category? categoryFromDb2 = _db.Categories.Where( u => u.Id == id).FirstOrDefault();

        }

        public IEnumerable<T> GetAll()
        {
            IQueryable<T> query = dbSet;

            return query.ToList();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);

        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
        }
    }
}
