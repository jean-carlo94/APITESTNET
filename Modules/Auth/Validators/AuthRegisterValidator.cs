using APITEST.Common.Utils;
using APITEST.Modules.Auth.DTOs;
using AutoMapper;
using FluentValidation;

namespace APITEST.Modules.Auth.Validators
{
    public class AuthRegisterValidator: AbstractValidator<AuthRegisterDto>
    {
        public AuthRegisterValidator() {
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
