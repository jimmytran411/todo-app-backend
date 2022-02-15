using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace TodoApp.Models
{
    [Table("Application_User")]
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            TodoItems = new HashSet<TodoItem>();
        }

        [Column("created_at", TypeName = "timestamp with time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("updated_at", TypeName = "timestamp with time zone")]
        public DateTime UpdatedAt { get; set; }

        [InverseProperty(nameof(TodoItem.ApplicationUser))]
        public virtual ICollection<TodoItem> TodoItems { get; set; }
    }

}
