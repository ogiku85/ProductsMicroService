using System;
using System.Collections.Generic;
using System.Text;

namespace ProductsMicroService.Data.Models
{
    public interface IAudit
    {
        int Id { get; set; }
        DateTime Created { get; set; }
        string CreatedBy { get; set; }
        DateTime Modified { get; set; }
        string ModifiedBy { get; set; }
        bool IsDeleted { get; set; }
        DateTime Deleted { get; set; }
        string DeletedBy { get; set; }
    }
}
