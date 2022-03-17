using System;

namespace ST.SolutionHub.DataLayer.Entities.Base
{

    public interface IBaseEntity<Tkey>
    {
        Tkey Id { get; set; }
        bool IsDeleted { get; set; }
        DateTime CreatedDate { get; set; }
        DateTime? UpdatedDate { get; set; }
        string CreatedBy { get; set; }
        string UpdatedBy { get; set; }
    }
}
