using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WeatherService.Models;

namespace WeatherService.DAL
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly UserSettingsContext _context = new UserSettingsContext();

        public Repository() { }

        public Repository(UserSettingsContext context)
        {
            _context = context;
        }

        public void AddEntity(T t)
        {
            _context.Set<T>().Add(t);
            _context.SaveChanges();
        }

        public IEnumerable<T> GetAllEntities()
        {
            return _context.Set<T>().ToList();
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate).ToList();
        }

        public void UpdatEntity(T t)
        {
            var entry = _context.Entry(t);
            _context.Set<T>().Attach(t);
            entry.State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Dispose()
        {
            if (_context != null)
                _context.Dispose();
        }
    }
}
