using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NonProfitManagement.Models;

namespace NonProfitManagement.Data
{
    public static class ModelBuilderExtensions {
        public static void Seed(this ModelBuilder builder) {
            var pwd = "P@$$w0rd";
            var passwordHasher = new PasswordHasher<IdentityUser>();

            // Seed Roles
            var adminRole = new IdentityRole("Admin");
            adminRole.NormalizedName = adminRole.Name.ToUpper();

            var memberRole = new IdentityRole("Member");
            memberRole.NormalizedName = memberRole.Name.ToUpper();

            // EJ
            var financeRole = new IdentityRole("Finance");
            financeRole.NormalizedName = financeRole.Name.ToUpper();

            List<IdentityRole> roles = new List<IdentityRole>() {
                adminRole,
                memberRole,
                financeRole // EJ
            };

            builder.Entity<IdentityRole>().HasData(roles);

            
            // Seed Users
            var adminUser = new IdentityUser {
                UserName = "a@a.a",
                Email = "a@a.a",
                EmailConfirmed = true,
            };
            adminUser.NormalizedUserName = adminUser.UserName.ToUpper();
            adminUser.NormalizedEmail = adminUser.Email.ToUpper();
            adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, pwd);

            var memberUser = new IdentityUser {
                UserName = "m@m.m",
                Email = "m@m.m",
                EmailConfirmed = true,
            };
            memberUser.NormalizedUserName = memberUser.UserName.ToUpper();
            memberUser.NormalizedEmail = memberUser.Email.ToUpper();
            memberUser.PasswordHash = passwordHasher.HashPassword(memberUser, pwd);

            // EJ
            var financeUser = new IdentityUser {
                UserName = "f@f.f",
                Email = "f@f.f",
                EmailConfirmed = true,
            };
            financeUser.NormalizedUserName = financeUser.UserName.ToUpper();
            financeUser.NormalizedEmail = financeUser.Email.ToUpper();
            financeUser.PasswordHash = passwordHasher.HashPassword(financeUser, pwd);

            List<IdentityUser> users = new List<IdentityUser>() {
                adminUser,
                memberUser,
                financeUser // EJ
            };

            builder.Entity<IdentityUser>().HasData(users);

            // Seed UserRoles
            List<IdentityUserRole<string>> userRoles = new List<IdentityUserRole<string>>();

            userRoles.Add(new IdentityUserRole<string> {
                UserId = users[0].Id,
                RoleId = roles.First(q => q.Name == "Admin").Id
            });

            userRoles.Add(new IdentityUserRole<string> {
                UserId = users[1].Id,
                RoleId = roles.First(q => q.Name == "Member").Id
            });

            // EJ
            userRoles.Add(new IdentityUserRole<string> {
                UserId = users[2].Id,
                RoleId = roles.First(q => q.Name == "Finance").Id
            });

            builder.Entity<IdentityUserRole<string>>().HasData(userRoles);

            builder.Entity<ContactList>().HasData(
                GetContactLists()
            );
            builder.Entity<Donation>().HasData(
                GetDonations()
            );
            builder.Entity<PaymentMethod>().HasData(
                GetPaymentMethods()
            );
            builder.Entity<TransactionType>().HasData(
                GetTransactionTypes()
            );
        }
                public static List<ContactList> GetContactLists() {
            List<ContactList> contactLists = new List<ContactList>() {
                new ContactList() {
                    AccountNo=12,
                    FirstName="Sam",
                    LastName="Fox",
                    Email="sam@fox.com",
                    Street="457 Fox Avenue",
                    City="Richmond",
                    PostalCode="V4F 1M7",
                    Country="Canada"
                },
                new ContactList() {
                    AccountNo=17,
                    FirstName="Ann",
                    LastName="Day",
                    Email="ann@day.com",
                    Street="231 River Road",
                    City="Delta",
                    PostalCode="V6G 1M6",
                    Country="Canada"
                },
                new ContactList() {
                    AccountNo=24,
                    FirstName="Ellie",
                    LastName="Smith",
                    Email="ellie@smith.com",
                    Street="314 12th Avenue",
                    City="Burnaby",
                    PostalCode="V7L 3J2",
                    Country="Canada"
                },
            };
            return contactLists;
        }

        public static List<Donation> GetDonations() {
            List<Donation> donations = new List<Donation>() {
                new Donation() {
                    TransId=1,
                    Date=DateTime.Now,
                    AccountNo=24,
                    TransactionTypeId=1,
                    Amount=500,
                    PaymentMethodId=1,
                    Notes="Donated monthly"
                },
                new Donation() {
                    TransId=2,
                    Date=DateTime.Now,
                    AccountNo=17,
                    TransactionTypeId=2,
                    Amount=1000,
                    PaymentMethodId=2,
                    Notes="Donated for homeless people"
                },
                new Donation() {
                    TransId=3,
                    Date=DateTime.Now,
                    AccountNo=12,
                    TransactionTypeId=3,
                    Amount=750,
                    PaymentMethodId=2,
                    Notes="Donators want a new gym"
                },
            };
            return donations;
        }

        public static List<PaymentMethod> GetPaymentMethods() {
            List<PaymentMethod> paymentMethods = new List<PaymentMethod>() {
                new PaymentMethod() {
                    PaymentMethodId=1,
                    Name="Direct Deposit"
                },
                new PaymentMethod() {
                    PaymentMethodId=2,
                    Name="Paypal"
                },
                new PaymentMethod() {
                    PaymentMethodId=3,
                    Name="Check"
                },
            };
            return paymentMethods;
        }

        public static List<TransactionType> GetTransactionTypes() {
            List<TransactionType> transactionTypes = new List<TransactionType>() {
                new TransactionType() {
                    TransactionTypeId=1,
                    Name="General Donation",
                    Description="Donations made without any special purpose"
                },
                new TransactionType() {
                    TransactionTypeId=2,
                    Name="Food for homeless",
                    Description="Donations made for homeless people"
                },
                new TransactionType() {
                    TransactionTypeId=3,
                    Name="Repair of Gym",
                    Description="Donations for the purpose of upgrading the gym"
                },
            };
            return transactionTypes;
        }
    }
}