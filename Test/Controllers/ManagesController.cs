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
using Microsoft.OpenApi.Extensions;

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

        // Get all the information of people with pagination
        [HttpGet]
        public IActionResult List(int pageNumber = 1, int pageSize = 10)
        {
            var totalItems = _context.Information.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var people = _context.Information
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
                
            var response = new
            {
                TotalItems = totalItems,
                TotalPages = totalPages,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                People = people
            };

            return Ok(response);
        }


        // Search by name or dob
        [HttpGet("Search")]
        public IActionResult Index(string? name, DateTime? dob)
        {
            var people = from p in _context.Information
                         select p;

            if (!String.IsNullOrEmpty(name))
            {
                people = people.Where(s => s.Name.Contains(name));
            }

            if (dob != null)
            {
                people = people.Where(s => s.DOB == dob);   
            }
             
            return Ok(people.ToList());
        }
        

        // Get by Id
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Manage>> GetPeople(int id)
        {
            try
            {
                var result = await _context.Information.FindAsync(id);

                if (result == null) return NotFound();

                return result;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }


        // Edit information of people
        [HttpPut("{id}")]
        public IActionResult Update(int id, Manage updatedInformation)
        {
            var person = _context.Information.Find(id);
            if (person == null)
                return NotFound();
            
            person.Name = updatedInformation.Name;
            person.DOB =updatedInformation.DOB;
            
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
            var people = _context.Information.Where(async => ids.Contains(async.Id)).ToList();
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
