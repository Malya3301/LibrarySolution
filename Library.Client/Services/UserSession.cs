namespace Library.Client.Services;

public static class UserSession
{
    public static string Role { get; set; } = "User";
    public static bool IsAdmin => Role == "Admin";
}