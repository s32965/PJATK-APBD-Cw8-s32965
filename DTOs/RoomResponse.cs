namespace Cw8.DTOs;

public record RoomResponse(
    string Id,
    bool hasTv,
    WardListResponse Ward
    );