using System;
using System.Collections.Generic;
using System.Text;

namespace myFirstProject.Model
{
    public class State
    {
        public int Id { get; set; }
        public string? StateName { get; set; }
        public int CountryId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
    }
}
