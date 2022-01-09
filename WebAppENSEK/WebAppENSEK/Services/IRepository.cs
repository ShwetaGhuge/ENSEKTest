using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using WebAppENSEK.Model;

namespace WebAppENSEK.Services
{
    public interface IRepository
    {
        ResultResponse MeterReadingUpload(IFormFile file);
    }

}
