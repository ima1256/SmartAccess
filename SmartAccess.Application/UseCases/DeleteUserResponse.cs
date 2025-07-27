using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartAccess.Application.UseCases
{
    // public class DeleteUserResponse
    // {
    //     public UpdateUserResult Result { get; set; }

    // }

    public enum DeleteUserResult
    {
        Success,
        NotFound,
        InvalidInput
    }

}