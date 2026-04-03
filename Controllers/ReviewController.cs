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
    public class ReviewController : ControllerBase
    {
        private readonly ReviewService _reviewService;
        public ReviewController(ReviewService reviewService)
        {
            _reviewService=reviewService;
        }

        [HttpGet("GetAllReviews")]
        public async Task<IActionResult> GetAllReviews()
        {
            var reviews = await _reviewService.GetReviewsAsync();

            if(reviews != null) return Ok(reviews);

            return NotFound(new {Message = "No reviews found"});
        }

        [HttpGet("GetReviewByUser/{username}")]
        public async Task<IActionResult> GetReviewByUser(string username)
        {
            var reviews = await _reviewService.GetReviewByUserAsync(username);

            if(reviews != null) return Ok( new { reviews });

            return NotFound(new {Message = "No reviews found"});
        }

        [HttpPost("AddReview")]
        public async Task<IActionResult> AddReview(ReviewModel review)
        {
            if (review == null)
            {
               return BadRequest("Review data is required.");
            }
            var success = await _reviewService.AddReviewAsync(review);

            if(success) return Ok(new {success});

            return BadRequest(new { success });
        }

        [HttpPut("EditReview")]
        public async Task<IActionResult> EditReview(ReviewModel review)
        {
            var success = await _reviewService.EditReviewAsync(review);

            if(success) return Ok(new {success});

            return BadRequest(new { success });
        }

        [HttpDelete("DeleteReview")]
        public async Task<IActionResult> DeleteReview(ReviewModel review)
        {
            var success = await _reviewService.EditReviewAsync(review);

            if(success) return Ok(new {success});

            return BadRequest(new { success });
        }

        [HttpGet("GetReviewByBuissness/{buissness}")]
        public async Task<IActionResult> GetReviewByBuissness(string buissness)
        {
            var reviews= await _reviewService.GetReviewsByBuissnessAsync(buissness);

            if(reviews != null) return Ok( new { reviews });

            return BadRequest(new {Message = "No reviews found"});
        }

        [HttpGet("GetReviewsByScore/{rating}")]
        public async Task<ActionResult<IEnumerable<ReviewModel>>> GetReviewsByScore(int rating)
        {
            var reviews = await _reviewService.GetReviewsByScoreAsync(rating);
            if (reviews == null)
                return NotFound("No reviews found.");
            return Ok(reviews);
        }

        

        
    }
}