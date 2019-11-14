using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Potestas.Migrations
{
    public partial class CreateDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CoordinatesWrapper",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    X = table.Column<double>(nullable: false),
                    Y = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoordinatesWrapper", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FlashObservationWrapper",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Intensity = table.Column<double>(nullable: false),
                    DurationMs = table.Column<int>(nullable: false),
                    ObservationTime = table.Column<DateTime>(nullable: false),
                    EstimatedValue = table.Column<double>(nullable: false),
                    CoordinatesId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlashObservationWrapper", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlashObservationWrapper_CoordinatesWrapper_CoordinatesId",
                        column: x => x.CoordinatesId,
                        principalTable: "CoordinatesWrapper",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FlashObservationWrapper_CoordinatesId",
                table: "FlashObservationWrapper",
                column: "CoordinatesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FlashObservationWrapper");

            migrationBuilder.DropTable(
                name: "CoordinatesWrapper");
        }
    }
}
