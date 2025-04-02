using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Retail.DataAccess.Data;
using Retail.DataAccess.Repository.IRepository;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

            // To Display DB listed entities here use EF Include Method as such
            _db.Products.Include(entity => entity.Category).Include(entityId => entityId.CategoryId);

        }
        public void Add(T entity)
        {

            //_db.Categories.Add(some entity);
            dbSet.Add(entity); 
        }
        //Set tracked property to false to stop EF Core auto tracking
        public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false)
        {

            IQueryable<T> query;
            if (tracked)
            {
                query= dbSet;
            }
            else
            {
                query = dbSet.AsNoTracking();
            }

            query = query.Where(filter);

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            return query.FirstOrDefault(); // This is equvalent to //Category? categoryFromDb2 = _db.Categories.Where( u => u.Id == id).FirstOrDefault();

        }

        //Including Category, CoverType and passing it as an IncludedProperties to populate it
        public IEnumerable<T> GetAll(string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;

            if(!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties
                    .Split(new char[] {','},StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

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
