using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Data;
using ProductCatalog.Models;
using ProductCatalog.Repositories;
using ProductCatalog.ViewModels.ProductViewModels;

namespace ProductCatalog.Controllers
{
    [Route("v1")]
    public class CategoryController : Controller
    {
        private readonly CategoryRepository _repository;

        public CategoryController(CategoryRepository repository)
        {
            _repository = repository;
        }

        [Route("categories")]
        [HttpGet]
        public IEnumerable<Category> Get()
        {
            return _repository.Get();
        }

        [Route("categories/{id}")]
        [HttpGet]
        public Category Get(int id)
        {
            // return _context.Categories.AsNoTracking().Where(x => x.Id == id).FirstOrDefault();
            return _repository.Get(id);
        }

        [Route("categories/{id}/products")]
        [HttpGet]
        public IEnumerable<Product> GetProducts(int id)
        {
            return _repository.GetProducts(id);
        }

        [Route("categories")]
        [HttpPost]
        public Category Post([FromBody]Category category)
        {
            _repository.Save(category);

            return category;
        }

        [Route("categories")]
        [HttpPut]
        public Category Put([FromBody]Category category)
        {
            // _context.Entry<Category>(category).State = EntityState.Modified;
            // _context.SaveChanges();

            _repository.Update(category);

            return category;
        }

        [Route("categories")]
        [HttpDelete]
        public ResultViewModel Delete([FromBody]Category category)
        {
            _repository.Delete(category);
            return new ResultViewModel
            {
                Success = true,
                Message = "Categoria deletada com sucesso.",
                Data = category
            };
        }
    }
}