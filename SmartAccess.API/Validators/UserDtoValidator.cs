using FluentValidation;
using SmartAccess.Application.DTOs;

namespace SmartAccess.API.Validators
{
    public class UserDtoValidator : AbstractValidator<UserDto>
    {
        public UserDtoValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required");
            RuleFor(x => x.RFIDCard).NotEmpty().WithMessage("RFID Card is required");
        }
    }
}