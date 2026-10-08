var builder = WebApplication.CreateBuilder(args);

// On Windows 10 (Production), listen on port 5000 across the network.
// On Windows 11 (Development), let Visual Studio manage the ports normally.
if (!builder.Environment.IsDevelopment())
{
    builder.WebHost.ConfigureKestrel(serverOptions =>
    {
        serverOptions.ListenAnyIP(5000);
    });
}
// Add services to the container.
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AddPageRoute("/WhatsNew", "whatsnew.html");
    options.Conventions.AddPageRoute("/Hastings", "hastings.htm");
    options.Conventions.AddPageRoute("/Hastings", "hastings.html");
});
builder.Services.AddSingleton<BritBase.Services.PgnService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapGet("/", () => Results.Redirect("/Home"));

app.MapStaticAssets();
app.MapRazorPages()
    .WithStaticAssets();

app.Run();
