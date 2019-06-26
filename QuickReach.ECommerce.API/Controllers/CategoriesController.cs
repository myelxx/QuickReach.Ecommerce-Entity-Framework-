using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickReach.ECommerce.Domain;
using QuickReach.ECommerce.Domain.Models;
using QuickReach.ECommerce.Infra.Data;
using QuickReach.ECommerce.Infra.Data.Repository;

namespace QuickReach.ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository repository;
        public CategoriesController(ICategoryRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public IActionResult Get(string search="", int skip = 0, int count = 10)
        {
            var categories = repository.Retrieve(search, skip, count);

            return Ok(categories);
        }

        //GET api/values/1
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var category = this.repository.Retrieve(id);
            return Ok(category);
        }

        //POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] Category newCategory)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest();
            }

            this.repository.Create(newCategory);

            return CreatedAtAction(nameof(this.Get), new { id = newCategory }, newCategory);
        }

        //PUT api'/values/1
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Category newCategory)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest();
            }

            this.repository.Update(id, newCategory);

            return Ok(repository);
        }

        //Delete
        [HttpDelete("{id}")]
        public IActionResult Delete (int id)
        {
            this.repository.Delete(id);
            return Ok();
        }
    }
}
