using System;
using System.Collections.Generic;
using System.Text;

namespace myFirstProject.Model
{
    public class District
    {
        public int Id { get; set; }
        public string? DistrictName { get; set; }
        public int StateId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
    }
}
