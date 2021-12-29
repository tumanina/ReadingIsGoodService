using ReadingIsGoodService.Common.Enums;

namespace ReadingIsGoodService.Data.Entities
{
    public class ActivityLogEntity : BaseEntity
    {
        public EntityType EntityType { get; set; }
        public int EntityId { get; set; }
        public string New { get; set; }
        public int UserId { get; set; }
    }
}
