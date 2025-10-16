using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoApplicationMVC.Migrations
{
    /// <inheritdoc />
    public partial class AddToDoTagsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TagToDos",
                columns: table => new
                {
                    TagsId = table.Column<int>(type: "int", nullable: false),
                    ToDoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagToDos", x => new { x.TagsId, x.ToDoId });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TagToDos");
        }
    }
}
