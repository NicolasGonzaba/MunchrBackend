using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MunchrBackend.Context;
using MunchrBackend.Models;

namespace MunchrBackend.Services;

    public class BusinessService
    {
        private readonly DataContext _dataContext;
        public BusinessService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<bool> CreateBusiness(BusinessModel newBusiness)
        {
            if(await DoesBusinessExist(newBusiness.BusinessName)) return false;

            await _dataContext.Business.AddAsync(newBusiness);
            return await _dataContext.SaveChangesAsync() != 0;
        }

        public async Task<bool> EditBusinessAsync (BusinessModel business)
        {
            var businessToEdit = await GetBusinessByIdAsync(business.BusinessId);

            if(businessToEdit == null) return false;

            businessToEdit.BusinessDescription = business.BusinessDescription;
            businessToEdit.BusinessHours = business.BusinessHours;
            businessToEdit.BusinessName = business.BusinessName;
            businessToEdit.BusinessPhoneNumber = business.BusinessPhoneNumber;
            businessToEdit.Category = business.Category;
            businessToEdit.City = business.City;
            businessToEdit.State = business.State;
            businessToEdit.ZipCode = business.ZipCode;
            businessToEdit.StreetName = business.StreetName;

            _dataContext.Business.Update(businessToEdit);
            return await _dataContext.SaveChangesAsync() != 0;
        }

        public async Task<BusinessModel> GetBusinessByIdAsync(int id)
        {
            return await _dataContext.Business.FindAsync(id);
        }

        public async Task<List<BusinessModel>> GetAllBusinesses()
        {
            return await _dataContext.Business.ToListAsync();
        }

        public async Task<BusinessModel> GetBusinessInfoByBusinessNameAsync(string businessName) => await _dataContext.Business.SingleOrDefaultAsync(business => business.BusinessName == businessName);

        public async Task<BusinessModel> GetBusinessByBusinessName(string businessName)
        {
            var currentBusiness = await _dataContext.Business.SingleOrDefaultAsync(business => business.BusinessName == businessName);

            BusinessModel business = new();
            business.BusinessId = currentBusiness.BusinessId;
            business.BusinessName = currentBusiness.BusinessName;
            return business;
        }

        public async Task<List<BusinessModel>> GetBusinessByState(string stateName)
        {
            return await _dataContext.Business.Where(business => business.State == stateName).ToListAsync();
        }

        public async Task<List<BusinessModel>> GetBusinessByPostalCode(int postalCode)
        {
            return await _dataContext.Business.Where(business => business.ZipCode == postalCode).ToListAsync();
        }

        public async Task<List<BusinessModel>> GetBusinessByCity(string cityName)
        {
            return await _dataContext.Business.Where(business => business.City == cityName).ToListAsync();
        }

        public async Task<List<BusinessModel>> GetBusinessByCategory(string foodCategory)
        {
            return await _dataContext.Business.Where(business => business.Category == foodCategory).ToListAsync();
        }

        private async Task<bool> DoesBusinessExist(string businessName)
        {
            return await _dataContext.Business.SingleOrDefaultAsync(business => business.BusinessName == businessName) != null;
        }
    }
