using DataAccessLibrary;
using DataAccessLibrary.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;


namespace SQLServerUI
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlCrud sql = new SqlCrud(GetConnectionString());

            //ReadAllContacts(sql);

            //ReadFullContactById(sql, 1);

            //CreateNewContact(sql);

            //UpdateContactName(sql);

            RemovePhoneNumberWithId(sql, 1);
        }

        private static void CreateNewContact(SqlCrud sql) 
        {
            FullContactModel user = new FullContactModel
            {
                FirstName = "Srdjan",
                LastName = "Stankovic"
            };
            user.EmailAdresses.Add(new EmailAddressModel { EmailAddress = "sample@gmail.com" });
            user.EmailAdresses.Add(new EmailAddressModel
            {
                Id = 1,
                EmailAddress = "moonburn.co@gmail.com"
            });

            user.PhoneNumbers.Add(new PhoneNumberModel { Id = 1, PhoneNumber = "060-49-89-884" });
            user.PhoneNumbers.Add(new PhoneNumberModel { PhoneNumber = "smp-11-11-111" });

            sql.CreateContact(user);
        }
        private static void ReadAllContacts(SqlCrud sql) 
        {
            List<BasicContactModel> rows = sql.GetAllContacts();

            foreach (BasicContactModel p in rows)
            {
                Console.WriteLine($"{p.Id}: {p.FirstName} {p.LastName}");
            }
        }
        private static void ReadFullContactById(SqlCrud sql, int id)
        {
            FullContactModel contact = sql.GetFullContactById(id);
          
            Console.WriteLine($"{contact.FirstName} {contact.LastName} \n");
            
            Console.WriteLine("Email adresses:");
            foreach (EmailAddressModel e in contact.EmailAdresses)
            {
                Console.WriteLine($"    {e.EmailAddress}");
            }
            
            Console.WriteLine("Phone numbers:");
            foreach (PhoneNumberModel p in contact.PhoneNumbers)
            {
                Console.WriteLine($"    {p.PhoneNumber}");
            }
        }
        private static string GetConnectionString(string connectionStringName = "Default") 
        {

            IConfigurationBuilder builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                                      .AddJsonFile("appsettings.json");

            IConfigurationRoot config = builder.Build();

            return config.GetConnectionString(connectionStringName);
        }
        private static void UpdateContactName(SqlCrud sql) 
        {
            sql.UpdateContactName(new BasicContactModel
            {
                Id = 1,
                FirstName = "Som ting",
                LastName = "Wong"
            });
        }

        private static void RemovePhoneNumberWithId(SqlCrud sql, int id)
        {
            sql.RemovePhoneNumberWithId(id);
        }

    }
}
