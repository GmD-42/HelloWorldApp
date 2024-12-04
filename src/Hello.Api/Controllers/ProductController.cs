using Microsoft.AspNetCore.Mvc;
using Hello.Api.Models;
using System.Collections.Generic;

namespace Hello.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly List<Product> _products;

        // Inject the shared List<Product> through the constructor
        public ProductController(List<Product> products)
        {
            _products = products;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetAll() => Ok(_products);

        [HttpGet("{id}")]
        public ActionResult<Product> Get(int id)
        {
            var product = _products.Find(p => p.Id == id);
            return product is null ? NotFound() : Ok(product);
        }

        [HttpPost]
        public ActionResult Create(Product product)
        {
            product.Id = _products.Count + 1;
            _products.Add(product);
            return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, Product updatedProduct)
        {
            var product = _products.Find(p => p.Id == id);
            if (product is null) return NotFound();
            
            product.Name = updatedProduct.Name;
            product.Price = updatedProduct.Price;
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var product = _products.Find(p => p.Id == id);
            if (product is null) return NotFound();

            _products.Remove(product);
            return NoContent();
        }
    }
}
