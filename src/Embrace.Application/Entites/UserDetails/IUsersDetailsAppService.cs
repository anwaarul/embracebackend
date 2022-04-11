using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Embrace.Entites.UsersDetails.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embrace.Entites.UsersDetails
{
    public interface IUsersDetailsAppService : IAsyncCrudAppService<UsersDetailsDto, long, PagedResultRequestDto, UsersDetailsDto, UsersDetailsDto>
    {


    }
}
