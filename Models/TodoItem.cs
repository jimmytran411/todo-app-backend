using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace TodoApp.Models
{
    [Table("Todo_item")]
    public partial class TodoItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Column("Created_At", TypeName = "timestamp with time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("Updated_At", TypeName = "timestamp with time zone")]
        public DateTime UpdatedAt { get; set; }
        [Column("Status")]
        public StatusTypes Status { get; set; }
        [Column("User_Id")]
        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty("TodoItems")]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
    public enum StatusTypes
    {
        NotStarted,
        OnGoing,
        Completed
    }
}
