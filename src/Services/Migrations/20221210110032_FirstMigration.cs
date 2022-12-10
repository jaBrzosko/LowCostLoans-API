using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.Migrations
{
    public partial class FirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Examples",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Examples", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Inquiries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    PersonalData_FirstName = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    PersonalData_LastName = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    PersonalData_GovernmentId = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    PersonalData_GovernmentIdType = table.Column<int>(type: "integer", nullable: true),
                    PersonalData_JobType = table.Column<int>(type: "integer", nullable: true),
                    MoneyInSmallestUnit = table.Column<int>(type: "integer", nullable: false),
                    NumberOfInstallments = table.Column<int>(type: "integer", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inquiries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Offers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InquireId = table.Column<Guid>(type: "uuid", nullable: false),
                    InterestRate = table.Column<int>(type: "integer", nullable: false),
                    MoneyInSmallestUnit = table.Column<int>(type: "integer", nullable: false),
                    NumberOfInstallments = table.Column<int>(type: "integer", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OfferTemplates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MinimumMoneyInSmallestUnit = table.Column<int>(type: "integer", nullable: false),
                    MaximumMoneyInSmallestUnit = table.Column<int>(type: "integer", nullable: false),
                    MinimumNumberOfInstallments = table.Column<int>(type: "integer", nullable: false),
                    MaximumNumberOfInstallments = table.Column<int>(type: "integer", nullable: false),
                    InterestRate = table.Column<int>(type: "integer", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfferTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PersonalData_FirstName = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    PersonalData_LastName = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    PersonalData_GovernmentId = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    PersonalData_GovernmentIdType = table.Column<int>(type: "integer", nullable: false),
                    PersonalData_JobType = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Examples");

            migrationBuilder.DropTable(
                name: "Inquiries");

            migrationBuilder.DropTable(
                name: "Offers");

            migrationBuilder.DropTable(
                name: "OfferTemplates");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
