namespace Cw8.DTOs;

public record BedListResponse(
    int Id,
    BedTypeResponse BedType,
    RoomResponse Room
    );