using AlexandriasLibrary.Components;
using AlexandriasLibrary.Services;
using MongoDB.Driver;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddHttpClient<SteamService>();
builder.Services.AddSingleton<IMongoClient>(_ =>
    new MongoClient(builder.Configuration["Mongo:ConnectionString"]));
builder.Services.AddHttpClient<SteamSpyService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

//TODO: find user code --DONE--
//TODO: Use code to find user data --DONE--
//TODO: Display Game info
    //Game name --DONE--
    //Game achievement completion --DONE--
    //Game tags (Top tags?) --DONE (ish)--
    //Time played --DONE--
//TODO: Allow list, grid, customize view --DONE (Could use more customization)
