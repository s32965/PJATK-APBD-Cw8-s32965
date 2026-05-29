namespace Cw8.DTOs;

public record AdmissionsListResponse (
    int Id,
    DateTime AdmissionDate,
    DateTime? DischargeDate,
    WardListResponse Ward
    );