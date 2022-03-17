using System;

namespace ST.SolutionHub.DataLayer.Entities.Base
{
    public class BaseEntity<T> : IBaseEntity<T>
    {
        public T Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
