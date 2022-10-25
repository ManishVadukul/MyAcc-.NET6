using Microsoft.EntityFrameworkCore;
using MyAcc.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MyAcc.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {

        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;


        public Repository(ApplicationDbContext db)
        {
            _db = db;
            dbSet = _db.Set<T>();
        }

        public IEnumerable<T> GetTable()
        {
            return dbSet.AsEnumerable();
        }

        public T Get(int id)
        {
            return dbSet.Find(id);
        }




        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null)
        {
             IQueryable<T> query =  dbSet;

            if (filter != null)
            {
               query =  query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            if (orderBy != null)
            {
                return  orderBy(query).ToList();
            }
            return  query.ToList();

        }

        public  T GetFirstOrDefault(System.Linq.Expressions.Expression<Func<T, bool>> filter = null, string includeProperties = null)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return  query.FirstOrDefault();
        }

        public void Add(T entity)
        {
            dbSet.Add(entity);
            _db.SaveChanges();
        }
        public void Update(T entity)
        {           
            _db.SaveChanges();
        }
        public void Remove(int id)
        {
            T entity = dbSet.Find(id);
            Remove(entity);
            _db.SaveChanges();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
            _db.SaveChanges();
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
            _db.SaveChanges();
        }
    }
}
