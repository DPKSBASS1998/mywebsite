using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mywebsite.Migrations
{
    /// <inheritdoc />
    public partial class ProductMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Barebones",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Layout = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RGB = table.Column<bool>(type: "bit", nullable: false),
                    HotSwap = table.Column<bool>(type: "bit", nullable: false),
                    Connection = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CaseMaterial = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Barebones", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Barebones_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Keyboards",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Layout = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RGB = table.Column<bool>(type: "bit", nullable: false),
                    HotSwap = table.Column<bool>(type: "bit", nullable: false),
                    Connection = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KeycapsProfile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CaseMaterial = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SwitchType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Keyboards", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Keyboards_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Keycaps",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Profile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Material = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sublegends = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ManufacturingProcess = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LayoutStandard = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Keycaps", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Keycaps_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Switches",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OperatingForce = table.Column<int>(type: "int", nullable: false),
                    TotalTravel = table.Column<int>(type: "int", nullable: false),
                    PreTravel = table.Column<int>(type: "int", nullable: false),
                    TactilePosition = table.Column<int>(type: "int", nullable: false),
                    TactileForce = table.Column<int>(type: "int", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Switches", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Switches_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Barebones");

            migrationBuilder.DropTable(
                name: "Keyboards");

            migrationBuilder.DropTable(
                name: "Keycaps");

            migrationBuilder.DropTable(
                name: "Switches");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
