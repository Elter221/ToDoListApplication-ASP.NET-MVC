using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoApplicationMVC.Migrations
{
    /// <inheritdoc />
    public partial class AddTagClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TagId",
                table: "ToDos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TagName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ToDos_TagId",
                table: "ToDos",
                column: "TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDos_Tags_TagId",
                table: "ToDos",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDos_Tags_TagId",
                table: "ToDos");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_ToDos_TagId",
                table: "ToDos");

            migrationBuilder.DropColumn(
                name: "TagId",
                table: "ToDos");
        }
    }
}
