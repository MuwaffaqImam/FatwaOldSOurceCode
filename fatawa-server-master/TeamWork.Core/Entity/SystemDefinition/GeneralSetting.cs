using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamWork.Core.Entity.SystemDefinition
{
    [Table("GeneralSettings")]
    public class GeneralSetting : BaseEntity
    {
        [Required, MaxLength(150)]
        public string SettingName { get; set; }
        public string SettingValue { get; set; }
    }
}
