using APITEST.Modules.Auth.DTOs;
using FluentValidation;

namespace APITEST.Modules.Auth.Validators
{
    public class AuthLoginValidator: AbstractValidator<AuthLoginDto>
    {
        public AuthLoginValidator() {
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
