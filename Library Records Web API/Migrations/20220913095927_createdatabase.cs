using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Library_Records_Web_API.Migrations
{
    public partial class createdatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberId = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    MemberName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    RNPost = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ClassDepartment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RecordNos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecordNos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Book",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BookName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Author = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    TotalCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Book", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Book_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SecurityQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Question = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecurityQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SecurityQuestions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Records",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecordId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    BorrowDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BorrowSignature = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReturnDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReturnSignature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateExtended = table.Column<int>(type: "int", nullable: false),
                    DExtendedSignature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Records", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Records_Book_BookId",
                        column: x => x.BookId,
                        principalTable: "Book",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Records_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Records_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Book_CategoryId",
                table: "Book",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Records_BookId",
                table: "Records",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Records_MemberId",
                table: "Records",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Records_UserId",
                table: "Records",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SecurityQuestions_UserId",
                table: "SecurityQuestions",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecordNos");

            migrationBuilder.DropTable(
                name: "Records");

            migrationBuilder.DropTable(
                name: "SecurityQuestions");

            migrationBuilder.DropTable(
                name: "Book");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
