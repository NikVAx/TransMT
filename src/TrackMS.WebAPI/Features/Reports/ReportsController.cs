using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Npgsql;
using TrackMS.Data;
using TrackMS.Domain.Entities;
using TrackMS.WebAPI.Features.Reports.DTO;

namespace TrackMS.WebAPI.Features.Reports;

public class VehicleInfo
{
    public string Id { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string Number { get; set; } = null!;
    public string DeviceId { get; set; } = null!;
}

public class StatusInfo
{
    public string Status { get; set; } = null!;
    public double Duration { get; set; }
}

public class ReportByGeoZonesRow
{
    public string DeviceId { get; set; } = null!;
    public string VehicleId { get; set; } = null!;
    public string VehicleStatus { get; set; } = null!;
    public string VehicleType { get; set; } = null!;
    public string VehicleNumber { get; set; } = null!;
    public double TotalDuration { get; set; }

    public string GeoZoneId { get; set; } = null!;
    public string GeoZoneName { get; set; } = null!;
    public string GeoZoneType { get; set; } = null!;
}
  

[Route("api/[controller]")]
[ApiController]
public class ReportsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ReportsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("by-vehicle")]
    public async Task<ActionResult> GetReportByVehicleData(DateTimeOffset? from = null, DateTimeOffset? to = null)
    {
        var fromParam = new NpgsqlParameter("from",
            (from ?? new DateTimeOffset(0, new TimeSpan(0))).ToUniversalTime());
        var toParam = new NpgsqlParameter("to",
            (to ?? DateTimeOffset.UtcNow).ToUniversalTime());

        var selected = await _context.Database
            .SqlQueryRaw<ReportByVehicleRow>(@"
                WITH grouped AS (
                 SELECT 
                   ""DeviceId"", 
                   ""VehicleId"", 
                   ""Timestamp"", 
                   ""VehicleStatus"", 
                   LAG(""Timestamp"") OVER (
                     PARTITION BY ""DeviceId"" 
                     ORDER BY 
                       ""Timestamp""
                   ) AS prev_timestamp 
                 FROM 
                   ""LocationStamps""
                 WHERE
                    {0} <= ""Timestamp"" AND ""Timestamp"" <= {1} 
                ), 
                intervals AS (
                  SELECT 
                    ""DeviceId"",
                    ""VehicleId"", 
                    ""Timestamp"", 
                    ""VehicleStatus"", 
                    prev_timestamp, 
                    CASE WHEN ""Timestamp"" - prev_timestamp <= interval '10 minutes' 
                      THEN 0 ELSE 1 END AS new_group 
                  FROM 
                    grouped
                ), 
                grouped_with_ids AS (
                  SELECT 
                    ""DeviceId"", 
                    ""VehicleId"", 
                    ""Timestamp"", 
                    ""VehicleStatus"", 
                    SUM(new_group) OVER (
                      PARTITION BY ""DeviceId"" 
                      ORDER BY 
                        ""Timestamp""
                    ) AS group_id 
                  FROM 
                    intervals
                )
                SELECT
                  ""DeviceId"",
                  ""VehicleId"",
                  ""VehicleType"",
                  ""VehicleNumber"",
                  ""VehicleStatus"" as ""Status"",
                  Sum(""Duration"") as ""Duration""
                FROM (
                	SELECT 
                	  ""DeviceId"", 
                      ""VehicleId"",
                      ""Vehicles"".""Type"" as ""VehicleType"", 
                      ""Vehicles"".""Number"" as ""VehicleNumber"", 
                	  ""VehicleStatus"", 
                	  EXTRACT(
                		EPOCH 
                		FROM 
                		  ""Timestamp"" - LAG(""Timestamp"", 1, ""Timestamp"") OVER (
                			PARTITION BY ""DeviceId"", 
                			group_id 
                			ORDER BY 
                			  ""Timestamp""
                		  )
                	  ) AS ""Duration""
                	FROM 
                	  grouped_with_ids 
                    LEFT JOIN
                      ""Vehicles""
                    ON ""Vehicles"".""Id"" = ""VehicleId""
                	ORDER BY 
                	  ""DeviceId"", 
                	  group_id, 
                	  ""Timestamp""
                	) as data
                GROUP BY 
                    ""DeviceId"", 
                    ""VehicleStatus"",
                    ""VehicleId"",
                    ""VehicleType"",
                    ""VehicleNumber""
                ", fromParam, toParam).ToListAsync();

        var grouped = selected.AsEnumerable()
            .GroupBy(x => new
            {
                x.DeviceId,
                x.VehicleType,
                x.VehicleNumber,
                x.VehicleId
            })
            .Select(x => new ReportDataGroupedByDeviceDto
            {
                DeviceId = x.Key.DeviceId,
                VehicleType = x.Key.VehicleType,
                VehicleId = x.Key.VehicleId,
                VehicleNumber = x.Key.VehicleNumber,
                Statuses = x.Select(y => new StatusDuration
                {
                    Duration = y.Duration,
                    Status = y.Status
                }).ToList(),
                Duration = x.Sum(y => y.Duration)
            })
            .ToList();

        return Ok(grouped);
    }
    //12.06 17:40,  13.06, 12:56

    [HttpGet("by-geozones")]
    public async Task<ActionResult> GetReportByGeoZoneData(DateTimeOffset? from = null, DateTimeOffset? to = null)
    {
        var fromParam = new NpgsqlParameter("from", 
            (from ?? new DateTimeOffset(0, new TimeSpan(0))).ToUniversalTime());
        var toParam = new NpgsqlParameter("to", 
            (to ?? DateTimeOffset.UtcNow).ToUniversalTime());

        var selected = await _context.Database
            .SqlQueryRaw<ReportByGeoZonesRow>(@"
                WITH grouped AS (
                  SELECT 
                    ""DeviceId"", 
                	""VehicleId"",
                    ""Timestamp"" as ""T1"", 
                    ""VehicleStatus"", 
                    ST_GeographyFromText(
                      'SRID=4326;POINT(' || ""Lng"" || ' ' || ""Lat"" || ')'
                    ) as ""Point"", 
                    LAG(""Timestamp"") OVER (
                      PARTITION BY ""DeviceId"" 
                      ORDER BY 
                        ""Timestamp""
                    ) AS ""T0"" 
                  FROM 
                    ""LocationStamps""
                  WHERE
                    {0} <= ""Timestamp"" AND ""Timestamp"" <= {1} 
                ), 
                intervals AS (
                  SELECT 
                    ""DeviceId"", 
                	""VehicleId"",
                    ""T1"", 
                    ""Point"", 
                    ""VehicleStatus"", 
                    CASE WHEN ""T1"" - ""T0"" <= interval '3 minute' THEN 0 ELSE 1 END AS ""IsStartOfRoute"" 
                  FROM 
                    grouped
                ), 
                grouped_intervals AS (
                  SELECT 
                    ""DeviceId"", 
                	""VehicleId"",
                    ""T1"", 
                    ""Point"", 
                    ""VehicleStatus"", 
                    SUM(""IsStartOfRoute"") OVER (
                      PARTITION BY ""DeviceId"" 
                      ORDER BY 
                        ""T1""
                    ) AS ""RouteId"" 
                  FROM 
                    intervals
                ), 
                data AS (
                  SELECT 
                    ""DeviceId"", 
                    ""RouteId"", 
                	""VehicleId"",
                    ""Point"", 
                    ""VehicleStatus"", 
                    LAG(""T1"", 1, ""T1"") OVER (
                      PARTITION BY ""DeviceId"", 
                      ""RouteId"" 
                      ORDER BY 
                        ""T1""
                    ) as ""T0"", 
                    ""T1"", 
                    EXTRACT(
                      EPOCH 
                      FROM 
                        ""T1"" - LAG(""T1"", 1, ""T1"") OVER (
                          PARTITION BY ""DeviceId"", 
                          ""RouteId"" 
                          ORDER BY 
                            ""T1""
                        )
                    ) AS ""Duration"" 
                  FROM 
                    grouped_intervals 
                  ORDER BY 
                    ""DeviceId"", 
                    ""RouteId"", 
                    ""T1""
                ), 
                geozones_data as (
                  SELECT 
                    ""DeviceId"", 
                	""VehicleId"",
                    ""VehicleStatus"", 
                    ""GeoZones"".""Id"" AS ""GeoZoneId"", 
                    ""GeoZones"".""Name"" AS ""GeoZoneName"", 
                	""GeoZones"".""Type"" AS ""GeoZoneType"", 
                    ""Duration"" 
                  FROM 
                    data 
                    INNER JOIN ""GeoZones"" ON ST_CoveredBy(""Point"", ""GeoZones"".""Points"")
                ) 
                SELECT 
                  ""DeviceId"", 
                  ""VehicleId"",
                  ""VehicleStatus"",
                  ""Vehicles"".""Type"" as ""VehicleType"",
                  ""Vehicles"".""Number"" as ""VehicleNumber"",
                  ""GeoZoneId"", 
                  ""GeoZoneName"", 
                  ""GeoZoneType"",
                  SUM(""Duration"") as ""TotalDuration"" 
                FROM 
                  geozones_data 
                INNER JOIN 
                  ""Vehicles""
                ON
                  ""VehicleId"" = ""Vehicles"".""Id""
                GROUP BY 
                  ""DeviceId"", 
                  ""VehicleStatus"", 
                  ""GeoZoneId"", 
                  ""GeoZoneName"",
                  ""GeoZoneType"",
                  ""VehicleId"",
                  ""VehicleType"",
                  ""VehicleNumber""
                ORDER BY 
                  ""GeoZoneId"", 
                  ""DeviceId"", 
                  ""VehicleStatus"";
                ", fromParam, toParam).ToListAsync();

        var grouped = selected.AsEnumerable()
            .GroupBy(row => new
            {
                Id = row.GeoZoneId,
                Name = row.GeoZoneName,
                Type = row.GeoZoneType
            })
            .Select(byGeoZone => new
            {
                Id = byGeoZone.Key.Id,
                Name = byGeoZone.Key.Name,
                Type = byGeoZone.Key.Type,
                Duration = byGeoZone.Sum(x => x.TotalDuration),
                Statuses = byGeoZone.GroupBy(row => row.VehicleStatus).Select(byStatus => new StatusDuration
                {
                    Status = byStatus.Key,
                    Duration = byStatus.Sum(x =>x.TotalDuration),
                }),
                Vehicles = byGeoZone.GroupBy(row => new
                {
                    Id = row.VehicleId,
                    Type = row.VehicleType,
                    Number = row.VehicleNumber,
                    DeviceId = row.DeviceId,
                })
                .Select(byVehicle => new
                {
                    Id = byVehicle.Key.Id,
                    Type = byVehicle.Key.Type,
                    Number = byVehicle.Key.Number,
                    DeviceId = byVehicle.Key.DeviceId,
                    Duration = byVehicle.Sum(x => x.TotalDuration),
                    Statuses = byVehicle.Select(row => new StatusDuration
                    {
                        Status = row.VehicleStatus,
                        Duration = row.TotalDuration
                    })
                })
            })
            .ToList();

        

        return Ok(new {
            Statuses = selected.Select(x => x.VehicleStatus).Distinct().ToList(),
            GeoZones = grouped,
            Duration = selected.Sum(x => x.TotalDuration),
        });
    }
}