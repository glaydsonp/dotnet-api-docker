using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Data;
using ProductCatalog.Models;
using ProductCatalog.ViewModels.ProductViewModels;

namespace ProductCatalog.Controllers
{
    [Route("v1")]
    public class ProductController : Controller
    {
        private readonly StoreDataContext _context;

        public ProductController(StoreDataContext context)
        {
            _context = context;
        }

        [Route("products")]
        [HttpGet]
        public IEnumerable<ListProductViewModel> Get()
        {
            return _context.Products.Include(x => x.Category).Select(x => new ListProductViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Price = x.Price,
                Category = x.Category.Title,
                CategoryId = x.CategoryId
            }).AsNoTracking().ToList();
        }

        [Route("products/{id}")]
        [HttpGet]
        public Product GetProduct(int id)
        {
            return _context.Products.AsNoTracking().Where(x => x.Id == id).FirstOrDefault();
        }

        [Route("products")]
        [HttpPost]
        public ResultViewModel Post([FromBody]EditProductViewModel model)
        {
            model.Validate();
            if (model.Invalid)
            {
                return new ResultViewModel
                {
                    Success = false,
                    Message = "Não foi possível cadastrar o produto.",
                    Data = model.Notifications
                };
            }

            var product = new Product();

            product.Title = model.Title;
            product.CategoryId = model.CategoryId;
            product.Description = model.Description;
            product.Image = model.Image;
            product.Price = model.Price;
            product.Quantity = model.Quantity;
            product.CreateDate = DateTime.Now;
            product.LastUpdateDate = DateTime.Now;

            _context.Products.Add(product);
            _context.SaveChanges();

            return new ResultViewModel
            {
                Success = true,
                Message = "Produto cadastrado com sucesso!",
                Data = product
            };
        }

        [Route("products")]
        [HttpPut]
        public ResultViewModel Put([FromBody]EditProductViewModel model)
        {
            try
            {
                model.Validate();
                if (model.Invalid)
                {
                    return new ResultViewModel
                    {
                        Success = false,
                        Message = "Não foi possível atualizar o produto.",
                        Data = model.Notifications
                    };
                }

                var product = _context.Products.Find(model.Id);

                product.Title = model.Title;
                product.CategoryId = model.CategoryId;
                product.Description = model.Description;
                product.Image = model.Image;
                product.Price = model.Price;
                product.Quantity = model.Quantity;
                product.LastUpdateDate = DateTime.Now;

                _context.Entry<Product>(product).State = EntityState.Modified;
                _context.SaveChanges();

                return new ResultViewModel
                {
                    Success = true,
                    Message = "Produto atualizado com sucesso!",
                    Data = product
                };
            }
            catch (System.Exception)
            {

                // throw;
                return new ResultViewModel
                {
                    Success = false,
                    Message = "Não foi possível encontrar o produto.",
                    Data = model.Notifications
                };
            }
        }
    }
}