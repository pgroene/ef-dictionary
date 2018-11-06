using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace efdictionary.Entities
{
    public class Child
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [ForeignKey("ParentFK")]
        public Guid ParentId { get; set; }
    }

}
