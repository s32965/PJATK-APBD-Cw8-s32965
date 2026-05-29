using System.ComponentModel.DataAnnotations;

namespace Cw8.DTOs;

public record CreateBedAssignmentRequest(
    [Required(ErrorMessage = "Bed assignment start date is required.")] DateTime From,
    DateTime? to,
    [Required(ErrorMessage = "Bed type is required.")] string BedType,
    [Required(ErrorMessage = "Ward is required.")] string Ward
    );