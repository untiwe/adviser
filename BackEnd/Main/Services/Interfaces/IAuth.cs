namespace Main.Services.Interfaces;

public interface IAuth
{
    string GetToken(string username, string password);
}

