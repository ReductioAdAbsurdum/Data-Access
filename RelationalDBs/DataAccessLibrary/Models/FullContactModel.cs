using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.Models
{
    public class FullContactModel
    {
        public int Id { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public List<EmailAddressModel> EmailAdresses { get; set; }
        public List<PhoneNumberModel> PhoneNumbers { get; set; }
    }
}
