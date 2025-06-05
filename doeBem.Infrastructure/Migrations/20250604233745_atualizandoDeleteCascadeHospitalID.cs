using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace doeBem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class atualizandoDeleteCascadeHospitalID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Donations_Hospitals_HospitalId",
                table: "Donations");

            migrationBuilder.AlterColumn<Guid>(
                name: "HospitalId",
                table: "Donations",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Donations_Hospitals_HospitalId",
                table: "Donations",
                column: "HospitalId",
                principalTable: "Hospitals",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Donations_Hospitals_HospitalId",
                table: "Donations");

            migrationBuilder.AlterColumn<Guid>(
                name: "HospitalId",
                table: "Donations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Donations_Hospitals_HospitalId",
                table: "Donations",
                column: "HospitalId",
                principalTable: "Hospitals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
