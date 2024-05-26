using FluentValidation;
using APITEST.Modules.Users.DTOs;

namespace APITEST.Modules.Users.Validators
{
    public class UserUpdateValidator: AbstractValidator<UserUpdateDto>
    {
        public UserUpdateValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.Name)
                .NotEmpty()
                .Length(1, 50);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8)
                .Matches(@"(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[_\W]).{6,}")
                .WithMessage("Su contraseña debe tener mayúsculas, minúsculas, caracteres especiales y números");
        }
    }
}
