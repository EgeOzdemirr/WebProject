// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using IdentityModel;
using WebProject.IdentityServer.Data;
using WebProject.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace WebProject.IdentityServer
{
    public class SeedData
    {
        private static string GeneratePassword()
        {
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnpqrstuvwxyz23456789";
            var bytes = RandomNumberGenerator.GetBytes(20);
            var body = new string(bytes.Select(b => chars[b % chars.Length]).ToArray());
            return body + "aA1!"; // Identity parola kurallarını (rakam, büyük/küçük harf, sembol) garanti eder
        }

        public static void EnsureSeedData(string connectionString)
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseNpgsql(connectionString));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            using (var serviceProvider = services.BuildServiceProvider())
            {
                using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                    context.Database.Migrate();

                    var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                    if (!roleMgr.RoleExistsAsync("Admin").Result)
                    {
                        roleMgr.CreateAsync(new IdentityRole("Admin")).Wait();
                        Log.Debug("Admin role created");
                    }

                    // 'alice' herkese açık, yetkisiz bir demo kullanıcısıdır.
                    var demoPassword = Environment.GetEnvironmentVariable("SEED_DEMO_PASSWORD") ?? "Pass123$";
                    // 'bob' Admin'dir: parola ortam değişkeniyle verilir, yoksa rastgele üretilip bir kez yazdırılır.
                    var adminPassword = Environment.GetEnvironmentVariable("SEED_ADMIN_PASSWORD");
                    if (string.IsNullOrWhiteSpace(adminPassword))
                    {
                        adminPassword = GeneratePassword();
                        Log.Warning("SEED_ADMIN_PASSWORD verilmedi. 'bob' (Admin) için üretilen parola: {Password}", adminPassword);
                    }

                    var alice = userMgr.FindByNameAsync("alice").Result;
                    if (alice == null)
                    {
                        alice = new ApplicationUser
                        {
                            UserName = "alice",
                            Email = "AliceSmith@email.com",
                            EmailConfirmed = true,
                        };
                        var result = userMgr.CreateAsync(alice, demoPassword).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(alice, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "Alice Smith"),
                            new Claim(JwtClaimTypes.GivenName, "Alice"),
                            new Claim(JwtClaimTypes.FamilyName, "Smith"),
                            new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }
                        Log.Debug("alice created");
                    }
                    else
                    {
                        Log.Debug("alice already exists");
                    }

                    var bob = userMgr.FindByNameAsync("bob").Result;
                    if (bob == null)
                    {
                        bob = new ApplicationUser
                        {
                            UserName = "bob",
                            Email = "BobSmith@email.com",
                            EmailConfirmed = true
                        };
                        var result = userMgr.CreateAsync(bob, adminPassword).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(bob, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "Bob Smith"),
                            new Claim(JwtClaimTypes.GivenName, "Bob"),
                            new Claim(JwtClaimTypes.FamilyName, "Smith"),
                            new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                            new Claim("location", "somewhere")
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }
                        Log.Debug("bob created");
                    }
                    else
                    {
                        Log.Debug("bob already exists");
                    }

                    if (!userMgr.IsInRoleAsync(bob, "Admin").Result)
                    {
                        userMgr.AddToRoleAsync(bob, "Admin").Wait();
                        Log.Debug("bob added to Admin role");
                    }
                }
            }
        }
    }
}
