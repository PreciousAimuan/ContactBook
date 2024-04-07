using ContactBook.Core.DTOs;
using ContactBook.Infrastructure;
using ContactBook.Infrastructure.Helper;
using ContactBook.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBook.Core
{
    public interface IAppUserRepository
    {
        Task<string> AddAppUser(AppUserDTO appUser);
        /*Task<string> DeleteAppUserById(string id);*/
        Task<List<AppUserDTO>> GetAllUser(PaginParameter userParameter);
        Task<AppUserDTO> GetAppUserById(string id);
        Task<string> UpdateAppUser(AppUser appUser);
        Task<bool> AddPhoto(PhotoDTO photo, string id);
        Task<List<AppUserDTO>> GetAllAsync(string term);
    }
}
