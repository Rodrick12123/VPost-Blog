using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.Migrations.AuthDb
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0b275ad8-842d-4423-ba91-a27ebbc0f3e7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "219c3c50-c0f3-443d-b78e-14eee1a87e84", "AQAAAAIAAYagAAAAECgl5wkGrC/aIDn66QW75cz0lN72i2oJnVnk+fVNMc+99eWc3NZqxkkIE/XrsNWBkQ==", "18ffde5c-2f3d-4931-9c02-95d52d77296b" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0b275ad8-842d-4423-ba91-a27ebbc0f3e7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "bb1af0c1-ff92-4f2c-b709-64ff378bc7cc", "AQAAAAIAAYagAAAAEOBj7XCHkTCcM28NfMg7JsDWT9C8kUKl4gfo37NMcAN/eTMWffwZ/mY0NI0P2I2mHA==", "683455a5-dd46-4816-ac4c-7694264d1d22" });
        }
    }
}
