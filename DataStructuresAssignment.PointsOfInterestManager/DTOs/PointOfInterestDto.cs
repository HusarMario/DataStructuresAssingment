using DataStructuresAssignment.PointsOfInterestManager.DBOs;

namespace DataStructuresAssignment.PointsOfInterestManager.DTOs;

public class PointOfInterestDto
{
    public long Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string? Address { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public PointOfInterestDbo ToDbo()
    {
        return new PointOfInterestDbo
        {
            Id = Id,
            Name = Name,
            Description = Description,
            Address = Address,
            Latitude = Latitude,
            Longitude = Longitude,
            CreatedAt = CreatedAt.ToUniversalTime(),
            UpdatedAt = UpdatedAt.ToUniversalTime()
        };
    }
}
