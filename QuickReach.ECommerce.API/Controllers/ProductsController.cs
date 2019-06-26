using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickReach.ECommerce.Domain;
using QuickReach.ECommerce.Domain.Models;

namespace QuickReach.ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository repository;
        public ProductsController(IProductRepository repository)
        {
            this.repository = repository;
        }

        //RETRIEVE
        [HttpGet]
        public IActionResult Get(string search="", int skip=0, int count = 10)
        {
            var products = repository.Retrieve(search, skip, count);
            return Ok(products);
        }

        //CREATE
        [HttpPost]
        public IActionResult Post([FromBody] Product newProduct)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest();
            }

            this.repository.Create(newProduct);

            return CreatedAtAction(nameof(this.Get), new { id = newProduct }, newProduct);
        }

        //UPDATE
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Product newProduct)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest();
            }

            this.repository.Update(id, newProduct);

            return Ok(repository);
        }

        //DELETE
        [HttpDelete("{id}")]
        public IActionResult Delete (int id)
        {
            this.repository.Delete(id);
            return Ok();
        }
    }
}