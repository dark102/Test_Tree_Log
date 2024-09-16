using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Test_Tree_Log.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExceptionData",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    exceptionType = table.Column<string>(type: "text", nullable: false),
                    exceptionMessage = table.Column<string>(type: "text", nullable: false),
                    stackTracert = table.Column<string>(type: "text", nullable: false),
                    inerExceptionDataid = table.Column<int>(type: "integer", nullable: true),
                    updatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    createdDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExceptionData", x => x.id);
                    table.ForeignKey(
                        name: "FK_ExceptionData_ExceptionData_inerExceptionDataid",
                        column: x => x.inerExceptionDataid,
                        principalTable: "ExceptionData",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "tree",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    updatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    createdDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tree", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "exceptionJournal",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    eventID = table.Column<string>(type: "text", nullable: false),
                    pathRequest = table.Column<string>(type: "text", nullable: false),
                    paramRequest = table.Column<string>(type: "text", nullable: true),
                    bodyRequest = table.Column<string>(type: "text", nullable: true),
                    exceptionDataid = table.Column<int>(type: "integer", nullable: false),
                    updatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    createdDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exceptionJournal", x => x.id);
                    table.ForeignKey(
                        name: "FK_exceptionJournal_ExceptionData_exceptionDataid",
                        column: x => x.exceptionDataid,
                        principalTable: "ExceptionData",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "child",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    treeid = table.Column<int>(type: "integer", nullable: false),
                    Childid = table.Column<int>(type: "integer", nullable: true),
                    updatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    createdDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_child", x => x.id);
                    table.ForeignKey(
                        name: "FK_child_child_Childid",
                        column: x => x.Childid,
                        principalTable: "child",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_child_tree_treeid",
                        column: x => x.treeid,
                        principalTable: "tree",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_child_Childid",
                table: "child",
                column: "Childid");

            migrationBuilder.CreateIndex(
                name: "IX_child_name",
                table: "child",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_child_treeid",
                table: "child",
                column: "treeid");

            migrationBuilder.CreateIndex(
                name: "IX_ExceptionData_inerExceptionDataid",
                table: "ExceptionData",
                column: "inerExceptionDataid");

            migrationBuilder.CreateIndex(
                name: "IX_exceptionJournal_exceptionDataid",
                table: "exceptionJournal",
                column: "exceptionDataid");

            migrationBuilder.CreateIndex(
                name: "IX_tree_name",
                table: "tree",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "child");

            migrationBuilder.DropTable(
                name: "exceptionJournal");

            migrationBuilder.DropTable(
                name: "tree");

            migrationBuilder.DropTable(
                name: "ExceptionData");
        }
    }
}
