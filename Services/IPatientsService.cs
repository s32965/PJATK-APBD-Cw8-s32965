using Cw8.DTOs;

namespace Cw8.Services;

public interface IPatientsService
{
    Task<IEnumerable<PatientsListResponse>> GetAllAsync(string search, CancellationToken cancellationToken);
}