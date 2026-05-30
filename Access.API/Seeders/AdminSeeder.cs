using Access.API.Data;


namespace Access.API.Seeders
{
    public class AdminSeeder
    {
        private readonly AppDbContext _context;

        public AdminSeeder(AppDbContext context){
            _context = context;
        }
    }
}
