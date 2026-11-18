using DataStructuresAssignment.PointsOfInterestManager.DTOs;
using DataStructuresAssignment.PointsOfInterestManager.Enums;

namespace DataStructuresAssignment.PointsOfInterestManager.Interfaces;

public interface IPointOfInterestService
{
    ETreePicked TreePicked { get; }

    void Initialize(ETreePicked treePicked, double minLatitude = -90, double minLongitude = -180, double maxLatitude = 90, double maxLongitude = 180, int maxDepth = 50);

    List<PointOfInterestDto> FindAll();
    List<PointOfInterestDto> FindByCoordinates(double latitude, double longitude);
    List<PointOfInterestDto> FindByCircle(double latitude, double longitude, double radiusInMeters);
    List<PointOfInterestDto> FindByName(string name, double minLatitude, double minLongitude, double maxLatitude, double maxLongitude);

    PointOfInterestDto Insert(PointOfInterestDto point);
    PointOfInterestDto Update(PointOfInterestDto pointOld, PointOfInterestDto pointNew);
    PointOfInterestDto Delete(PointOfInterestDto point);

    void Generate(int count, double minLatitude, double minLongitude, double maxLatitude, double maxLongitude);
    void CsvExport(string path);
    void CsvImport(string path);
}