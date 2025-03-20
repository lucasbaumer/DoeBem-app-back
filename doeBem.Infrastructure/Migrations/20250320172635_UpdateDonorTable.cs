using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace doeBem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDonorTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Donors",
                newName: "PasswordCript");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PasswordCript",
                table: "Donors",
                newName: "PasswordHash");
        }
    }
}
