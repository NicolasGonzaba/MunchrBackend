using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MunchrBackend.Context;
using MunchrBackend.Models;

namespace MunchrBackend.Services
{
    public class ReviewService
    {
        private readonly DataContext _dataContext;

        public ReviewService(DataContext dataContext)
        {
            _dataContext=dataContext;
        }

        public async Task<List<ReviewModel>> GetReviewsAsync ()=> await _dataContext.Reviews.ToListAsync();

        internal async Task<bool> AddReviewAsync(ReviewModel review)
        {
            await _dataContext.Reviews.AddAsync(review);
            return await _dataContext.SaveChangesAsync()!=0;
        }

        internal async Task<bool> EditReviewAsync(ReviewModel review)
        {
            var reviewToEdit = await GetReviewByIdAsync(review.Id);

            if(reviewToEdit == null) return false;

            
            reviewToEdit.Image = review.Image;
            reviewToEdit.Publisher = review.Publisher;
            reviewToEdit.Content = review.Content;
            reviewToEdit.Date = review.Date;
            reviewToEdit.Rating = review.Rating;
            reviewToEdit.Buissness = review.Buissness;
            reviewToEdit.IsPublished = review.IsPublished;
            reviewToEdit.IsDeleted = review.IsDeleted;

            _dataContext.Reviews.Update(reviewToEdit);
            return await _dataContext.SaveChangesAsync() != 0;
        }

        private async Task<ReviewModel> GetReviewByIdAsync(int id)
        {
            return await _dataContext.Reviews.FindAsync(id);
        }

        internal async Task<List<ReviewModel>> GetReviewByUserAsync(string username) => await _dataContext.Reviews.Where(review => review.Publisher==username).ToListAsync();
        


        internal async Task<List<ReviewModel>> GetReviewsByBuissnessAsync(string buissness)
        {
            return await _dataContext.Reviews
            .Where(review => review.Buissness == buissness)
            .ToListAsync();
        }
    }
}