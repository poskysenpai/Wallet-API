using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WalletAPI.Migrations
{
    /// <inheritdoc />
    public partial class SeedRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                    INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp) 
                    VALUES ('1', 'noob', 'NOOB', '40EDA8B1-7D6A-4396-B855-78480CE120AD'),  
                           ('2', 'admin', 'ADMIN', 'C0D5CFC5-6D37-4E08-BCCF-3C459FCAADD6'), 
                           ('3', 'elite', 'ELITE', 'C0D5CFC5-6D37-4E08-BCCF-3C459FCAArr6')
            
            ");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
