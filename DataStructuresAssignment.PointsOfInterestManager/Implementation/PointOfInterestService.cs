using DataStructuresAssignment.PointsOfInterestManager.DBOs;
using DataStructuresAssignment.PointsOfInterestManager.DTOs;
using DataStructuresAssignment.PointsOfInterestManager.Enums;
using DataStructuresAssignment.PointsOfInterestManager.Interfaces;
using DataStructuresAssignment.PointsOfInterestManager.Wrappers;
using DataStructuresAssignment.Structures.Implementations;
using Microsoft.VisualBasic.FileIO;
using System.Globalization;
using System.Text;

namespace DataStructuresAssignment.PointsOfInterestManager.Implementation;

public class PointOfInterestService : IPointOfInterestService
{
    private long LastId { get; set; }
    private KdTree<PointOfInterestWrapper>? KdTree { get; set; }
    private QdTree<PointOfInterestWrapper, PointOfInterestBoundary>? QdTree { get; set; }
    private QdKdTree<PointOfInterestWrapper, PointOfInterestBoundary>? QdKdTree { get; set; }

    public ETreePicked TreePicked { get; private set; }
    public double MinLatitude { get; private set; }
    public double MinLongitude { get; private set; }
    public double MaxLatitude { get; private set; }
    public double MaxLongitude { get; private set; }

    public void Initialize(ETreePicked treePicked, double minLatitude = -90, double minLongitude = -180, double maxLatitude = 90, double maxLongitude = 180, int maxDepth = 50)
    {
        TreePicked = treePicked;
        MinLatitude = minLatitude;
        MinLongitude = minLongitude;
        MaxLatitude = maxLatitude;
        MaxLongitude = maxLongitude;

        LastId = 0;
        KdTree = null;
        QdTree = null;
        QdKdTree = null;

        switch (treePicked)
        {
            case ETreePicked.Kd:
                KdTree = new(2);
                break;
            case ETreePicked.Qd:
                QdTree = new(new(minLatitude), new(minLongitude), new(maxLatitude), new(maxLongitude), maxDepth);
                break;
            case ETreePicked.QdKd:
                QdKdTree = new(new(minLatitude), new(minLongitude), new(maxLatitude), new(maxLongitude), maxDepth);
                break;
            default:
                throw new InvalidOperationException("Invalid tree!");
        }
    }

    public List<PointOfInterestDto> FindAll()
    {
        var results = TreePicked switch
        {
            ETreePicked.Kd => KdTree!.FindRange(),
            ETreePicked.Qd => QdTree!.FindRange(),
            ETreePicked.QdKd => QdKdTree!.FindRange(),
            _ => throw new InvalidOperationException("Invalid tree!")
        };

        var response = new List<PointOfInterestDto>();

        foreach (var result in results)
        {
            response.Add(result.Data.ToDto());
        }

        return response;
    }

    public List<PointOfInterestDto> FindByCoordinates(double latitude, double longitude)
    {
        var wrapper = new PointOfInterestWrapper(new PointOfInterestDbo { Latitude = latitude, Longitude = longitude });

        var results = TreePicked switch
        {
            ETreePicked.Kd => KdTree!.Find(wrapper),
            ETreePicked.Qd => QdTree!.Find(wrapper),
            ETreePicked.QdKd => QdKdTree!.Find(wrapper),
            _ => throw new InvalidOperationException("Invalid tree!")
        };

        var response = new List<PointOfInterestDto>();

        foreach (var result in results)
        {
            response.Add(result.Data.ToDto());
        }

        return response;
    }

    /// <summary>Finds points using a fixed meter-to-degree ratio and a circle membership test.</summary>
    /// <remarks>
    /// https://stackoverflow.com/questions/11723503/formula-for-getting-longitude
    /// https://stackoverflow.com/questions/481144/equation-for-testing-if-a-point-is-inside-a-circle
    /// </remarks>
    public List<PointOfInterestDto> FindByCircle(double latitude, double longitude, double radiusInMeters)
    {
        if (radiusInMeters < 0)
            throw new ArgumentException("Negative radius!");

        double radiusInDegrees = radiusInMeters / 111_000;

        var min = new PointOfInterestWrapper(new PointOfInterestDbo { Latitude = latitude - radiusInDegrees, Longitude = longitude - radiusInDegrees });
        var max = new PointOfInterestWrapper(new PointOfInterestDbo { Latitude = latitude + radiusInDegrees, Longitude = longitude + radiusInDegrees });

        var results = TreePicked switch
        {
            ETreePicked.Kd => KdTree!.FindRange(min, max),
            ETreePicked.Qd => QdTree!.FindRange(min, max),
            ETreePicked.QdKd => QdKdTree!.FindRange(min, max),
            _ => throw new InvalidOperationException("Invalid tree!")
        };

        var response = new List<PointOfInterestDto>();

        foreach (var result in results)
        {
            double diffX = result.Data.Latitude - latitude;
            double diffY = result.Data.Longitude - longitude;

            if (diffX * diffX + diffY * diffY <= radiusInDegrees * radiusInDegrees)
            {
                response.Add(result.Data.ToDto());
            }
        }

        return response;
    }

    public List<PointOfInterestDto> FindByName(string name, double minLatitude, double minLongitude, double maxLatitude, double maxLongitude)
    {
        var min = new PointOfInterestWrapper(new PointOfInterestDbo { Latitude = minLatitude, Longitude = minLongitude });
        var max = new PointOfInterestWrapper(new PointOfInterestDbo { Latitude = maxLatitude, Longitude = maxLongitude });

        var results = TreePicked switch
        {
            ETreePicked.Kd => KdTree!.FindRange(min, max),
            ETreePicked.Qd => QdTree!.FindRange(min, max),
            ETreePicked.QdKd => QdKdTree!.FindRange(min, max),
            _ => throw new InvalidOperationException("Invalid tree!")
        };

        var response = new List<PointOfInterestDto>();

        foreach (var result in results)
        {
            if (CultureInfo.CurrentCulture.CompareInfo.IndexOf(result.Data.Name, name, CompareOptions.IgnoreNonSpace) >= 0)
                response.Add(result.Data.ToDto());
        }

        return response;
    }

    public PointOfInterestDto Insert(PointOfInterestDto point)
    {
        if (string.IsNullOrWhiteSpace(point.Name))
            throw new ArgumentException("Name is required!");

        var time = DateTime.UtcNow;

        var data = new PointOfInterestDbo
        {
            Id = checked(++LastId),
            Name = point.Name,
            Description = point.Description,
            Address = point.Address,
            Latitude = point.Latitude,
            Longitude = point.Longitude,
            CreatedAt = time,
            UpdatedAt = time
        };

        var wrapper = new PointOfInterestWrapper(data);

        bool inserted = TreePicked switch
        {
            ETreePicked.Kd => KdTree!.Insert(wrapper),
            ETreePicked.Qd => QdTree!.Insert(wrapper),
            ETreePicked.QdKd => QdKdTree!.Insert(wrapper),
            _ => throw new InvalidOperationException("Invalid tree!")
        };

        if (!inserted)
            throw new InvalidOperationException("Point was not inserted!");

        return data.ToDto();
    }

    public PointOfInterestDto Update(PointOfInterestDto pointOld, PointOfInterestDto pointNew)
    {
        if (string.IsNullOrWhiteSpace(pointNew.Name))
            throw new ArgumentException("Name is required!");

        var wrapper = new PointOfInterestWrapper(new PointOfInterestDbo { Latitude = pointOld.Latitude, Longitude = pointOld.Longitude });

        var results = TreePicked switch
        {
            ETreePicked.Kd => KdTree!.Find(wrapper),
            ETreePicked.Qd => QdTree!.Find(wrapper),
            ETreePicked.QdKd => QdKdTree!.Find(wrapper),
            _ => throw new InvalidOperationException("Invalid tree!")
        };

        PointOfInterestWrapper? previous = null;

        foreach (var result in results)
        {
            if (result.Data.Id == pointOld.Id)
            {
                previous = result;
                break;
            }
        }

        if (previous is null)
            throw new InvalidOperationException("Point was not found!");

        if (previous.Data.Latitude == pointNew.Latitude && previous.Data.Longitude == pointNew.Longitude)
        {
            previous.Data.Name = pointNew.Name;
            previous.Data.Description = pointNew.Description;
            previous.Data.Address = pointNew.Address;
            previous.Data.UpdatedAt = DateTime.UtcNow;

            return previous.Data.ToDto();
        }

        var data = new PointOfInterestDbo
        {
            Id = previous.Data.Id,
            Name = pointNew.Name,
            Description = pointNew.Description,
            Address = pointNew.Address,
            Latitude = pointNew.Latitude,
            Longitude = pointNew.Longitude,
            CreatedAt = previous.Data.CreatedAt,
            UpdatedAt = DateTime.UtcNow
        };

        var replacement = new PointOfInterestWrapper(data);

        bool deleted = TreePicked switch
        {
            ETreePicked.Kd => KdTree!.Delete(previous),
            ETreePicked.Qd => QdTree!.Delete(previous),
            ETreePicked.QdKd => QdKdTree!.Delete(previous),
            _ => throw new InvalidOperationException("Invalid tree!")
        };

        if (!deleted)
            throw new InvalidOperationException("Original point was not removed!");

        try
        {
            bool inserted = TreePicked switch
            {
                ETreePicked.Kd => KdTree!.Insert(replacement),
                ETreePicked.Qd => QdTree!.Insert(replacement),
                ETreePicked.QdKd => QdKdTree!.Insert(replacement),
                _ => throw new InvalidOperationException("Invalid tree!")
            };

            if (!inserted)
                throw new InvalidOperationException("Point was not updated!");
        }
        catch
        {
            switch (TreePicked)
            {
                case ETreePicked.Kd:
                    KdTree!.Insert(previous);
                    break;
                case ETreePicked.Qd:
                    QdTree!.Insert(previous);
                    break;
                case ETreePicked.QdKd:
                    QdKdTree!.Insert(previous);
                    break;
                default:
                    throw new InvalidOperationException("Invalid tree!");
            }

            throw;
        }

        return data.ToDto();
    }

    public PointOfInterestDto Delete(PointOfInterestDto point)
    {
        var wrapper = new PointOfInterestWrapper(new PointOfInterestDbo { Latitude = point.Latitude, Longitude = point.Longitude });

        var results = TreePicked switch
        {
            ETreePicked.Kd => KdTree!.Find(wrapper),
            ETreePicked.Qd => QdTree!.Find(wrapper),
            ETreePicked.QdKd => QdKdTree!.Find(wrapper),
            _ => throw new InvalidOperationException("Invalid tree!")
        };

        PointOfInterestWrapper? original = null;

        foreach (var result in results)
        {
            if (result.Data.Id == point.Id)
            {
                original = result;
                break;
            }
        }

        if (original is null)
            throw new InvalidOperationException("Point was not found!");

        bool deleted = TreePicked switch
        {
            ETreePicked.Kd => KdTree!.Delete(original),
            ETreePicked.Qd => QdTree!.Delete(original),
            ETreePicked.QdKd => QdKdTree!.Delete(original),
            _ => throw new InvalidOperationException("Invalid tree!")
        };

        if (!deleted)
            throw new InvalidOperationException("Point was not deleted!");

        return original.Data.ToDto();
    }

    public void Generate(int count, double minLatitude, double minLongitude, double maxLatitude, double maxLongitude)
    {
        if (count < 0)
            throw new ArgumentException("Count cannot be negative!");

        if (minLatitude > maxLatitude || minLongitude > maxLongitude)
            throw new ArgumentException("Invalid generation boundaries!");

        if (TreePicked != ETreePicked.Kd && (minLatitude < MinLatitude || minLongitude < MinLongitude || maxLatitude > MaxLatitude || maxLongitude > MaxLongitude))
            throw new ArgumentException("Generation boundaries must be inside tree boundaries!");

        var random = new Random();
        for (int i = 0; i < count; i++)
        {
            double latitude = Math.Round(minLatitude + random.NextDouble() * (maxLatitude - minLatitude), 6);
            double longitude = Math.Round(minLongitude + random.NextDouble() * (maxLongitude - minLongitude), 6);

            var point = new PointOfInterestDto
            {
                Name = $"Point #{LastId + 1}",
                Description = "Generated point",
                Address = "",
                Latitude = latitude,
                Longitude = longitude
            };

            Insert(point);
        }
    }

    public void CsvExport(string path)
    {
        var points = TreePicked switch
        {
            ETreePicked.Kd => KdTree!.FindRange(),
            ETreePicked.Qd => QdTree!.FindRange(),
            ETreePicked.QdKd => QdKdTree!.FindRange(),
            _ => throw new InvalidOperationException("Invalid tree!")
        };

        using var writer = new StreamWriter(path, false, Encoding.UTF8);
        writer.WriteLine("id;name;description;lat;lon;address;created_at;updated_at");

        foreach (var point in points)
        {
            var data = point.Data;

            string[] row =
            {
            data.Id.ToString(CultureInfo.InvariantCulture),
            data.Name,
            data.Description,
            data.Latitude.ToString(CultureInfo.InvariantCulture),
            data.Longitude.ToString(CultureInfo.InvariantCulture),
            data.Address ?? "",
            data.CreatedAt.ToString("O", CultureInfo.InvariantCulture),
            data.UpdatedAt.ToString("O", CultureInfo.InvariantCulture)
        };

            for (int i = 0; i < row.Length; i++)
            {
                row[i] = "\"" + row[i].Replace("\"", "\"\"") + "\"";
            }

            writer.WriteLine(string.Join(";", row));
        }
    }

    public void CsvImport(string path)
    {
        using var parser = new TextFieldParser(path, Encoding.UTF8);
        parser.SetDelimiters(";");
        parser.TrimWhiteSpace = false;
        parser.ReadFields();

        int maxDepth = TreePicked switch
        {
            ETreePicked.Kd => 0,
            ETreePicked.Qd => QdTree!.MaxDepth,
            ETreePicked.QdKd => QdKdTree!.MaxDepth,
            _ => throw new InvalidOperationException("Invalid tree!")
        };

        Initialize(TreePicked, MinLatitude, MinLongitude, MaxLatitude, MaxLongitude, maxDepth);

        while (!parser.EndOfData)
        {
            var row = parser.ReadFields()!;

            var data = new PointOfInterestDbo
            {
                Id = long.Parse(row[0]),
                Name = row[1],
                Description = row[2],
                Latitude = double.Parse(row[3]),
                Longitude = double.Parse(row[4]),
                Address = row[5],
                CreatedAt = DateTime.Parse(row[6]).ToUniversalTime(),
                UpdatedAt = DateTime.Parse(row[7]).ToUniversalTime()
            };

            var wrapper = new PointOfInterestWrapper(data);

            bool inserted = TreePicked switch
            {
                ETreePicked.Kd => KdTree!.Insert(wrapper),
                ETreePicked.Qd => QdTree!.Insert(wrapper),
                ETreePicked.QdKd => QdKdTree!.Insert(wrapper),
                _ => throw new InvalidOperationException("Invalid tree!")
            };

            if (!inserted)
                throw new InvalidOperationException("Point was not imported!");

            LastId = Math.Max(LastId, data.Id);
        }
    }
}