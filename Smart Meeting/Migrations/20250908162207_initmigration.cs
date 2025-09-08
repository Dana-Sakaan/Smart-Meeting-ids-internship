using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Smart_Meeting.Migrations
{
    /// <inheritdoc />
    public partial class initmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3dd3feff-d7c1-4850-8b7d-a24e9e702db5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b9de1d51-aa57-47fe-ba86-abc1eb56a7dd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cb92dfcd-e857-42ce-a950-590b82e76c82");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "9de75c33-d895-476a-bf71-ad68e353d152", null, "Admin", "ADMIN" },
                    { "c8fb0168-6481-4fbc-8b7d-3fea24a9854a", null, "User", "USER" },
                    { "fbfdb894-5c70-475d-8db5-ea373257e965", null, "Employee", "EMPLOYEE" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9de75c33-d895-476a-bf71-ad68e353d152");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c8fb0168-6481-4fbc-8b7d-3fea24a9854a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fbfdb894-5c70-475d-8db5-ea373257e965");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3dd3feff-d7c1-4850-8b7d-a24e9e702db5", null, "Admin", "ADMIN" },
                    { "b9de1d51-aa57-47fe-ba86-abc1eb56a7dd", null, "Employee", "EMPLOYEE" },
                    { "cb92dfcd-e857-42ce-a950-590b82e76c82", null, "User", "USER" }
                });
        }
    }
}
