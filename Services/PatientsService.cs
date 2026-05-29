using Microsoft.AspNetCore.Mvc;
using Cw8.DAK;
using Cw8.Models;
using Cw8.DTOs;
using Cw8.Exceptions;
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

    public async Task<BedAssignmentResponse> AddAsync(string pesel, CreateBedAssignmentRequest request, CancellationToken cancellationToken)
    {
        var patientExists = await ctx.Patients.AnyAsync(p => p.Pesel == pesel, cancellationToken);
        if (!patientExists) 
            throw new PatientNotFoundException("Patient not found.");
        
        var ward = await ctx.Wards.FirstOrDefaultAsync(w => w.Name == request.Ward, cancellationToken);
        if (ward == null) 
            throw new WardNotFoundException("Ward not found.");
        
        var bedType = await ctx.BedTypes.FirstOrDefaultAsync(bt => bt.Name == request.BedType, cancellationToken);
        if (bedType == null) 
            throw new BedTypeNotFoundException("BedType not found.");
        
        var availableBed = await ctx.Beds
            .Where(b => b.Room.WardId == ward.Id && b.BedTypeId == bedType.Id)
            .Where(b => !b.BedAssignments.Any(ba =>
                // Logika sprawdzania, czy rezerwacje się na siebie NIE nakładają.
                (!request.to.HasValue || ba.From < request.to) &&
                (!ba.To.HasValue || ba.To > request.From)
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (availableBed == null)
            throw new BedNotAvailableException("Bed not available in given time.");
        
        var bedAssignment = new BedAssignment
        {
            PatientPesel = pesel,
            BedId = availableBed.Id,
            From = request.From,
            To = request.to
        };

        ctx.BedAssignments.Add(bedAssignment);
        await ctx.SaveChangesAsync(cancellationToken);

        var responseDto = new BedAssignmentResponse(
            bedAssignment.Id,
            bedAssignment.PatientPesel,
            bedAssignment.BedId,
            bedAssignment.From,
            bedAssignment.To
        );
        
        return responseDto;
    }
}