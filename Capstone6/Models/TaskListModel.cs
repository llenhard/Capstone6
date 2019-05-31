namespace Capstone6
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Linq;

    public class TaskListModel : DbContext
    {

        public TaskListModel() : base("name=TaskListModel")
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Task> Tasks { get; set; }
    }

    public class User
    {
        [Key]
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class Task
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [ForeignKey("User")]
        public string Assigned { get; set; }
        public DateTime? Due { get; set; }
        public string Desc { get; set; }
        public bool Complete { get; set; }

        public virtual User User { get; set; }
    }
}