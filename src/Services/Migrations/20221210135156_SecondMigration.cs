using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.Migrations
{
    public partial class SecondMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Inquiries");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Offers",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Offers");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Inquiries",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PersonalData_FirstName = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    PersonalData_GovernmentId = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    PersonalData_GovernmentIdType = table.Column<int>(type: "integer", nullable: false),
                    PersonalData_JobType = table.Column<int>(type: "integer", nullable: false),
                    PersonalData_LastName = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }
    }
}
