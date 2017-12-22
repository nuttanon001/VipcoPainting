using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using VipcoPainting.Models;

namespace VipcoPainting.Data
{
    public static class DbInitializer
    {
        public static void Initialize(PaintingContext Context)
        {
            Context.Database.EnsureCreated();

            if (!Context.PaintTeams.Any())
            {
                var vipco = Context.PaintTeams
                    .Add(new PaintTeam { CreateDate = DateTime.Now, Creator = "Admin", TeamName = "VIPCO" }).Entity; ;
                var npt = Context.PaintTeams
                    .Add(new PaintTeam { CreateDate = DateTime.Now, Creator = "Admin", TeamName = "NPT" }).Entity; ;
                var bps = Context.PaintTeams
                    .Add(new PaintTeam { CreateDate = DateTime.Now, Creator = "Admin", TeamName = "BPS" }).Entity; ;

                Context.BlastRooms
                    .Add(new BlastRoom
                    {
                        BlastRoomName = "Room1",
                        BlastRoomNumber = 1,
                        CreateDate = DateTime.Now,
                        Creator = "Admin",
                        PaintTeam = vipco
                    });

                Context.BlastRooms
                   .Add(new BlastRoom
                   {
                       BlastRoomName = "Room2",
                       BlastRoomNumber = 2,
                       CreateDate = DateTime.Now,
                       Creator = "Admin",
                       PaintTeam = npt
                   });

                Context.BlastRooms
                   .Add(new BlastRoom
                   {
                       BlastRoomName = "Room3",
                       BlastRoomNumber = 3,
                       CreateDate = DateTime.Now,
                       Creator = "Admin",
                       PaintTeam = npt
                   });

                Context.BlastRooms
                   .Add(new BlastRoom
                   {
                       BlastRoomName = "Room4",
                       BlastRoomNumber = 4,
                       CreateDate = DateTime.Now,
                       Creator = "Admin",
                       PaintTeam = bps
                   });

                Context.BlastRooms
                   .Add(new BlastRoom
                   {
                       BlastRoomName = "Room5",
                       BlastRoomNumber = 5,
                       CreateDate = DateTime.Now,
                       Creator = "Admin",
                       PaintTeam = vipco
                   });

                Context.SaveChanges();
            }
        }
    }
}
