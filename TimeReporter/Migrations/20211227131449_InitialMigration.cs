using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeReporter.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Workers",
                columns: table => new
                {
                    WorkerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Timestamp = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workers", x => x.WorkerId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    ActivityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    WorkerId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Budget = table.Column<int>(type: "int", nullable: false),
                    Active = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.ActivityId);
                    table.ForeignKey(
                        name: "FK_Activities_Workers_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "Workers",
                        principalColumn: "WorkerId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    ReportId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Frozen = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    WorkerId = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.ReportId);
                    table.ForeignKey(
                        name: "FK_Reports_Workers_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "Workers",
                        principalColumn: "WorkerId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Subactivities",
                columns: table => new
                {
                    SubactivityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ActivityId = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subactivities", x => x.SubactivityId);
                    table.ForeignKey(
                        name: "FK_Subactivities_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "ActivityId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Accepteds",
                columns: table => new
                {
                    AcceptedId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ActivityId = table.Column<int>(type: "int", nullable: false),
                    Time = table.Column<int>(type: "int", nullable: false),
                    ReportId = table.Column<int>(type: "int", nullable: false),
                    WorkerId = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accepteds", x => x.AcceptedId);
                    table.ForeignKey(
                        name: "FK_Accepteds_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "ActivityId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Accepteds_Reports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Reports",
                        principalColumn: "ReportId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Accepteds_Workers_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "Workers",
                        principalColumn: "WorkerId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Entries",
                columns: table => new
                {
                    EntryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    ActivityId = table.Column<int>(type: "int", nullable: false),
                    SubactivityId = table.Column<int>(type: "int", nullable: true),
                    Time = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    WorkerId = table.Column<int>(type: "int", nullable: false),
                    ReportId = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entries", x => x.EntryId);
                    table.ForeignKey(
                        name: "FK_Entries_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "ActivityId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Entries_Reports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Reports",
                        principalColumn: "ReportId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Entries_Subactivities_SubactivityId",
                        column: x => x.SubactivityId,
                        principalTable: "Subactivities",
                        principalColumn: "SubactivityId");
                    table.ForeignKey(
                        name: "FK_Entries_Workers_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "Workers",
                        principalColumn: "WorkerId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Workers",
                columns: new[] { "WorkerId", "Name" },
                values: new object[,]
                {
                    { 1, "Clarkson" },
                    { 2, "Hammond" },
                    { 3, "May" },
                    { 4, "Plant" },
                    { 5, "Page" },
                    { 6, "Bonham" },
                    { 7, "Jones" },
                    { 8, "Hetfield" },
                    { 9, "Hammett" },
                    { 10, "Ulrich" },
                    { 11, "Trujillo" }
                });

            migrationBuilder.InsertData(
                table: "Activities",
                columns: new[] { "ActivityId", "Active", "Budget", "Code", "Name", "WorkerId" },
                values: new object[,]
                {
                    { 1, false, 115, "Mercury-1", "Mercury", 1 },
                    { 2, true, 50, "Jupiter-2", "Jupiter", 2 },
                    { 3, true, 100, "Pluto-1", "Pluto", 3 },
                    { 4, true, 100, "Saturn-5", "Saturn", 4 },
                    { 5, true, -10, "Venus-3", "Venus", 5 },
                    { 6, true, 70, "Uranus-2", "Uranus", 6 },
                    { 7, true, -1, "OTHER", "Other", 7 },
                    { 8, false, 150, "Neptune-7", "Neptune", 8 },
                    { 9, true, 150, "Luna-1", "Luna", 9 },
                    { 10, true, 300, "Europa-77", "Europa", 10 }
                });

            migrationBuilder.InsertData(
                table: "Reports",
                columns: new[] { "ReportId", "Date", "Frozen", "WorkerId" },
                values: new object[,]
                {
                    { 1, new DateTime(2021, 12, 27, 0, 0, 0, 0, DateTimeKind.Local), true, 1 },
                    { 2, new DateTime(2021, 12, 27, 0, 0, 0, 0, DateTimeKind.Local), false, 2 },
                    { 3, new DateTime(2021, 12, 27, 0, 0, 0, 0, DateTimeKind.Local), false, 3 },
                    { 4, new DateTime(2021, 12, 27, 0, 0, 0, 0, DateTimeKind.Local), true, 4 },
                    { 5, new DateTime(2021, 12, 27, 0, 0, 0, 0, DateTimeKind.Local), false, 5 },
                    { 6, new DateTime(2021, 12, 27, 0, 0, 0, 0, DateTimeKind.Local), false, 6 },
                    { 7, new DateTime(2021, 12, 27, 0, 0, 0, 0, DateTimeKind.Local), false, 7 },
                    { 8, new DateTime(2021, 12, 27, 0, 0, 0, 0, DateTimeKind.Local), true, 8 },
                    { 9, new DateTime(2021, 12, 27, 0, 0, 0, 0, DateTimeKind.Local), false, 9 },
                    { 10, new DateTime(2021, 12, 27, 0, 0, 0, 0, DateTimeKind.Local), false, 10 }
                });

            migrationBuilder.InsertData(
                table: "Accepteds",
                columns: new[] { "AcceptedId", "ActivityId", "ReportId", "Time", "WorkerId" },
                values: new object[,]
                {
                    { 1, 2, 1, 50, 1 },
                    { 2, 7, 1, 110, 1 },
                    { 3, 3, 1, 25, 1 },
                    { 4, 2, 4, 70, 4 },
                    { 5, 8, 4, 100, 4 },
                    { 6, 7, 4, 30, 4 },
                    { 7, 5, 4, 30, 4 },
                    { 8, 3, 8, 70, 8 },
                    { 9, 1, 8, 100, 8 }
                });

            migrationBuilder.InsertData(
                table: "Entries",
                columns: new[] { "EntryId", "ActivityId", "Date", "Description", "ReportId", "SubactivityId", "Time", "WorkerId" },
                values: new object[,]
                {
                    { 2, 7, new DateTime(2021, 12, 27, 0, 0, 0, 0, DateTimeKind.Local), "tea time", 1, null, 150, 1 },
                    { 6, 7, new DateTime(2021, 12, 27, 0, 0, 0, 0, DateTimeKind.Local), "resting", 4, null, 30, 4 },
                    { 11, 8, new DateTime(2021, 12, 27, 0, 0, 0, 0, DateTimeKind.Local), "tea time", 10, null, 15, 10 },
                    { 12, 2, new DateTime(2021, 12, 27, 0, 0, 0, 0, DateTimeKind.Local), "celebrating", 10, null, 30, 10 },
                    { 15, 9, new DateTime(2021, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "tea time", 10, null, 25, 10 }
                });

            migrationBuilder.InsertData(
                table: "Subactivities",
                columns: new[] { "SubactivityId", "ActivityId", "Code" },
                values: new object[,]
                {
                    { 1, 1, "database" },
                    { 2, 1, "frontend" },
                    { 3, 1, "backend" },
                    { 4, 2, "database" },
                    { 5, 2, "frontend" },
                    { 6, 2, "backend" },
                    { 7, 3, "database" },
                    { 8, 3, "frontend" },
                    { 9, 3, "backend" },
                    { 10, 4, "database" },
                    { 11, 4, "frontend" },
                    { 12, 4, "backend" },
                    { 13, 5, "database" },
                    { 14, 5, "frontend" },
                    { 15, 5, "backend" },
                    { 16, 6, "database" },
                    { 17, 6, "frontend" },
                    { 18, 6, "backend" },
                    { 22, 8, "database" },
                    { 23, 8, "frontend" },
                    { 24, 8, "backend" },
                    { 25, 9, "database" },
                    { 26, 9, "frontend" },
                    { 27, 9, "backend" },
                    { 28, 10, "database" },
                    { 29, 10, "frontend" },
                    { 30, 10, "backend" }
                });

            migrationBuilder.InsertData(
                table: "Entries",
                columns: new[] { "EntryId", "ActivityId", "Date", "Description", "ReportId", "SubactivityId", "Time", "WorkerId" },
                values: new object[,]
                {
                    { 1, 2, new DateTime(2021, 12, 27, 0, 0, 0, 0, DateTimeKind.Local), "created view", 1, 5, 70, 1 },
                    { 3, 3, new DateTime(2021, 12, 27, 0, 0, 0, 0, DateTimeKind.Local), "edited model", 1, 9, 30, 1 },
                    { 4, 2, new DateTime(2021, 12, 27, 0, 0, 0, 0, DateTimeKind.Local), "edited row", 4, 4, 70, 4 },
                    { 5, 8, new DateTime(2021, 12, 27, 0, 0, 0, 0, DateTimeKind.Local), "added button", 4, 23, 150, 4 },
                    { 7, 5, new DateTime(2021, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "modified controller", 4, 15, 30, 4 },
                    { 8, 5, new DateTime(2021, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "created view", 4, 14, 150, 4 },
                    { 9, 3, new DateTime(2021, 12, 27, 0, 0, 0, 0, DateTimeKind.Local), "created table", 8, 7, 70, 8 },
                    { 10, 1, new DateTime(2021, 12, 27, 0, 0, 0, 0, DateTimeKind.Local), "modified view", 8, 2, 120, 8 },
                    { 13, 9, new DateTime(2021, 12, 27, 0, 0, 0, 0, DateTimeKind.Local), "added view", 10, 26, 130, 10 },
                    { 14, 1, new DateTime(2021, 12, 27, 0, 0, 0, 0, DateTimeKind.Local), "modified view", 10, 2, 80, 10 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accepteds_ActivityId",
                table: "Accepteds",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_Accepteds_ReportId",
                table: "Accepteds",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_Accepteds_WorkerId",
                table: "Accepteds",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_WorkerId",
                table: "Activities",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_Entries_ActivityId",
                table: "Entries",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_Entries_ReportId",
                table: "Entries",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_Entries_SubactivityId",
                table: "Entries",
                column: "SubactivityId");

            migrationBuilder.CreateIndex(
                name: "IX_Entries_WorkerId",
                table: "Entries",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_WorkerId",
                table: "Reports",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_Subactivities_ActivityId",
                table: "Subactivities",
                column: "ActivityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accepteds");

            migrationBuilder.DropTable(
                name: "Entries");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "Subactivities");

            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.DropTable(
                name: "Workers");
        }
    }
}
