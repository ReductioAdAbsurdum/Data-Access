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
        private SqlDataAccess db = new SqlDataAccess();

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

        public void CreateContact(FullContactModel contact) 
        {
            // Save basic contact
            String sql = "INSERT INTO dbo.Contacts (FirstName, LastName) VALUES (@FirstName, @LastName);";
            db.SaveData(sql,
                        new 
                        { 
                            contact.FirstName,
                            contact.LastName 
                        },
                        connectionString);

            // Get the id of created contact
            sql = "SELECT Id FROM dbo.Contacts where FirstName = @FirstName AND LastName = @LastName;";
            int contactId = db.LoadData<IdModel, dynamic>(sql,
                                                new 
                                                { 
                                                    contact.FirstName,
                                                    contact.LastName 
                                                },
                                                connectionString).First().Id;


            foreach (PhoneNumberModel p in contact.PhoneNumbers)
            {
                if (p.Id == 0) 
                {
                    sql = "INSERT into dbo.PhoneNumbers (PhoneNumber) VALUES (@PhoneNumber);";
                    db.SaveData(sql, new { p.PhoneNumber }, connectionString);

                    sql = "SELECT Id from dbo.PhoneNumbers AS p WHERE p.PhoneNumber = @PhoneNumber; ";
                    p.Id = db.LoadData<IdModel, dynamic>(sql, new { PhoneNumber = p.PhoneNumber }, connectionString).First().Id;
                }

                sql = "INSERT INTO dbo.ContactPhoneNumbers (ContactId, PhoneNumberId) VALUES (@ContactId, @PhoneNumberId);";
                db.SaveData(sql, new {ContactId = contactId , PhoneNumberId = p.Id }, connectionString);
            }

            foreach (EmailAddressModel e in contact.EmailAdresses)
            {
                if (e.Id == 0) 
                {
                    sql = "INSERT INTO dbo.EmailAddresses (EmailAddress) VALUES (@EmailAddress);";
                    db.SaveData(sql, new {@EmailAddress = e.EmailAddress }, connectionString);

                    sql = "select Id from dbo.EmailAddresses as e where e.EmailAddress = @EmailAddress;";
                    e.Id = db.LoadData<IdModel, dynamic>(sql, new { @EmailAddress = e.EmailAddress }, connectionString).First().Id;
                }

                sql = "insert into dbo.ContactEmail (ContactId, EmailAddressId) values (@ContactId, @EmailAddressId);";
                db.SaveData(sql, new { ContactId = contactId, EmailAddressId = e.Id }, connectionString);
            }

        }

        public void UpdateContactName(BasicContactModel contact) 
        {
            string sql = @"Update dbo.Contacts
                            Set FirstName = @FirstName, LastName = @LastName
                            Where Id = @Id;";

            db.SaveData(sql, contact, connectionString);
        }

        public void RemovePhoneNumberWithId(int phoneNumberId) 
        {
            String sql = "Delete From dbo.PhoneNumbers Where Id = @Id;";
            db.SaveData(sql, new { Id = phoneNumberId }, connectionString);

            sql = "Delete From dbo.ContactPhoneNumbers Where PhoneNumberId = @Id;";
            db.SaveData(sql, new { Id = phoneNumberId }, connectionString);
        }
    }
}
