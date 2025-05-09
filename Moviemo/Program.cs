using Microsoft.EntityFrameworkCore;
using Moviemo.Data;
using Moviemo.Services;
using Moviemo.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Controllerlar eklendi
builder.Services.AddControllers();

// Servisler
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IVoteService, VoteService>();

var app = builder.Build();

app.UseRouting();
app.MapControllers();

app.Run();
