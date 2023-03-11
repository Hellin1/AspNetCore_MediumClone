using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MediumClone.DataAccess.Migrations
{
    public partial class AppRoleSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "CreatedTime", "Name", "NormalizedName" },
                values: new object[] { 1, "f1f8c11e-9cc5-4d0f-9322-36421f2ece49", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Member", "MEMBER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
