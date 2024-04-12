using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ManageInformation.Data;
using ManageInformation.Model;
using Microsoft.VisualBasic;
using Newtonsoft.Json;

namespace ManageInformation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ManageController : ControllerBase
    {
        private readonly ManageDbContext _context;

        public ManageController(ManageDbContext context)
        {
            _context = context;
        }

        //
        [HttpPost]
        public IActionResult Create(Manage manage)
        {
            _context.Information.Add(manage);
            _context.SaveChanges();
            return Ok(manage);
        }

        // Get all the information of people
        [HttpGet]
        public IActionResult List()
        {
            var people = _context.Information.ToList();
            return Ok(people);
        }

        // Filter by Name or Dob 
        [HttpGet("search")]
        public IActionResult SearchByNameOrDOB(string? name, DateTime? dob)
        {
            var people = _context.Information.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                people = people.Where(p => p.Name == name);
            }

            if (dob != null)
            {
                people = people.Where(p => p.DOB == dob);
            }

            var result = people.ToList();

            if (result.Count == 0)
            {
                return NotFound();
            }

            return Ok(result);
        }
        

        // Edit information of people
        [HttpPut("{id}")]
        public IActionResult Update(int id, Manage updatedInformation)
        {
            var person = _context.Information.Find(id);
            if (person == null)
                return NotFound();

            person.Name = updatedInformation.Name;
            person.DOB = updatedInformation.DOB;

            _context.SaveChanges();
            return Ok(person);
        }

        // Delete by id
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var person = _context.Information.Find(id);
            if (person == null)
                return NotFound();

            _context.Information.Remove(person);
            _context.SaveChanges();
            return NoContent();
        }

        // Delete mulitiple apis
        [HttpDelete("multiple")]
        public IActionResult DeleteMultiple([FromBody] int[] ids)
        {
            var people = _context.Information.Where(p => ids.Contains(p.Id)).ToList();
            _context.Information.RemoveRange(people);
            _context.SaveChanges();
            return NoContent();
        }

        // Delete all the api 
        [HttpDelete("all")]
        public IActionResult DeleteAll()
        {
            var allPeople = _context.Information.ToList();
            _context.Information.RemoveRange(allPeople);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
