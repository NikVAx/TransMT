using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrackMS.Data;
using TrackMS.WebAPI.Features.Reports.DTO;
using TrackMS.WebAPI.Features.Tracking;

namespace TrackMS.WebAPI.Features.Reports;

public class ReportByGeoZonesItem
{

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
    public async Task<ActionResult> GetReportByVehicleTypeData()
    {
        var selected = await _context.Database
            .SqlQueryRaw<DurationInStatusForDeviceDto>(
            $@"
WITH grouped AS (
  SELECT 
    ""DeviceId"", 
    ""Timestamp"", 
    ""VehicleStatus"", 
    LAG(""Timestamp"") OVER (
      PARTITION BY ""DeviceId"" 
      ORDER BY 
        ""Timestamp""
    ) AS prev_timestamp 
  FROM 
    ""LocationStamps""
), 
intervals AS (
  SELECT 
    ""DeviceId"", 
    ""Timestamp"", 
    ""VehicleStatus"", 
    prev_timestamp, 
    CASE WHEN ""Timestamp"" - prev_timestamp <= interval '10 minutes' THEN 0 ELSE 1 END AS new_group 
  FROM 
    grouped
), 
grouped_with_ids AS (
  SELECT 
    ""DeviceId"", 
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
  ""VehicleStatus"",
  Sum(""Duration"") as ""TotalDuration""
FROM (
	SELECT 
	  ""DeviceId"", 
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
	ORDER BY 
	  ""DeviceId"", 
	  group_id, 
	  ""Timestamp""
	)
GROUP BY ""DeviceId"", ""VehicleStatus"";

                ").ToListAsync();

        var grouped = selected.AsEnumerable()
            .GroupBy(x => x.DeviceId)
            .Select(x => new ReportDataGroupedByDeviceDto
            {
                DeviceId = x.Key,
                GroupsByStatus = x.Select(y => new DurationInStatusDto
                {
                    TotalDuration = y.TotalDuration,
                    VehicleStatus = y.VehicleStatus
                }).ToList(),
                TotalDuration = x.Sum(y => y.TotalDuration)
            })
            .ToList();


        return Ok(grouped);
    }

    [HttpGet("by-geozones")]
    public async Task<ActionResult> GetReportByGeoZoneData()
    {
        var selected = await _context.Database
               .SqlQueryRaw<ReportByGeoZonesItem>(@"

            ").ToListAsync();   

        return Ok();
    }
}

/*
SELECT 
  *, 
  ST_GeographyFromText('SRID=4326;POINT(' || "Lng" || ' ' || "Lat" || ')') as "Point"
FROM "LocationStamps"
INNER JOIN "GeoZones" ON
ST_Within(
    ST_GeomFromText('SRID=4326;POINT(' || "Lng" || ' ' || "Lat" || ')')::geometry,
    "GeoZones"."Points"::geometry
  );
 */

/*
 SELECT 
  *, 
  ST_GeographyFromText('SRID=4326;POINT(' || "Lng" || ' ' || "Lat" || ')') as "Point"
FROM "LocationStamps"
INNER JOIN "GeoZones" ON
ST_Within(
    ST_GeomFromText('SRID=4326;POINT(' || "Lng" || ' ' || "Lat" || ')')::geometry,
    "GeoZones"."Points"::geometry
  );
 
 
 */

/*
 SELECT 
  *, 
  LAG("Current", 1, "Current") 
    OVER (PARTITION BY "DeviceId" ORDER BY "Current") as "LagTime",
  LEAD("Current", 1, "Current")
  	OVER (PARTITION BY "DeviceId" ORDER BY "Current") as "LeadTime"
FROM (
  SELECT
    *,
    "Timestamp" AS "Current"
  FROM
    "LocationStamps"
  ) as BASE_TABLE;

 
 */


/*
     LEFT JOIN "GeoZones" ON ST_Within(
    ST_GeomFromText(
      'SRID=4326;POINT(' || "Lng" || ' ' || "Lat" || ')'
    ):: geometry, 
    "GeoZones"."Points" :: geometry
  ) 

SELECT 
  *, 
  LAG("Current", 1, "Current") OVER (
    PARTITION BY "DeviceId" 
    ORDER BY 
      "Current"
  ) as "LagTime", 
  LEAD("Current", 1, "Current") OVER (
    PARTITION BY "DeviceId" 
    ORDER BY 
      "Current"
  ) as "LeadTime", 
  EXTRACT(
    EPOCH 
    FROM 
      "Current" - LAG("Current", 1, "Current") OVER (
        PARTITION BY "DeviceId" 
        ORDER BY 
          "Current"
      )
  ) AS "Duration", 
  ST_GeographyFromText(
    'SRID=4326;POINT(' || "Lng" || ' ' || "Lat" || ')'
  ) as "Point" 
FROM 
  (
    SELECT 
      *, 
      "Timestamp" AS "Current" 
    FROM 
      "LocationStamps"
  ) as BASE_TABLE
ORDER BY 
  "Current";

 
 
 
 */

/*
 SELECT 
  *, 
  LAG("Current", 1, "Current") OVER (
    PARTITION BY "DeviceId" 
    ORDER BY 
      "Current"
  ) as "LagTime", 
  LEAD("Current", 1, "Current") OVER (
    PARTITION BY "DeviceId" 
    ORDER BY 
      "Current"
  ) as "LeadTime", 
  EXTRACT(
    EPOCH 
    FROM 
      "Current" - LAG("Current", 1, "Current") OVER (
        PARTITION BY "DeviceId" 
        ORDER BY 
          "Current"
      )
  ) AS "Duration", 
  ST_GeographyFromText(
    'SRID=4326;POINT(' || "Lng" || ' ' || "Lat" || ')'
  ) as "Point" 
FROM 
  (
    SELECT 
      *, 
      "Timestamp" AS "Current" 
    FROM 
      "LocationStamps"
  ) as BASE_TABLE
ORDER BY 
  "Current";

 
 */





//
/*

WITH grouped AS (
  SELECT 
    "DeviceId", 
    "Timestamp", 
    "VehicleStatus", 
    LAG("Timestamp") OVER (
      PARTITION BY "DeviceId" 
      ORDER BY 
        "Timestamp"
    ) AS prev_timestamp 
  FROM 
    "LocationStamps"
), 
intervals AS (
  SELECT 
    "DeviceId", 
    "Timestamp", 
    "VehicleStatus", 
    prev_timestamp, 
    CASE WHEN "Timestamp" - prev_timestamp <= interval '10 minutes' THEN 0 ELSE 1 END AS new_group 
  FROM 
    grouped
), 
grouped_with_ids AS (
  SELECT 
    "DeviceId", 
    "Timestamp", 
    "VehicleStatus", 
    SUM(new_group) OVER (
      PARTITION BY "DeviceId" 
      ORDER BY 
        "Timestamp"
    ) AS group_id 
  FROM 
    intervals
) 
SELECT 
  "DeviceId", 
  "Timestamp", 
  "VehicleStatus", 
  EXTRACT(
    EPOCH 
    FROM 
      "Timestamp" - LAG("Timestamp", 1, "Timestamp") OVER (
        PARTITION BY "DeviceId", 
        group_id 
        ORDER BY 
          "Timestamp"
      )
  ) AS "Duration", 
  group_id 
FROM 
  grouped_with_ids 
ORDER BY 
  "DeviceId", 
  group_id, 
  "Timestamp";
 
 
*/