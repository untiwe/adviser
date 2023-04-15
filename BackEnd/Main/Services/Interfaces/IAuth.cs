using Main.Contracts;

namespace Main.Services.Interfaces;

public interface IAuth
{
    string GetToken(GetTockenDTO userToken);
    Task RegisterUserAsync(RegisterUserDTO registerUSer);
    Task ChangeRoleUserAsync(ChangeRoleUser changeRole);
}

