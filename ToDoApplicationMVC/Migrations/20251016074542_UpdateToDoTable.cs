using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoApplicationMVC.Migrations
{
    /// <inheritdoc />
    public partial class UpdateToDoTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDos_Tags_TagId",
                table: "ToDos");

            migrationBuilder.DropIndex(
                name: "IX_ToDos_TagId",
                table: "ToDos");

            migrationBuilder.DropColumn(
                name: "TagId",
                table: "ToDos");

            migrationBuilder.CreateTable(
                name: "TagToDo",
                columns: table => new
                {
                    TagsId = table.Column<int>(type: "int", nullable: false),
                    ToDoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagToDo", x => new { x.TagsId, x.ToDoId });
                    table.ForeignKey(
                        name: "FK_TagToDo_Tags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TagToDo_ToDos_ToDoId",
                        column: x => x.ToDoId,
                        principalTable: "ToDos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TagToDo_ToDoId",
                table: "TagToDo",
                column: "ToDoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TagToDo");

            migrationBuilder.AddColumn<int>(
                name: "TagId",
                table: "ToDos",
                type: "int",
                nullable: true);

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
    }
}
