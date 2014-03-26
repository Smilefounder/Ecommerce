using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Kooboo.Commerce.API.LocalProvider
{
    /// <summary>
    /// entity - object mapper
    /// 
    /// to avoid recursive complex property map, the complex property names should be given explicitly.
    /// for example:
    /// public class User 
    /// {
    ///     public string Id { get; set; }
    ///     public Role Role { get; set; }
    /// }
    /// 
    /// public class Role 
    /// {
    ///     public string Name { get; set; }
    ///     public ICollection<Permission> Permissions { get; set; }
    /// }
    /// 
    /// public class Permission
    /// {
    ///     public string Name { get; set; }
    ///     public bool Allowed { get; set; }
    /// }
    /// User class has a Role property, and Role class has a list of Permissions property.
    /// if you want to map the role property and and permissions property of role as well when mapping an user entity,
    /// pass include complex property names as:
    /// new string[] { "Role", "Role.Permissions" },
    /// 
    /// please notice that the parameter is the property name, not the property type name.
    /// 
    /// </summary>
    /// <typeparam name="T">object</typeparam>
    /// <typeparam name="U">entity</typeparam>
    public interface IMapper<T, U>
        where T: class, new()
        where U: class, new()
    {
        /// <summary>
        /// map the entity to object
        /// </summary>
        /// <param name="obj">entity</param>
        /// <param name="includeComplexPropertyNames">include extra complex type property</param>
        /// <returns>object</returns>
        T MapTo(U obj, params string[] includeComplexPropertyNames);
        /// <summary>
        /// map the object to entity
        /// </summary>
        /// <param name="obj">object</param>
        /// <param name="includeComplexPropertyNames">include extra complex type property</param>
        /// <returns>entity</returns>
        U MapFrom(T obj, params string[] includeComplexPropertyNames);
    }
}
