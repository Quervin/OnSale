using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnSale.Web.Data;
using OnSale.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnSale.Web.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountriesController : ControllerBase
    {
        private readonly DataContext _context;

        public CountriesController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetCountries()
        {
            return Ok(_context.Countries
                .Include(c => c.Departments)
                .ThenInclude(d => d.Cities));
        }

        [HttpGet]
        [Route("GetListCountries")]
        public async Task<IActionResult> GetListCountries()
        {
            var countries = await _context.Countries
                .Include(c => c.Departments)
                .ThenInclude(d => d.Cities)
                .ToListAsync();

            var countriesList = countries.Select(x => new ContriesResponse()
            {
                Id = x.Id,
                Name = x.Name,
                Departments = x.Departments?.Select(d => new DepartmentResponse()
                {
                    Id = d.Id,
                    Name = d.Name,
                    Cities = d.Cities?.Select(c => new CityResponse()
                    {
                        Id = c.Id,
                        Name = c.Name
                    }).ToList()
                }).ToList()
            }).ToList();

            return Ok(countriesList);
        }
    }

}
