using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Xml.Linq;
using TrackMS.Data;
using TrackMS.Domain.Entities;
using TrackMS.Domain.ValueTypes;
using TrackMS.WebAPI.Features.Buildings;
using TrackMS.WebAPI.Features.GeoZones;
using TrackMS.WebAPI.Features.IdentityManagement.Roles;
using TrackMS.WebAPI.Features.Roles.IdentityManagement.DTO;
using TrackMS.WebAPI.Features.Users;
using TrackMS.WebAPI.Shared.Models;
using TrackMS.WebAPI.Shared.Utils;

namespace TrackMS.WebAPI.Features.Development;

[Route("api/[controller]")]
[ApiController]    
public class DevelopmentController : ControllerBase
{
    private readonly AuthDbContext _authDbContext;
    private readonly ApplicationDbContext _appDbContext;
    private readonly UsersService _usersService;
    private readonly RolesService _rolesService;

    public DevelopmentController(
        AuthDbContext authDbContext, 
        ApplicationDbContext appDbContext,
        UsersService usersService,
        RolesService rolesService)
    {
        _authDbContext = authDbContext;
        _appDbContext = appDbContext;
        _usersService = usersService;
        _rolesService = rolesService;
    }

    [HttpPost("rst")]
    public async Task<ActionResult> Setup(bool forData = true, bool forAuth = true)
    {
        if(forData)
        {
            _appDbContext.Database.EnsureDeleted();
            _appDbContext.Database.EnsureCreated();
            _appDbContext.GeoZones.AddRange(
                new GeoZone
                {
                    Id = Guid.NewGuid().ToString(),
                    Color = "008000",
                    Name = "Территория дамба Гребёнка",
                    Type = "Зона хранения",
                    Points = GeoUtil.CreatePolygon(
                    [
                        new GeoPoint(
                            59.88720371276617,
                            30.2197140455246
                        ),
                        new GeoPoint(
                            59.894947169128855,
                            30.230083465576175
                        ),
                        new GeoPoint(
                            59.89520245044669,
                            30.230416059494022
                        ),
                        new GeoPoint(
                            59.894174586442766,
                            30.23422479629517
                        ),
                        new GeoPoint(
                            59.89366871499377,
                            30.233151912689213
                        ),
                        new GeoPoint(
                            59.890321158723175,
                            30.22876381874085
                        ),
                        new GeoPoint(
                            59.89025119012228,
                            30.22849559783936
                        ),
                        new GeoPoint(
                            59.88619056492377,
                            30.223170588361594
                        ),
                        new GeoPoint(
                            59.886303604864814,
                            30.222129891263815
                        )
                    ])
                },
                new GeoZone
                {
                    Id = Guid.NewGuid().ToString(),
                    Color = "008000",
                    Name = "Новая Канонерская гавань",
                    Type = "Зона хранения",
                    Points = GeoUtil.CreatePolygon(
                    [
                        new GeoPoint(
                           59.90489731585696,
                           30.22098004817963
                        ),
                        new GeoPoint(
                           59.904668672163744,
                           30.222085118293766
                        ),
                        new GeoPoint(
                           59.90444053126254,
                           30.22301316261292
                        ),
                        new GeoPoint(
                           59.9042414741037,
                           30.22359788417816
                        ),
                        new GeoPoint(
                           59.9041849846926,
                           30.223748087883
                        ),
                        new GeoPoint(
                           59.904230209853786,
                           30.22394120693207
                        ),
                        new GeoPoint(
                           59.90431090887315,
                           30.224037766456604
                        ),
                        new GeoPoint(
                           59.90430014901524,
                           30.224107503890995
                        ),
                        new GeoPoint(
                           59.90452610529938,
                           30.224429368972782
                        ),
                        new GeoPoint(
                           59.90448844602544,
                           30.224536657333378
                        ),
                        new GeoPoint(
                           59.9046637966561,
                           30.224783420562748
                        ),
                        new GeoPoint(
                           59.90471490539044,
                           30.224622488021854
                        ),
                        new GeoPoint(
                           59.907703284609,
                           30.22871553897858
                        ),
                        new GeoPoint(
                           59.9076548701674,
                           30.22897839546204
                        ),
                        new GeoPoint(
                           59.9086043182784,
                           30.230153203010563
                        ),
                        new GeoPoint(
                           59.9098092444933,
                           30.22972941398621
                        ),
                        new GeoPoint(
                           59.91236687819731,
                           30.233162641525272
                        ),
                        new GeoPoint(
                           59.912243169640355,
                           30.233607888221744
                        ),
                        new GeoPoint(
                           59.912280820119534,
                           30.23418724536896
                        ),
                        new GeoPoint(
                           59.91270035114146,
                           30.23472905158997
                        ),
                        new GeoPoint(
                           59.912893979055674,
                           30.234820246696476
                        ),
                        new GeoPoint(
                           59.91303382073577,
                           30.234847068786625
                        ),
                        new GeoPoint(
                           59.91313063386149,
                           30.234825611114506
                        ),
                        new GeoPoint(
                           59.913262406828935,
                           30.23473978042603
                        ),
                        new GeoPoint(
                           59.91337804390006,
                           30.234546661376957
                        ),
                        new GeoPoint(
                           59.9136039384118,
                           30.233833193778995
                        ),
                        new GeoPoint(
                           59.91361738446521,
                           30.232620835304264
                        ),
                        new GeoPoint(
                           59.91374108790109,
                           30.232406258583072
                        ),
                        new GeoPoint(
                           59.913555532574414,
                           30.228919386863712
                        ),
                        new GeoPoint(
                           59.91349905900825,
                           30.228790640830997
                        ),
                        new GeoPoint(
                           59.91093932737288,
                           30.228034257888797
                        ),
                        new GeoPoint(
                           59.9092072911459,
                           30.22571682929993
                        ),
                        new GeoPoint(
                           59.90903515838589,
                           30.225116014480594
                        ),
                        new GeoPoint(
                           59.909180395460965,
                           30.224021673202518
                        ),
                        new GeoPoint(
                           59.90902440005876,
                           30.223882198333744
                        ),
                        new GeoPoint(
                           59.90889529986124,
                           30.224879980087284
                        ),
                        new GeoPoint(
                           59.906485337362334,
                           30.222111940383915
                        ),
                        new GeoPoint(
                           59.90641002321565,
                           30.222390890121464
                        ),
                        new GeoPoint(
                           59.90587206005714,
                           30.221875905990604
                        ),
                        new GeoPoint(
                           59.90543092376349,
                           30.220867395401005
                        ),
                        new GeoPoint(
                           59.9053120647736,
                           30.220556259155277
                        ),
                        new GeoPoint(
                           59.90519908949774,
                           30.22049188613892
                        ),
                        new GeoPoint(
                           59.90513184212722,
                           30.22048652172089
                        ),
                        new GeoPoint(
                           59.90516899631628,
                           30.220888853073124
                        ),
                        new GeoPoint(
                           59.905061400360594,
                           30.22093713283539
                        ),
                        new GeoPoint(
                           59.905040385484796,
                           30.221081972122196
                        ),
                    ])
                },
                new GeoZone
                {
                    Id = Guid.NewGuid().ToString(),
                    Color = "008000",
                    Name = "2-й район Морского Порта",
                    Type = "Зона хранения",
                    Points = GeoUtil.CreatePolygon(
                    [
                        new GeoPoint(
                            59.88999284324258,
                            30.207831859588627
                        ),
                        new GeoPoint(
                            59.88890022901868,
                            30.211104154586796
                        ),
                        new GeoPoint(
                            59.89289912982513,
                            30.216372013092045
                        ),
                        new GeoPoint(
                            59.89232327471694,
                            30.21797060966492
                        ),
                        new GeoPoint(
                            59.89758632673644,
                            30.224922895431522
                        ),
                        new GeoPoint(
                            59.89868403616741,
                            30.22560954093933
                        ),
                        new GeoPoint(
                            59.89906069280861,
                            30.225555896759037
                        ),
                        new GeoPoint(
                            59.900287487684096,
                            30.221832990646366
                        ),
                        new GeoPoint(
                            59.8954032434646,
                            30.21520793437958
                        ),
                        new GeoPoint(
                            59.89546563367778,
                            30.21497189998627
                        )
                    ])
                },
                new GeoZone
                {
                    Id = Guid.NewGuid().ToString(),
                    Color = "008000",
                    Name = "3-й район Морского Порта",
                    Type = "Зона хранения",
                    Points = GeoUtil.CreatePolygon(
                    [
                        new GeoPoint(
                            59.885763136620454,
                            30.193691253662113
                        ),
                        new GeoPoint(
                            59.88499454586958,
                            30.184711217880253
                        ),
                        new GeoPoint(
                            59.88474154121722,
                            30.184485912323
                        ),
                        new GeoPoint(
                            59.884257058847744,
                            30.18427670001984
                        ),
                        new GeoPoint(
                            59.883917916986775,
                            30.187924504280094
                        ),
                        new GeoPoint(
                            59.882601690795276,
                            30.189464092254642
                        ),
                        new GeoPoint(
                            59.882706667803184,
                            30.19378781318665
                        ),
                        new GeoPoint(
                            59.88248863826209,
                            30.19378781318665
                        ),
                        new GeoPoint(
                            59.88251824729604,
                            30.19578874111176
                        ),
                        new GeoPoint(
                            59.88223023102796,
                            30.196309089660648
                        ),
                        new GeoPoint(
                            59.882391735784715,
                            30.196647047996525
                        ),
                        new GeoPoint(
                            59.878919210563375,
                            30.203304290771488
                        ),
                        new GeoPoint(
                            59.87860424265743,
                            30.2029824256897
                        ),
                        new GeoPoint(
                            59.87735595107688,
                            30.20021438598633
                        ),
                        new GeoPoint(
                            59.873845205726624,
                            30.207080841064457
                        ),
                        new GeoPoint(
                            59.875288317708694,
                            30.210299491882328
                        ),
                        new GeoPoint(
                            59.87578168071109,
                            30.210363864898685
                        ),
                        new GeoPoint(
                            59.8757042777229,
                            30.21327137947083
                        ),
                        new GeoPoint(
                            59.87461657841259,
                            30.215406417846683
                        ),
                        new GeoPoint(
                            59.87411040917493,
                            30.216640233993534
                        ),
                        new GeoPoint(
                            59.87292034263183,
                            30.219300985336307
                        ),
                        new GeoPoint(
                            59.871474435242156,
                            30.223039984703068
                        ),
                        new GeoPoint(
                            59.870482861848274,
                            30.22650003433228
                        ),
                        new GeoPoint(
                            59.87149530306785,
                            30.228109359741214
                        ),
                        new GeoPoint(
                            59.873862706526985,
                            30.22126436233521
                        ),
                        new GeoPoint(
                            59.876320804351586,
                            30.215873122215275
                        ),
                        new GeoPoint(
                            59.87598158150776,
                            30.21524012088776
                        ),
                        new GeoPoint(
                            59.87629926649617,
                            30.214644670486454
                        ),
                        new GeoPoint(
                            59.87651195220712,
                            30.214655399322513
                        ),
                        new GeoPoint(
                            59.87668425355754,
                            30.214387178421024
                        ),
                        new GeoPoint(
                            59.87782976561242,
                            30.21669387817383
                        ),
                        new GeoPoint(
                            59.87788545856774,
                            30.216549038887027
                        ),
                        new GeoPoint(
                            59.87803083188462,
                            30.216876268386844
                        ),
                        new GeoPoint(
                            59.88710882153028,
                            30.199763826833905
                        ),
                        new GeoPoint(
                            59.88690427774027,
                            30.198798231588544
                        ),
                        new GeoPoint(
                            59.88672126380831,
                            30.19890551994914
                        )
                    ])
                }
            );

            var building1 = new Building
            {
                Id = Guid.NewGuid().ToString(),
                Address = "198096, г Санкт-Петербург, Кировский р-н, ул Корабельная",
                Location = new NetTopologySuite.Geometries.Point(30.23244381, 59.87678235),
                Name = "Судостроительный завод \"Серверная верфь\", цех №047",
                Type = "Техническое",
            };

            var building2 = new Building
            {
                Id = Guid.NewGuid().ToString(),
                Address = "198184, г Санкт-Петербург, Кировский р-н, Канонерский остров, д 20",
                Location = new NetTopologySuite.Geometries.Point(30.219185, 59.902449),
                Name = "Ангар №1",
                Type = "Ангар/Гараж"
            };

            var vehicle1 = new Vehicle
            {
                Id = Guid.NewGuid().ToString(),
                Number = "A220BB",
                OperatingStatus = "Нет",
                StorageAreaId = building2.Id,
                Type = "Контейнеровоз"
            };
            var vehicle2 = new Vehicle
            {
                Id = Guid.NewGuid().ToString(),
                Number = "A221KM",
                OperatingStatus = "Нет",
                StorageAreaId = building2.Id,
                Type = "Контейнеровоз"
            };
            var vehicle3 = new Vehicle
            {
                Id = Guid.NewGuid().ToString(),
                Number = "B123AC",
                OperatingStatus = "Нет",
                StorageAreaId = building2.Id,
                Type = "Грузовой автомобиль"
            };
            var vehicle4 = new Vehicle
            {
                Id = Guid.NewGuid().ToString(),
                Number = "M245AC",
                OperatingStatus = "Нет",
                StorageAreaId = building2.Id,
                Type = "Грузовой автомобиль"
            };
            var vehicle5 = new Vehicle
            {
                Id = Guid.NewGuid().ToString(),
                Number = "X975YZ",
                OperatingStatus = "Нет",
                StorageAreaId = building2.Id,
                Type = "Грузовой автомобиль"
            };

            var device1 = new Device
            {
                Id = "83746371",
                VehicleId = vehicle1.Id,
            };
            var device2 = new Device
            {
                Id = "81237264",
                VehicleId = vehicle2.Id,
            };
            var device3 = new Device
            {
                Id = "45876549",
                VehicleId = vehicle3.Id,
            };
            var device4 = new Device
            {
                Id = "86469465",
                VehicleId = vehicle4.Id,
            };
            var device5 = new Device
            {
                Id = "87465764",
                VehicleId = vehicle5.Id,
            };


            _appDbContext.Buildings.AddRange(building1, building2);
            _appDbContext.Vehicles.AddRange(vehicle1, vehicle2, vehicle3, vehicle4, vehicle5);
            _appDbContext.Devices.AddRange(device1, device2, device3, device4, device5);

            _appDbContext.SaveChanges();
        }

        if (forAuth)
        {
            _authDbContext.Database.EnsureDeleted();
            _authDbContext.Database.EnsureCreated();

            var permissions = Permissions.GetPermissions();
                
            var identityPerms = permissions
                .Where(x => x.Name.Contains("USER") || x.Name.Contains("ROLE"))
                .ToList();

            _authDbContext.Permissions.AddRange(permissions);
            _authDbContext.SaveChanges();
            _authDbContext.ChangeTracker.Clear();

            var superUserRole = await _rolesService.CreateRoleAsync(new CreateRoleDto
            {
                Name = "superuser",
                Description = "Пользователь со всеми разрешениями доступными в системе",
                Permissions = permissions.Select(permission => permission.Id)
            });

            var identityManagerRole = await _rolesService.CreateRoleAsync(new CreateRoleDto
            {
                Name = "identity-admin",
                Permissions = identityPerms.Select(permission => permission.Id)
            });

            _authDbContext.ChangeTracker.Clear();

            await _usersService.CreateUserAsync(
                new Users.DTO.CreateUserDto
                {
                    Email = "admin@vkusnuts.online",
                    Username = "admin",
                    Password = "admin",
                    Roles =
                    [
                        superUserRole.Name
                    ]
                });

            await _usersService.CreateUserAsync(
                new Users.DTO.CreateUserDto
                {
                    Email = "staff.manager@vkusnuts.online",
                    Username = "mng-auth",
                    Password = "mng-auth",
                    Roles =
                    [
                        identityManagerRole.Name,
                    ]
                });
        }
        
        return Ok();
    }
}

/*
[

]
 
 */