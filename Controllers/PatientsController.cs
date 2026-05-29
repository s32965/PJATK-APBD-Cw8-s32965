using Cw8.DTOs;
using Microsoft.AspNetCore.Mvc;
using Cw8.Services;
using Cw8.Exceptions;

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

    [HttpPost("{pesel}/bedassignments")]
    public async Task<IActionResult> AddBedAssignments([FromRoute] string pesel, [FromBody] CreateBedAssignmentRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await service.AddAsync(pesel, request, cancellationToken);
            return CreatedAtAction(nameof(GetAll), new { id = result.Id }, result);
        }
        catch (PatientNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (WardNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (BedTypeNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (BedNotAvailableException e)
        {
            return NotFound(e.Message);
        }
    }
}