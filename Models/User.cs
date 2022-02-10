using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace TodoApp.Models
{
    [Table("User")]
    public partial class User
    {
        public User()
        {
            TodoItems = new HashSet<TodoItem>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public Guid Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Column("created_at", TypeName = "timestamp with time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("updated_at", TypeName = "timestamp with time zone")]
        public DateTime UpdatedAt { get; set; }

        [InverseProperty(nameof(TodoItem.User))]
        public virtual ICollection<TodoItem> TodoItems { get; set; }
    }
}
