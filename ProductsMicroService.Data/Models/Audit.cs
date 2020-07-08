using System;
using System.Collections.Generic;
using System.Text;

namespace ProductsMicroService.Data.Models
{
    public class Audit : IAudit
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime Modified { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime Deleted { get; set; }
        public string DeletedBy { get; set; }
    }
}
