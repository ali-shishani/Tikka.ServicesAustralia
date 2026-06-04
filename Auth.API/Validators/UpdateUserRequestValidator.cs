namespace Horus.API.Validators;

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEqual(Guid.Empty)
            .WithMessage("Invalid user ID.");

        RuleFor(x => x.Username)
            .Username()
            .WithMessage("Username must be 3-20 characters long and contain only letters, numbers, and underscores.");

        RuleFor(x => x.DateOfBirth)
            .NotEmpty()
            .Must(BeAtLeast13YearsOld)
            .WithMessage("You must be at least 13 years old to register.");

        RuleFor(x => x.Gender)
            .IsInEnum()
            .WithMessage("Please select a valid gender. Valid values: 0 (Male), 1 (Female).");
    }

    private static bool BeAtLeast13YearsOld(DateTime dateOfBirth)
    {
        var today = DateTime.Today;
        var age = today.Year - dateOfBirth.Year;
        
        if (dateOfBirth.Date > today.AddYears(-age))
            age--;
            
        return age >= 13;
    }
}