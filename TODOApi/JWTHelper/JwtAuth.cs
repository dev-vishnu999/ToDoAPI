using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TODOApi
{
    public static class JwtAuth
    {
        public static IServiceCollection AddAuth(
            this IServiceCollection services,
            JwtConfig jwtconfg)
        {
            services
                .AddAuthorization(options =>
                {
                    options.AddPolicy("SuperAdmin", policy => policy.RequireRole("SuperAdmin"));
                    options.AddPolicy("Admin", policy => policy.RequireRole("SuperAdmin", "Admin"));
                    options.AddPolicy("User", policy => policy.RequireRole("User", "SuperAdmin", "Admin"));
                })
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = jwtconfg.Issuer,
                        ValidAudience = jwtconfg.Issuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtconfg.Secret)),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            return services;
        }

        public static IApplicationBuilder UseAuth(this IApplicationBuilder app)
        {
            app.UseAuthentication();

            app.UseAuthorization();

            return app;
        }
    }
}
