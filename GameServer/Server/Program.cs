using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using Server;
using Server.Models;
using Server.Services;

//Start of Program.cs

var builder = WebApplication.CreateBuilder(args); //Dependency Injection Container Object

/*
 * We build settings object the settings is a singleton because we won't have access (or at least we should not)
 *  to the dependency container outside of this start up we create a singelton for the settings that can hold
 *  the secret key's we need at runtime or any other setting's
 */
var settings = new Settings();
builder.Configuration.Bind("Settings",settings);
builder.Services.AddSingleton(settings);

// Add services to the container.

/*
 * Connection string (Should be named GameDB Connection string in case we use more than one Database)
 * is a string that allows us to make a connection to a local or online database. the actual connection
 *  string should never be stored in a .cs file or source code file instead we store all the secret key's
 *  (API key's, Connection String's, Hash Key's, etc....) in the appsettings.json
 *  we can then call the builder and get the config and pass in the key Identifier in this case
 *  DB => Connection String to the GameDB.
 *
 * we can then add the DBcontext using the custom class we made (this is documented) we can pass in some
 * optional parameters o.useMySQL allows the DBcontext to use a SQLDatabase in that we pass in the connection
 *  string and the version which we auto detect but the actual version would be whatever version the database uses.
 */

var connectionString = builder.Configuration.GetConnectionString("Db");
builder.Services.AddDbContext<GameDbContext>(o =>
    o.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddControllers().AddNewtonsoftJson(o =>
{
  
}); //Adding A Json Serialization and Deserialization library
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUserService, UserService>(); //User service Injection (App User's)
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>(); //Custom Authentication Injection

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters()
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(settings.BearerKey)),
        ValidateIssuerSigningKey = true,
        ValidateAudience = false,
        ValidateIssuer = false
    };
}); //Authentication Injection

var app = builder.Build(); //Build the Dependency injection container.
//app.Urls.Add("http://192.168.56.101:5024"); //Ubuntu ip development test

/*
 * the Pipeline is run each time a request is sent to the web server
 * from top-bottom it passes the request through each middle ware example (app.UseHttpsRedirection())
 * is a middleware that redirects the request to HTTPS protocol.
 * after the request has been passed through all the middleware it returns the request.
 */

#region Pipeline
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.UseSwagger();
    // app.UseSwaggerUI();
}

app.UseHttpsRedirection(); //redirects HTTP to Https

app.UseAuthentication(); //Authentication before Authorization
app.UseAuthorization();


app.MapControllers();

app.Run();
#endregion