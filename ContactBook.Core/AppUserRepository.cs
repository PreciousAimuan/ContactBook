using ContactBook.Core.DTOs;
using ContactBook.Data;
using ContactBook.Infrastructure;
using ContactBook.Infrastructure.Helper;
using ContactBook.Infrastructure.Interfaces;
using ContactBook.Infrastructure.Services;
using ContactBook.Model.Entities;
//using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBook.Core
{
    public class AppUserRepository : IAppUserRepository
    {
        private readonly ContactBookContext _context;
        private readonly IPhotoService _photoServices;
        private readonly UserManager<AppUser> _userManger;

        public AppUserRepository(ContactBookContext context, IPhotoService photoServices, UserManager<AppUser> userManager)
        {
            _context = context;
            _photoServices = photoServices;
            _userManger = userManager;
        }
        public async Task<string> AddAppUser(AppUserDTO appUserDto)
        {
            var existingUser = await _context.AppUsers.FirstOrDefaultAsync(e => e.Email == appUserDto.Email);
            if (existingUser != null)
            {
                return "User already exist";
            }

            var newAppUser = new AppUser()
            {
                FirstName = appUserDto.FirstName,
                LastName = appUserDto.LastName,
                Email = appUserDto.Email,
                PhoneNumber = appUserDto.PhoneNumber,
            };
            _context.AppUsers.Add(newAppUser);
            var saveChanges = await _context.SaveChangesAsync();
            if (saveChanges > 0)
            {
                return "User added Successfully";
            }

            return "User could not be added";
        }



        /*public async Task<string> DeleteAppUserById(string id)
        {
            var appUser = await GetAppUserById(id);
            if (appUser != null)
            {
                _context.AppUsers.Remove(appUser);
                await _context.SaveChangesAsync();
                return "User Deleted Successfully";
            }

            return "User not found";
        }*/

        public async Task<List<AppUserDTO>> GetAllUser(PaginParameter userParameter)
        {
            var contacts = _context.AppUsers
                .OrderBy(p => p.FirstName)
                /*.Skip((userParameter.PageNumber - 1) * userParameter.PageSize)
                .Take(userParameter.PageSize)*/
                .ToList();


            var data = new List<AppUserDTO>();
            foreach(var userData in contacts)
            {
                data.Add(new AppUserDTO
                {
                    FirstName = userData.FirstName,
                    LastName = userData.LastName,
                    Email = userData.Email,
                    PhoneNumber = userData.PhoneNumber,
                    City = userData.City,
                    Country = userData.Country,
                    ImageUrl = userData.ImageUrl,
                    State = userData.State,
                    FacebookUrl = userData.FacebookUrl,
                    TwitterUrl = userData.TwitterUrl
                });
            }
            return data.Skip((userParameter.PageNumber - 1) * userParameter.PageSize)
                .Take(userParameter.PageSize).ToList();
        }

        public async Task<AppUserDTO> GetAppUserById(string id)
        {
            var userData = await _context.AppUsers.FirstOrDefaultAsync(p => p.Id == id);
            var data = new AppUserDTO
            {  
                    FirstName = userData.FirstName,
                    LastName = userData.LastName,
                    Email = userData.Email,
                    PhoneNumber = userData.PhoneNumber,
                    City = userData.City,
                    Country = userData.Country,
                    ImageUrl = userData.ImageUrl,
                    State = userData.State,
                    FacebookUrl = userData.FacebookUrl,
                    TwitterUrl = userData.TwitterUrl
            };
            return data;
        }

        public async Task<string> UpdateAppUser(AppUser appUser)
        {
            var existingUser = await _context.AppUsers.FirstOrDefaultAsync(e => e.Id == appUser.Id);
            if (existingUser != null)
            {
                _context.AppUsers.Update(existingUser);
                // new addition
                await _context.SaveChangesAsync();
                return "User updated successfully";
            }

            return "No User found";
        }

        public async Task<bool> AddPhoto(PhotoDTO photoDTO, string id)
        {
            var result = await _photoServices.AddPhotoAsync(photoDTO.ImageUrl);
            var existingUser = await _context.AppUsers.FirstOrDefaultAsync(p => p.Id == id);
            existingUser.ImageUrl = result.Url.AbsolutePath;
            await _userManger.UpdateAsync(existingUser);
            return true;
        }

        public async Task<List<AppUserDTO>> GetAllAsync(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return new List<AppUserDTO>();
            }
            var users = await _userManger.Users
                .Where(p => p.Email.Contains(term)
                || p.FirstName.Contains(term)
                || p.LastName.Contains(term)
                || p.City.Contains(term)
                || p.State.Contains(term)
                || p.Country.Contains(term)
                ).ToListAsync();
            var AppUserDTO = users.Select(item => new AppUserDTO
            {
                FirstName = item.FirstName,
                LastName = item.LastName,
                Email = item.Email,
                ImageUrl = item.ImageUrl,
                City = item.City,
                State = item.State,
                Country = item.Country,
                FacebookUrl = item.FacebookUrl,
                TwitterUrl = item.TwitterUrl
            }).ToList();
            return AppUserDTO;
        }
    }
}
