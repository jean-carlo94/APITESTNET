using APITEST.Modules.Users.DTOs;
using FluentValidation;

namespace APITEST.Modules.Users.Validators
{
    public class UserInsertValidator: AbstractValidator<UserInserDto>
    {
        public UserInsertValidator() {
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
