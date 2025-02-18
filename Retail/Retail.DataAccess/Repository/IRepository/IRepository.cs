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
        IEnumerable<T> GetAll(string? includeProperties = null);
        T Get(Expression<Func<T, bool>> filter, string? includeProperties = null);//Linq Operator and filtering
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
    }
    
}
