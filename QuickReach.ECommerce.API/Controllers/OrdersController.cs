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
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository repository;
        private readonly ECommerceDbContext context;

        public OrdersController(IOrderRepository repository, ECommerceDbContext context)
        {
            this.repository = repository;
            this.context = context;
        }

        //Retrieve
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var order = this.repository.Retrieve(id);
            return Ok(order);
        }

        //CREATE
        [HttpPost]
        public IActionResult Post([FromBody] Order newOrder)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest();
            }

            this.repository.Create(newOrder);

            return CreatedAtAction(nameof(this.Get), new { id = newOrder }, newOrder);
        }

        //UPDATE
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Order newOrder)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest();
            }

            this.repository.Update(id, newOrder);

            return Ok(repository);
        }

        //DELETE
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            this.repository.Delete(id);
            return Ok();
        }

        //CREATE Order Item
        [HttpPut("{id}/items")]
        public IActionResult AddOrderItem(int id, [FromBody] OrderItem entity)
        {
            var order = repository.Retrieve(id);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (order == null)
            {
                return NotFound();
            }

            if (entity == null)
            {
                return NotFound();
            }


            order.AddItem(entity);

            repository.Update(id, order);
            return Ok(order);

        }

        //Remove Order Item
        [HttpPut("{id}/items/{itemid}")]
        public IActionResult DeleteOrderItem(int id, int itemId)
        {
            var order = repository.Retrieve(id);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (order == null)
            {
                return NotFound();
            }

            order.RemoveItem(itemId);
            this.repository.Update(id, order);

            DeleteItem(itemId);

            return Ok();
        }

        public void DeleteItem(int itemId)
        {
            var connection = ConnectionHelper.GetConnection();

            var queryToDelete = @"DELETE FROM OrderItem WHERE Id = @Id";

            connection.Query<OrderItem>(queryToDelete, new { Id = itemId })
                       .ToList();
        }
    }
}