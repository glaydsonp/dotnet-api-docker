using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Models;
using ProductCatalog.Repositories;
using ProductCatalog.ViewModels.ProductViewModels;

namespace ProductCatalog.Controllers
{
    [Route("v1")]
    public class ProductController : Controller
    {
        private readonly ProductRepository _repository;

        public ProductController(ProductRepository repository)
        {
            _repository = repository;
        }

        [Route("products")]
        [HttpGet]
        // Cache Handling is in Minutes
        [ResponseCache(Duration = 60)]
        public IEnumerable<ListProductViewModel> Get()
        {
            return _repository.Get();
        }

        [Route("products/{id}")]
        [HttpGet]
        public Product GetProduct(int id)
        {
            return _repository.Get(id);
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

            _repository.Save(product);

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

                var product = _repository.Find(model.Id);

                product.Title = model.Title;
                product.CategoryId = model.CategoryId;
                product.Description = model.Description;
                product.Image = model.Image;
                product.Price = model.Price;
                product.Quantity = model.Quantity;
                product.LastUpdateDate = DateTime.Now;

                _repository.Update(product);

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