using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankAccountWebApi.Migrations
{
    public partial class InitialEventSourcing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SerializedEventPayload",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AggregateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderNumber = table.Column<long>(type: "bigint", nullable: false),
                    EventExecutor = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StateMachineId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SerializedJsonData = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SerializedEventPayload", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SerializedPayloadMessage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SerializedPayloadMessageData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SerializedPayloadMessage", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SerializedEventPayload");

            migrationBuilder.DropTable(
                name: "SerializedPayloadMessage");
        }
    }
}
