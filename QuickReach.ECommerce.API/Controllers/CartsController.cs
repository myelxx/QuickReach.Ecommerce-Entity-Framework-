using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickReach.ECommerce.API.Controllers.Utilities;
using QuickReach.ECommerce.Domain;
using QuickReach.ECommerce.Domain.Models;
using QuickReach.ECommerce.Infra.Data;

namespace QuickReach.ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ICartRepository repository;
        private readonly ICustomerRepository customerRepo;
        private readonly ECommerceDbContext context;

        public CartsController(ICartRepository repository, ICustomerRepository customerRepo, ECommerceDbContext context)
        {
            this.repository = repository;
            this.customerRepo = customerRepo;
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
        public IActionResult Post([FromBody] Cart newCart)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest();
            }

            var customer = customerRepo.Retrieve(newCart.CustomerId);
            if (customer == null)
            {
                return NotFound();
            }

            this.repository.Create(newCart);

            return CreatedAtAction(nameof(this.Get), new { id = newCart }, newCart);
        }

        //UPDATE
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Cart newCart)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest();
            }

            var customer = customerRepo.Retrieve(newCart.CustomerId);
            if (customer == null)
            {
                return NotFound();
            }

            this.repository.Update(id, newCart);

            return Ok(repository);
        }

        //DELETE
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            this.repository.Delete(id);
            return Ok();
        }

        //CREATE Cart Item
        [HttpPut("{id}/items")]
        public IActionResult AddCartItem(int id, [FromBody] CartItem entity)
        {
            var cart = repository.Retrieve(id);
            
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (cart == null)
            {
                return NotFound();
            }

            var customer = customerRepo.Retrieve(cart.CustomerId);
            if (customer == null)
            {
                return NotFound();
            }

            //if (entity == null)
            //{
            //    return NotFound();
            //}

            cart.AddItem(entity);

            repository.Update(id, cart);
            return Ok(cart);

        }

        //Remove Cart Item
        [HttpPut("{id}/items/{itemid}")]
        public IActionResult DeleteCartItem(int id, int itemId)
        {
            var cart = repository.Retrieve(id);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (cart == null)
            {
                return NotFound();
            }

            var customer = customerRepo.Retrieve(cart.CustomerId);
            if (customer == null)
            {
                return NotFound();
            }

            cart.RemoveItem(itemId);
            this.repository.Update(id, cart);

            DeleteItem(itemId);

            return Ok();
        }

        public void DeleteItem(int itemId)
        {
            var connection = ConnectionHelper.GetConnection();
            var queryToDelete = @"DELETE FROM CartItem WHERE Id = @Id";

            connection.Query<CartItem>(queryToDelete, new { Id = itemId })
                       .ToList();
        }

    }
}