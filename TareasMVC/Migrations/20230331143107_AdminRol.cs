using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TareasMVC.Migrations
{
    /// <inheritdoc />
    public partial class AdminRol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"IF NOT EXISTS(SELECT Id FROM AspNetRoles where Id = 'e2a8ca17-690f-453f-8a49-cf2f64621cd0')
                                BEGIN
                                    INSERT INTO AspNetRoles(Id,Name, NormalizedName)
                                    VALUES ('e2a8ca17-690f-453f-8a49-cf2f64621cd0','admin','ADMIN')
                                END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE AspNetRoles where Id = 'e2a8ca17-690f-453f-8a49-cf2f64621cd0'");
        }
    }
}
