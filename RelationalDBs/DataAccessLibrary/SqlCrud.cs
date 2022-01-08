using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccessLibrary
{
    public class SqlCrud
    {
        private readonly string connectionString;
        private SQLDataAccess db = new SQLDataAccess();

        public SqlCrud(String connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<BasicContactModel> GetAllContacts() 
        {
            string queryString = "SELECT Id, FirstName, LastName FROM dbo.Contacts";

            return db.LoadData<BasicContactModel, dynamic>(queryString, new { }, connectionString);
        }

        public FullContactModel GetFullContactById(int id) 
        {
            FullContactModel output = new FullContactModel();

            string basicInfoQuery = @"SELECT Id, FirstName, LastName 
                                      FROM dbo.Contacts 
                                      WHERE Id = @Id";

            BasicContactModel basicInfo = db.LoadData<BasicContactModel, dynamic>(basicInfoQuery,
                                                                                  new {Id = id },
                                                                                  connectionString
                                                                                  ).FirstOrDefault();

            if (basicInfo == null) 
            {
                Console.WriteLine($"User with id:{ id } not found");
                return null;
            }

            output.Id = basicInfo.Id;
            output.FirstName = basicInfo.FirstName;
            output.LastName = basicInfo.LastName;


            string emailQuery = @"SELECT e.*
                                  FROM dbo.EmailAddresses AS e
                                  INNER JOIN 
                                  dbo.ContactEmail AS ce ON ce.EmailAddressId = e.Id
                                  WHERE ce.ContactID = @Id";

            output.EmailAdresses = db.LoadData<EmailAddressModel, dynamic>(emailQuery,
                                                                new { Id = id },
                                                                connectionString).ToList();

            string phoneNumberQuery = @"SELECT p.*
                                  FROM dbo.PhoneNumbers AS p
                                  INNER JOIN 
                                  dbo.ContactPhoneNumbers AS cpn ON cpn.PhoneNumberId = p.Id
                                  WHERE cpn.ContactID = @Id";

            output.PhoneNumbers = db.LoadData<PhoneNumberModel, dynamic>(phoneNumberQuery,
                                                                         new { Id = id },
                                                                         connectionString).ToList();

            return output;
        }
    }
}
