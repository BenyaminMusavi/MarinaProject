using FluentValidation;
using System.Text.RegularExpressions;

namespace Marina.UI.Models.ViewModels;

public partial class UserRegistrationValidator : AbstractValidator<RegisterVm>
{
    public UserRegistrationValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();

        RuleFor(x => x.Email).NotEmpty().EmailAddress()
             .When(_ => !string.IsNullOrEmpty(_.Email) && EmailRegex().IsMatch(_.Email), ApplyConditionTo.CurrentValidator)
             .WithMessage("Email Is Already Exist");

        RuleFor(x => x.Password).NotEmpty().WithMessage("Your password cannot be empty")
                .MinimumLength(6).WithMessage("Your password length must be at least 6.")
                .MaximumLength(16).WithMessage("Your password length must not exceed 16.")
                .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
                .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
                .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.")
                .Matches(@"[\@\!\?\*\.]+").WithMessage("Your password must contain at least on (@!? *.).)");

        RuleFor(x => x.ConfirmPassword).Equal(_ => _.Password).WithMessage("Confirm password must be equal 'Password'");

    }

    [GeneratedRegex("\\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\\Z", RegexOptions.IgnoreCase, "en-US")]
    private static partial Regex EmailRegex();
}
