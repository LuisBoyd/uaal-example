namespace Server.Services;

public interface IUserService
{
    void Dosomething();
}

public class UserService : IUserService
{
    public void Dosomething()
    {
        Console.WriteLine("User Service");
    }
}

public class MockUserService : IUserService
{
    public void Dosomething()
    {
        Console.WriteLine("Mock User Service");
    }
}