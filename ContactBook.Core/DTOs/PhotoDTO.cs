using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBook.Core.DTOs
{
    public class PhotoDTO
    {
        public IFormFile ImageUrl { get; set; }
    }
}
