using Microsoft.EntityFrameworkCore;

namespace P2_Apli1_Lohammy.DAL;

public class Contexto : DbContext
{
    public Contexto(DbContextOptions<Contexto> options) : base(options) { }

}
