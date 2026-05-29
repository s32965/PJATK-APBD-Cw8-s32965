using Microsoft.AspNetCore.Mvc;
using Cw8.DAK;
using Cw8.Models;
using Cw8.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Cw8.Services;

public class PatientsService(HospitalDbContext ctx) : IPatientsService
{
    public async Task<IEnumerable<PatientsListResponse>> GetAllAsync(string search, CancellationToken cancellationToken)
    {
        return await ctx.Patients
            .Where(p => string.IsNullOrWhiteSpace(search) ||
                       EF.Functions.Like(p.FirstName, $"%{search}%") ||
                       EF.Functions.Like(p.LastName, $"%{search}%"))
            .Select(p => new PatientsListResponse(
            p.Pesel,
            p.FirstName,
            p.LastName,
            p.Age,
            p.Sex,
            p.Admissions.Select(ad => new AdmissionsListResponse(
                ad.Id,
                ad.AdmissionDate,
                ad.DischargeDate,
                new WardListResponse(
                    ad.Ward.Id,
                    ad.Ward.Name,
                    ad.Ward.Description
                )
            )),
            p.BedAssignments.Select(ba => new BedAssignmentsListResponse(
                ba.Id,
                ba.From,
                ba.To,
                new BedListResponse(
                    ba.Bed.Id,
                    new BedTypeResponse(
                        ba.Bed.BedType.Id,
                        ba.Bed.BedType.Name,
                        ba.Bed.BedType.Description
                    ),
                    new RoomResponse(
                        ba.Bed.Room.Id,
                        ba.Bed.Room.HasTv,
                        new WardListResponse(
                            ba.Bed.Room.Ward.Id,
                            ba.Bed.Room.Ward.Name,
                            ba.Bed.Room.Ward.Description
                        )
                    )
                )
            ))
        )).ToListAsync(cancellationToken);
    }
}