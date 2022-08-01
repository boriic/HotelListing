using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelListingAPI.Migrations
{
    public partial class AddedDefaultRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "393c62b0-d08f-48e9-b61b-6b24b9165c41", "6288116b-0a01-4530-a752-f96a968166b9", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "d1dc92a4-6d48-4711-b276-adbbff2d5abc", "a6aa937c-edd3-42e2-9afa-687529d4b79b", "Admin", "ADMIN" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "393c62b0-d08f-48e9-b61b-6b24b9165c41");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d1dc92a4-6d48-4711-b276-adbbff2d5abc");
        }
    }
}
