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
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierRepository repository;
        public SuppliersController(ISupplierRepository repository)
        {
            this.repository = repository;
        }

        //RETRIEVE
        [HttpGet]
        public IActionResult Get(string search = "", int skip = 0, int count = 10)
        {
            var supplier = repository.Retrieve(search, skip, count);
            return Ok(supplier);
        }

        //CREATE
        [HttpPost]
        public IActionResult Post([FromBody] Supplier newSupplier)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest();
            }

            this.repository.Create(newSupplier);

            return CreatedAtAction(nameof(this.Get), new { id = newSupplier }, newSupplier);
        }

        //UPDATE
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Supplier newSupplier)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest();
            }

            this.repository.Update(id, newSupplier);

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