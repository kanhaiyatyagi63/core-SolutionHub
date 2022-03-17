using ST.SolutionHub.DataLayer.Entities.Base;
using System;

namespace ST.SolutionHub.DataLayer.Entities
{
    public class RefreshToken : BaseEntity<long>
    {
        public string UserId { get; set; }
        public string Token { get; set; }
        public string IPAddress { get; set; }
        public DateTime ExpiresOn { get; set; }
    }
}
