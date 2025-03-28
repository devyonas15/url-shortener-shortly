using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Url",
                columns: table => new
                {
                    UrlId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShortUrl = table.Column<string>(type: "nvarchar(27)", nullable: false),
                    LongUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Url", x => x.UrlId);
                });

            migrationBuilder.CreateTable(
                name: "UrlMetric",
                columns: table => new
                {
                    UrlMetricId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UrlId = table.Column<int>(type: "int", nullable: false),
                    AccessorIp = table.Column<string>(type: "nvarchar(45)", nullable: false),
                    UserAgent = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrlMetric", x => x.UrlMetricId);
                    table.ForeignKey(
                        name: "FK_UrlMetric_Url_UrlId",
                        column: x => x.UrlId,
                        principalTable: "Url",
                        principalColumn: "UrlId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UrlMetric_UrlId",
                table: "UrlMetric",
                column: "UrlId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UrlMetric");

            migrationBuilder.DropTable(
                name: "Url");
        }
    }
}
