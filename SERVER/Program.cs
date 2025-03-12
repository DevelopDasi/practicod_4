using TodoApi; 
using Microsoft.EntityFrameworkCore; 
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Todo API", Version = "v1" });
});
builder.Services.AddCors(option => option.AddPolicy("AllowAll",
    p => p.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()));
builder.Services.AddDbContext<ToDoDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("ToDoDb"),
    new MySqlServerVersion(new Version(8, 0, 41))));

var app = builder.Build();
app.UseCors("AllowAll");
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapGet("/getAll", async (ToDoDbContext context) => 
    await context.Items.ToListAsync());
app.MapPost("/addItem", async (ToDoDbContext context, string name) => {
    var item = new Item { Name =  name, IsComplete = false };
    context.Items.Add(item);
    await context.SaveChangesAsync();
});
app.MapPut("/updateItem/{id}", async (ToDoDbContext context, int id) => {
    var itemToUpdate = await context.Items.FindAsync(id);
    if (itemToUpdate == null)
    {
        return Results.NotFound();
    }
    itemToUpdate.IsComplete = itemToUpdate.IsComplete == true ? false : true;
    await context.SaveChangesAsync();
    return Results.NoContent();
});    
app.MapDelete("/deleteItem/{id}", async (ToDoDbContext context, int id) => {
    var item = await context.Items.FindAsync(id);
    if (item == null)
    {
        return Results.NotFound();
    }
    context.Items.Remove(item);
    await context.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();