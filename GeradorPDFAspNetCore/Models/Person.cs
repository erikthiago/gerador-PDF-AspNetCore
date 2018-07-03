using System;

namespace GeradorPDFAspNetCore.Models
{
    public class Person
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
    }
}
