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
            ReadFullContactById(sql, 1);
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
                Console.WriteLine($"                {e.EmailAddress}");
            }
            
            Console.WriteLine("Phone numbers:");
            foreach (PhoneNumberModel p in contact.PhoneNumbers)
            {
                Console.WriteLine($"                {p.PhoneNumber}");
            }
        }
        private static string GetConnectionString(string connectionStringName = "Default") 
        {

            IConfigurationBuilder builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                                      .AddJsonFile("appsettings.json");

            IConfigurationRoot config = builder.Build();

            return config.GetConnectionString(connectionStringName);
        }
    }
}
