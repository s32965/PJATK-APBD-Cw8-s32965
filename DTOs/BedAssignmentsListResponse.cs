namespace Cw8.DTOs;

public record BedAssignmentsListResponse (
    int Id,
    DateTime From,
    DateTime? To,
    BedListResponse Bed
    ); 