using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ecology.Data.Migrations
{
    /// <inheritdoc />
    public partial class Notification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Text = table.Column<string>(type: "text", nullable: false),
                    Start = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    End = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotificationDataUserData",
                columns: table => new
                {
                    NotificationsWhichIAlreadySawId = table.Column<int>(type: "integer", nullable: false),
                    UsersWhoAlreadySawItId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationDataUserData", x => new { x.NotificationsWhichIAlreadySawId, x.UsersWhoAlreadySawItId });
                    table.ForeignKey(
                        name: "FK_NotificationDataUserData_Notifications_NotificationsWhichIA~",
                        column: x => x.NotificationsWhichIAlreadySawId,
                        principalTable: "Notifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotificationDataUserData_Users_UsersWhoAlreadySawItId",
                        column: x => x.UsersWhoAlreadySawItId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationDataUserData_UsersWhoAlreadySawItId",
                table: "NotificationDataUserData",
                column: "UsersWhoAlreadySawItId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotificationDataUserData");

            migrationBuilder.DropTable(
                name: "Notifications");
        }
    }
}
