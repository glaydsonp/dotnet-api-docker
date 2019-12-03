using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Data;
using ProductCatalog.Models;

namespace ProductCatalog.Controllers
{
    [Route("v1")]
    public class CategoryController : Controller
    {
        private readonly StoreDataContext _context;

        public CategoryController(StoreDataContext context)
        {
            _context = context;
        }

        [Route("categories")]
        [HttpGet]
        public IEnumerable<Category> Get()
        {
            return _context.Categories.AsNoTracking().ToList();
        }

        [Route("categories/{id}")]
        [HttpGet]
        public Category Get(int id)
        {
            return _context.Categories.AsNoTracking().Where(x => x.Id == id).FirstOrDefault();
        }

        [Route("categories/{id}/products")]
        [HttpGet]
        public IEnumerable<Product> GetProducts(int id)
        {
            return _context.Products.AsNoTracking().Where(x => x.Category.Id == id).ToList();
        }

        [Route("categories")]
        [HttpPost]
        public Category Post([FromBody]Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();

            return category;
        }

        [Route("categories")]
        [HttpPut]
        public Category Put([FromBody]Category category)
        {
            _context.Entry<Category>(category).State = EntityState.Modified;
            _context.SaveChanges();

            return category;
        }

        [Route("categories")]
        [HttpDelete]
        public Category Delete([FromBody]Category category)
        {
            _context.Categories.Remove(category);
            _context.SaveChanges();

            return category;
        }
    }
}