namespace Cw8.DTOs;

public record BedAssignmentResponse(
    int Id,
    string PatientPesel,
    int BedId,
    DateTime From,
    DateTime? To
    );