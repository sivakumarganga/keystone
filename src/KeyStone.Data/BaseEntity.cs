using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyStone.Data
{
    public interface IEntity
    {

    }
    public abstract class BaseEntity : IEntity
    {

    }

    /// <summary>
    /// Attribute to ignore an entity from the migration process.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class IgnoreEntity : Attribute
    {
        /// <summary>
        /// Gets or sets the reason why the entity is ignored from migration.
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="IgnoreEntity"/> class.
        /// </summary>
        public IgnoreEntity()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IgnoreEntity"/> class with a specified reason.
        /// </summary>
        /// <param name="reason">The reason why the entity is ignored from migration.</param>
        public IgnoreEntity(string reason)
        {
            Reason = reason;
        }
    }
}
