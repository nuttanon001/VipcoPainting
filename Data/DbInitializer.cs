using System;
using System.Collections.Generic;
using System.Linq;

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

            if (!Context.MovementStockStatuses.Any())
            {
                Context.MovementStockStatuses
                  .Add(new MovementStockStatus
                  {
                      CreateDate = DateTime.Now,
                      Creator = "Admin",
                      StatusMovement = StatusMovement.Stock,
                      StatusName = "Stock",
                      TypeStatusMovement = TypeStatusMovement.Increased
                  });

                Context.MovementStockStatuses
                  .Add(new MovementStockStatus
                  {
                      CreateDate = DateTime.Now,
                      Creator = "Admin",
                      StatusMovement = StatusMovement.Requsition,
                      StatusName = "Requsition",
                      TypeStatusMovement = TypeStatusMovement.Decreased
                  });

                Context.MovementStockStatuses
                  .Add(new MovementStockStatus
                  {
                      CreateDate = DateTime.Now,
                      Creator = "Admin",
                      StatusMovement = StatusMovement.AdjustIncreased,
                      StatusName = "AdjustIncreased",
                      TypeStatusMovement = TypeStatusMovement.Increased
                  });

                Context.MovementStockStatuses
                  .Add(new MovementStockStatus
                  {
                      CreateDate = DateTime.Now,
                      Creator = "Admin",
                      StatusMovement = StatusMovement.AdjustDecreased,
                      StatusName = "AdjustDecreased",
                      TypeStatusMovement = TypeStatusMovement.Decreased
                  });

                Context.MovementStockStatuses
                  .Add(new MovementStockStatus
                  {
                      CreateDate = DateTime.Now,
                      Creator = "Admin",
                      StatusMovement = StatusMovement.Cancel,
                      StatusName = "Cancel",
                      TypeStatusMovement = TypeStatusMovement.Decreased
                  });

                Context.SaveChanges();
            }

            if (!Context.PaymentDetails.Any())
            {
                var TempPayment = new string[]
                {
                    "งาน Blasting only SA 2 or SSPC-SP6 (Raw material )",
                    "งาน Blasting only SA 2.5 or SSPC-SP10 (Raw material)",
                    "งาน Blasting only SA 3 or SSPC-SP5  (Raw material)",
                    "งานอะไหล่ SA 2 - Accessoies Part (พื้นที่น้อยกว่า 5 m2)",
                    "งานอะไหล่ SA 2.5 - Accessoies Part (พื้นที่น้อยกว่า 5 m2)",
                    "งานอะไหล่ SA 3 - Accessoies Part (พื้นที่น้อยกว่า 5 m2)",
                    "งาน Blasting only sa.2 or SSPC-SP6",
                    "งาน Blasting only sa.2.5 or SSPC-SP10",
                    "งาน Blasting only sa.3 or SSPC-SP5",
                    "งาน Light Blast Cleaning Sa.1 or SSPC-SP7",
                    "งาน Power tool Cleaning SSPC-SP3",
                    "งานพ่นสี (Cost)"
                };

                var TempCost = new double[] { 90, 100, 110, 130, 140, 140, 70, 80, 90, 50, 20, 30 };

                for (int i = 0; i < TempPayment.Length; i++)
                {
                    Context.PaymentDetails
                        .Add(new PaymentDetail
                        {
                            CreateDate = DateTime.Now,
                            Creator = "Admin",
                            Description = TempPayment[i],
                            LastCost = TempCost[i],
                            PaymentType = TempPayment[i].Contains("พ่นสี") ? PaymentType.Paint : PaymentType.Blast,
                            PaymentCostHistorys = new List<PaymentCostHistory>() { new PaymentCostHistory
                            {
                                CreateDate = DateTime.Now,
                                Creator = "Admin",
                                StartDate = DateTime.Now,
                                PaymentCost = TempCost[i]
                            }}
                        });
                }

                Context.SaveChanges();
            }
        }
    }
}