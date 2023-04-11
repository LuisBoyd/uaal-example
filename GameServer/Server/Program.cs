using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using Server;
using Server.Models;
using Server.Services;


//Start of Program.cs

#region Configure Service
var builder = WebApplication.CreateBuilder(args); //Dependency Injection Container Object

/*
 * We build settings object the settings is a singleton because we won't have access (or at least we should not)
 *  to the dependency container outside of this start up we create a singelton for the settings that can hold
 *  the secret key's we need at runtime or any other setting's
 */
var settings = new Settings();
var corsPolicy = new CorsPolicy();
var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Configuration.Bind("JWT",settings);
builder.Configuration.Bind("Cors", corsPolicy);
settings.PepperKey = builder.Configuration["PepperKey"];
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

/*
 * add Identity of type User and give it roles the user is what we use to interact with the api
 * the user itself can have many different role's and those roles can have claims allowing for further
 * customisable authorization level's
 */
// builder.Services.AddIdentity<UserTestData, IdentityRole>()
//     .AddEntityFrameworkStores<GameDbContext>();
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    //options.Password.RequireDigit = true; //Password Requires a digit in it
   //options.Password.RequireLowercase = true; //Password requires a lower Case character
    //options.Password.RequireNonAlphanumeric = true; //password requires a non aplhanumeric (?,#,\)
    //options.Password.RequireUppercase = true; //password requires a upperCase character
    //options.Password.RequiredLength = 6; //Minimum length for a password must be 6
    //options.Password.RequiredUniqueChars = 1; //there must at least be 1 unique character
}).AddEntityFrameworkStores<GameDbContext>().AddDefaultTokenProviders();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUserService, UserService>(); //User service Injection (App User's)
builder.Services.AddScoped<IMapService, MapService>(); //Map Service Injection (User Maps)

builder.Services.AddAuthentication(o =>
{
    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.SaveToken = true;
    o.RequireHttpsMetadata = false; //Should be enabled in production enviornmnent.
    o.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero, //Only for development when wanting access token to without any padding room of 5 mins by default
        
        ValidAudience = settings.ValidAudience,
        ValidIssuer = settings.ValidIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.BearerKey))
    };
}); //Authentication Injection


builder.Services.AddCors(o =>
{

    o.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins(corsPolicy.AllowedOrigins);
        });
    
});//Adding Cors for Cross-origin-resource-sharing

builder.Services.AddControllers();
builder.Services.AddControllers().AddNewtonsoftJson(o =>
{
  
}); //Adding A Json Serialization and Deserialization library

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#endregion

#region Configure HTTP pipeline
var app = builder.Build(); //Build the Dependency injection container.
//app.Urls.Add("http://192.168.56.101:5024"); //Ubuntu ip development test

/*
 * the Pipeline is run each time a request is sent to the web server
 * from top-bottom it passes the request through each middle ware example (app.UseHttpsRedirection())
 * is a middleware that redirects the request to HTTPS protocol.
 * after the request has been passed through all the middleware it returns the request.
 */

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection(); //redirects HTTP to Https
app.UseRouting(); //Use routing

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication(); //Authentication before Authorization
app.UseAuthorization();


app.MapControllers();

app.Run();
#endregion