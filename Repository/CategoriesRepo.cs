using Microsoft.EntityFrameworkCore;
using ProductWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductWebApp.Repository
{
    public class CategoriesRepo : ICategoriesRepo
    {
        private readonly AppDbContext _entities;

        public CategoriesRepo(AppDbContext entities)
        {
            _entities = entities;
        }

        public Category Add(Category model)
        {
            var result = _entities.Categories.Attach(model);
            result.State = Microsoft.EntityFrameworkCore.EntityState.Added;
            _entities.SaveChanges();
            return model;
        }

        public List<Category> All()
        {
            return _entities.Categories.AsNoTracking().ToList();
        }

        public Category Delete(Guid id)
        {
            var obj = _entities.Categories.Find(id);
            if (obj != null)
            {
                _entities.Categories.Remove(obj);
                _entities.SaveChanges();
            }
            return obj;
        }

        public Category Edit(Category model)
        {
            var result = _entities.Categories.Attach(model);
            result.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _entities.SaveChanges();
            return model;
        }

        public Category Get(Guid id)
        {
            return _entities.Categories.Find(id);
        }
    }
}
