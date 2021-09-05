using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WeatherService.DAL
{
    public interface IRepository<T>: IDisposable where T : class
    {
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        IEnumerable<T> GetAllEntities();
        void AddEntity(T t);
        void UpdatEntity(T t);
    }
}
