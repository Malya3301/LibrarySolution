using Microsoft.AspNetCore.Mvc;

namespace Library.Server.Helpers;

public class ActionsResults
{
    public static IActionResult BadRequest()
    {
        return new BadRequestObjectResult(
            new
            {
                status = false,
                message = "Server error!",
            });
    }
}