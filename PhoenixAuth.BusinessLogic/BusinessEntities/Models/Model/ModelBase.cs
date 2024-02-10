using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PhoenixAuth.BusinessLogic.BusinessEntities.Models.Model
{
    /// <summary>
    /// Holds all shared model/entity properties
    /// </summary>
    public class ModelBase
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime IssueDate { get; set; } = DateTime.UtcNow;


    }
}
