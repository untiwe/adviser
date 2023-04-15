using Main.Contracts;

namespace Main.Services.Interfaces;

public interface IAuth
{
    string GetToken(string username, string password);
    Task RegisterUserAsync(RegisterUserDTO registerUSer);
    Task ChangeRoleUserAsync(ChangeRoleUser changeRole);
}

