using Microsoft.AspNetCore.Mvc;
using Cw8.Services;

namespace Cw8.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientsController(IPatientsService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search, CancellationToken cancellationToken)
    {
        return Ok(await service.GetAllAsync(search, cancellationToken));
    }
}