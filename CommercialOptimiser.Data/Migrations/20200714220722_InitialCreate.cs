using Microsoft.EntityFrameworkCore.Migrations;

namespace CommercialOptimiser.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Break",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Capacity = table.Column<int>(nullable: false),
                    InvalidCommercialTypes = table.Column<string>(nullable: true),
                    Title = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Break", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Demographic",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Demographic", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserReportBreak",
                columns: table => new
                {
                    UserUniqueId = table.Column<string>(nullable: false),
                    Report = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserReportBreak", x => x.UserUniqueId);
                });

            migrationBuilder.CreateTable(
                name: "BreakDemographic",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BreakId = table.Column<int>(nullable: true),
                    DemographicId = table.Column<int>(nullable: true),
                    Rating = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BreakDemographic", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BreakDemographic_Break_BreakId",
                        column: x => x.BreakId,
                        principalTable: "Break",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BreakDemographic_Demographic_DemographicId",
                        column: x => x.DemographicId,
                        principalTable: "Demographic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Commercial",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CommercialType = table.Column<string>(maxLength: 50, nullable: true),
                    DemographicId = table.Column<int>(nullable: true),
                    Title = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commercial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Commercial_Demographic_DemographicId",
                        column: x => x.DemographicId,
                        principalTable: "Demographic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BreakDemographic_BreakId",
                table: "BreakDemographic",
                column: "BreakId");

            migrationBuilder.CreateIndex(
                name: "IX_BreakDemographic_DemographicId",
                table: "BreakDemographic",
                column: "DemographicId");

            migrationBuilder.CreateIndex(
                name: "IX_Commercial_DemographicId",
                table: "Commercial",
                column: "DemographicId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BreakDemographic");

            migrationBuilder.DropTable(
                name: "Commercial");

            migrationBuilder.DropTable(
                name: "UserReportBreak");

            migrationBuilder.DropTable(
                name: "Break");

            migrationBuilder.DropTable(
                name: "Demographic");
        }
    }
}
