using ProductWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductWebApp.Repository
{
    public interface ICategoriesRepo
    {
        List<Category> All();
        Category Get(Guid id);
        Category Add(Category model);
        Category Edit(Category model);
        Category Delete(Guid id);
    }
}
