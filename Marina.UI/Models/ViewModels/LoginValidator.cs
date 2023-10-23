using FluentValidation;

namespace Marina.UI.Models.ViewModels
{
    public class LoginValidator : AbstractValidator<LoginVm>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.");
        }
    }
}
