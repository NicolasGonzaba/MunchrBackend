using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MunchrBackend.Models;
using MunchrBackend.Services;

namespace MunchrBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BusinessController : ControllerBase
    {
        private readonly BusinessService _businessServices;

        public BusinessController(BusinessService businessServices)
        {
            _businessServices = businessServices;
        }

        [HttpPost("CreateBusiness")]
        public async Task<ActionResult<bool>> CreateBusiness([FromBody] BusinessModel newBusiness)
        {
            var result = await _businessServices.CreateBusiness(newBusiness);
            if (!result)
                return BadRequest("Failed to create.");

            return Ok(result);
        }

        [HttpPut("EditBusiness")]
        public async Task<ActionResult<bool>> EditBusiness([FromBody] BusinessModel business)
        {
            var result = await _businessServices.EditBusinessAsync(business);
            if (!result)
                return NotFound("Failed to update.");

            return Ok(result);
        }

        [HttpGet("GetBusinessByName/{businessName}")]
        public async Task<ActionResult<BusinessModel>> GetBusinessByName(string businessName)
        {
            var business = await _businessServices.GetBusinessInfoByBusinessNameAsync(businessName);
            if (business == null)
                return NotFound("Business not found.");

            return Ok(business);
        }

        [HttpGet("GetBusinessInfoByName/{businessName}")]
        public async Task<ActionResult<BusinessModel>> GetBusinessInfoByName(string businessName)
        {
            var business = await _businessServices.GetBusinessByBusinessName(businessName);
            if (business == null)
                return NotFound("Business not found.");

            return Ok(business);
        }

        [HttpGet("GetBusinessById/{id}")]
        public async Task<ActionResult<BusinessModel>> GetBusinessById(int id)
        {
            var business = await _businessServices.GetBusinessByIdAsync(id);
            if (business == null) return NotFound();

            return Ok(business);
        }

        [HttpGet("GetAllBusinesses")]
        public async Task<ActionResult<IEnumerable<BusinessModel>>> GetAllBusinesses()
        {
            var businesses = await _businessServices.GetAllBusinesses();
            if (businesses == null) return NotFound("Businesses not found.");

            return Ok(businesses);
        }

        [HttpGet("GetBusinessByState/{stateName}")]
        public async Task<ActionResult<IEnumerable<BusinessModel>>> GetBusinessByState(string stateName)
        {
            var businesses = await _businessServices.GetBusinessByState(stateName);
            if (businesses == null || businesses.Count == 0)
                return NotFound("No businesses found in this state.");

            return Ok(businesses);
        }

        [HttpGet("GetBusinessByPostalCode/{postalCode}")]
        public async Task<ActionResult<IEnumerable<BusinessModel>>> GetBusinessByPostalCode(int postalCode)
        {
            var businesses = await _businessServices.GetBusinessByPostalCode(postalCode);
            if (businesses == null || businesses.Count == 0)
                return NotFound("No businesses found with this postal code.");

            return Ok(businesses);
        }

        [HttpGet("GetBusinessByCity/{cityName}")]
        public async Task<ActionResult<IEnumerable<BusinessModel>>> GetBusinessByCity(string cityName)
        {
            var businesses = await _businessServices.GetBusinessByCity(cityName);
            if (businesses == null || businesses.Count == 0)
                return NotFound("No businesses found in this city.");

            return Ok(businesses);
        }

        [HttpGet("GetBusinessByCategory/{foodCategory}")]
        public async Task<ActionResult<IEnumerable<BusinessModel>>> GetBusinessByCategory(string foodCategory)
        {
            var businesses = await _businessServices.GetBusinessByCategory(foodCategory);
            if (businesses == null || businesses.Count == 0)
                return NotFound("No businesses found in this category.");

            return Ok(businesses);
        }

        [HttpPost("CreateBusinessWithImage")]
        public async Task<ActionResult<bool>> CreateBusinessWithImage(
    [FromForm] IFormFile businesfile,
    [FromForm] IFormFile menufile,
    [FromForm] string businessName,
    [FromForm] string businessHours,
    [FromForm] string businessPhoneNumber,
    [FromForm] string businessDescription,
    [FromForm] string category,
    [FromForm] string streetName,
    [FromForm] string city,
    [FromForm] string state,
    [FromForm] int zipCode
)
        {
            string businessUrl = "";
            if (businesfile != null)
            {
                using var stream = businesfile.OpenReadStream();
                businessUrl = await _businessServices.UploadFileAsync(stream, businesfile.FileName);
            }
            
            string menuUrl = "";
            if (menufile != null)
            {
                using var stream = menufile.OpenReadStream();
                menuUrl = await _businessServices.UploadFileAsync(stream, menufile.FileName);
            }

            BusinessModel newBusiness = new()
            {
                BusinessName = businessName,
                BusinessHours = businessHours,
                BusinessPhoneNumber = businessPhoneNumber,
                BusinessDescription = businessDescription,
                Category = category,
                StreetName = streetName,
                City = city,
                State = state,
                ZipCode = zipCode,
                BusinessImage = businessUrl,
                MenuImage = menuUrl
            };

            var result = await _businessServices.CreateBusiness(newBusiness);
            if (!result)
                return BadRequest("Failed to create.");

            return Ok(result);
        }
    }

}