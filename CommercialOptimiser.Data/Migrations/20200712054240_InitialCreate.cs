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
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Demographic", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UniqueUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BreakDemographic",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                        .Annotation("SqlServer:Identity", "1, 1"),
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

            migrationBuilder.CreateTable(
                name: "UserReportBreak",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BreakTitle = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserReportBreak", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserReportBreak_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserReportBreakCommercial",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommercialTitle = table.Column<string>(nullable: true),
                    Rating = table.Column<int>(nullable: false),
                    UserReportBreakId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserReportBreakCommercial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserReportBreakCommercial_UserReportBreak_UserReportBreakId",
                        column: x => x.UserReportBreakId,
                        principalTable: "UserReportBreak",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateIndex(
                name: "IX_UserReportBreak_UserId",
                table: "UserReportBreak",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserReportBreakCommercial_UserReportBreakId",
                table: "UserReportBreakCommercial",
                column: "UserReportBreakId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BreakDemographic");

            migrationBuilder.DropTable(
                name: "Commercial");

            migrationBuilder.DropTable(
                name: "UserReportBreakCommercial");

            migrationBuilder.DropTable(
                name: "Break");

            migrationBuilder.DropTable(
                name: "Demographic");

            migrationBuilder.DropTable(
                name: "UserReportBreak");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
