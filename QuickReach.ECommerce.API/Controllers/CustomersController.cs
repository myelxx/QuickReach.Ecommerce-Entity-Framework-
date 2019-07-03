using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickReach.ECommerce.Domain;
using QuickReach.ECommerce.Domain.Models;
using QuickReach.ECommerce.Infra.Data;

namespace QuickReach.ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepository repository;
        private readonly ECommerceDbContext context;

        public CustomersController(ICustomerRepository repository, ECommerceDbContext context)
        {
            this.repository = repository;
            this.context = context;
        }

        //Retrieve
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var cart = this.repository.Retrieve(id);
            return Ok(cart);
        }

        //CREATE
        [HttpPost]
        public IActionResult Post([FromBody] Customer newCustomer)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest();
            }

            this.repository.Create(newCustomer);

            return CreatedAtAction(nameof(this.Get), new { id = newCustomer }, newCustomer);
        }

        //UPDATE
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Customer newCustomer)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest();
            }

            this.repository.Update(id, newCustomer);

            return Ok(repository);
        }

        //DELETE
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            this.repository.Delete(id);
            return Ok();
        }

    }
}