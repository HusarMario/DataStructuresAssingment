using DataStructuresAssignment.PointsOfInterestManager.DTOs;

namespace DataStructuresAssignment.PointsOfInterestManager.DBOs;

public class PointOfInterestDbo
{
    public long Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string? Address { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public PointOfInterestDto ToDto()
    {
        return new PointOfInterestDto
        {
            Id = Id,
            Name = Name,
            Description = Description,
            Address = Address,
            Latitude = Latitude,
            Longitude = Longitude,
            CreatedAt = CreatedAt.ToLocalTime(),
            UpdatedAt = UpdatedAt.ToLocalTime()
        };
    }
}