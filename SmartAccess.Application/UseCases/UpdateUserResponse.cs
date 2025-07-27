using SmartAccess.Application.DTOs;

namespace SmartAccess.Application.UseCases
{

    public class UpdateUserResponse
    {
        public UpdateUserResult Result { get; set; }
        public UserDto? UpdatedUser { get; set; }
    }

    public enum UpdateUserResult
    {
        Success,
        NotFound,
        InvalidInput
    }
}