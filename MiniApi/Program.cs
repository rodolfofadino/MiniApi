
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PessoaDbContext>(options => options.UseInMemoryDatabase("Pessoas"));

await using var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.MapGet("/pessoas", async (PessoaDbContext dbContext) => await dbContext.Pessoas.ToListAsync());

app.MapGet("/pessoas/{id}", async (int id, PessoaDbContext dbContext) => await dbContext.Pessoas.FirstOrDefaultAsync(a => a.Id == id));

app.MapPost("/pessoas", async (Pessoa pessoa, PessoaDbContext dbContext) =>
{
    dbContext.Pessoas.Add(pessoa);
    await dbContext.SaveChangesAsync();

    return pessoa;
});

app.MapPut("/pessoas/{id}", async (int id, Pessoa pessoa, PessoaDbContext dbContext) =>
{
    dbContext.Entry(pessoa).State = EntityState.Modified;
    await dbContext.SaveChangesAsync();
    return pessoa;
});

app.MapDelete("/pessoas/{id}", async (int id, PessoaDbContext dbContext) =>
{
    var pessoa = await dbContext.Pessoas.FirstOrDefaultAsync(a => a.Id == id);

    dbContext.Pessoas.Remove(pessoa);
    await dbContext.SaveChangesAsync();
    return;
});

app.Run();



public class Pessoa
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Sobrenome { get; set; }
}

public class PessoaDbContext : DbContext
{
    public PessoaDbContext(DbContextOptions options) : base(options)
    {
    }
    public DbSet<Pessoa> Pessoas { get; set; }
}
