using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Retail.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        //T - Category - Also getting Properties with includeProperties param
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
        T Get(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false);//Linq Operator and filtering 
        // added bool tracked = false to stop EFC auto-tracking feature that will update in DB automatically
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
    }
    
}
