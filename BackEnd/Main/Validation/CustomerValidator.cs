using FluentValidation;
using Main.Contracts;

namespace Main.Validation;

public class CustomerValidator : AbstractValidator<ChangeRoleUserDTO>
{

    public CustomerValidator()
    {
        RuleFor(customer => customer.Role)
            .Must(role => role == UsersRoles.User || role == UsersRoles.Admin)
            .WithMessage("Роль должна быть admin или user");


    }
}
