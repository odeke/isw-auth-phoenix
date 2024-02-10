using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PhoenixAuth.BusinessLogic.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PartnerAuths",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsActive = table.Column<bool>(nullable: false),
                    IssueDate = table.Column<DateTime>(nullable: false),
                    ClientId = table.Column<string>(nullable: true),
                    AuthToken = table.Column<string>(nullable: true),
                    ExpireTime = table.Column<DateTime>(nullable: false),
                    ServerSessionPublicKey = table.Column<string>(nullable: true),
                    TransactionReference = table.Column<string>(nullable: true),
                    EcdhPublicKey = table.Column<string>(nullable: true),
                    EcdhPrivateKey = table.Column<string>(nullable: true),
                    Service = table.Column<string>(nullable: true),
                    EncryptionType = table.Column<string>(nullable: true),
                    RsaPublicKey = table.Column<string>(nullable: true),
                    RsaPrivateKey = table.Column<string>(nullable: true),
                    Environment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartnerAuths", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PartnerAuths");
        }
    }
}
