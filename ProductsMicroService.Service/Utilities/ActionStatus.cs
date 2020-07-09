using System;
using System.Collections.Generic;
using System.Text;

namespace ProductsMicroService.Service.Utilities
{
    public enum ActionStatus
    {
        Ok,
        Created,
        Updated,
        NotFound,
        Deleted,
        NothingModified,
        NotAuthenticated,
        NotAuthorized,
        Error,
        PermissionError,
        PartialData
    }
}
