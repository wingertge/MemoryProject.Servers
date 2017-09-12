using System;
using System.Collections.Generic;

namespace MemoryApi.DbModels
{
    public class User
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Username { get; set; }
        public FullName Name { get; set; }
        public Address UserAddress { get; set; }
        public string PasswordHash { get; set; }
        public long BirthDate { get; set; }
        public bool EmailVerified { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string Locale { get; set; }
        public bool PhoneVerified { get; set; }
        public string PhoneNumber { get; set; }
        public string ProfilePicture { get; set; }
        public string DisplayName { get; set; }
        public List<Provider> Providers { get; set; }
        public long UpdatedAt { get; set; }
        public string ZoneInfo { get; set; }
        public LanguagePair LastLanguages { get; set; }
        public List<string> Roles { get; set; }

        public struct FullName
        {
            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public string LastName { get; set; }
        }

        public struct Address
        {
            public string Line1 { get; set; }
            public string Line2 { get; set; }
            public string Country { get; set; }
            public string PostalCode { get; set; }
            public string City { get; set; }
            public string Locality { get; set; }
        }

        public struct Provider
        {
            public string Name { get; set; }
            public string AccountId { get; set; }
        }

        public struct LanguagePair
        {
            public int From { get; set; }
            public int To { get; set; }
        }
    }
}