namespace Cw8.DTOs;

public record PatientsListResponse (
    string Pesel,
    string FirtName,
    string LastName,
    int Age,
    bool Sex,
    IEnumerable<AdmissionsListResponse> Admissions,
    IEnumerable<BedAssignmentsListResponse> BedAssignments
    );