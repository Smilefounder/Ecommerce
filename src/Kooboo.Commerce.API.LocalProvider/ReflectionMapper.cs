using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.API.LocalProvider;
using System.Collections;

namespace DAF.Core.Map
{
    [Dependency(typeof(IMapper<,>), ComponentLifeStyle.Transient)]
    public class ReflectionMapProvider<T, U> : IMapper<T, U>
        where T : class, new()
        where U : class, new()
    {
        private object Map(object fobj, object tobj)
        {
            if (fobj == null)
                return null;
            var fobjProps = fobj.GetType().GetProperties();
            var tobjProps = tobj.GetType().GetProperties();

            foreach (var op in fobjProps)
            {
                var top = tobjProps.FirstOrDefault(o => o.Name == op.Name);
                if (top != null)
                {
                    if (op.PropertyType != top.PropertyType)
                    {
                        // 1. if is array(or enumerable), then map the item in the collection
                        if(typeof(IEnumerable).IsAssignableFrom(op.PropertyType))
                        {
                            // TODO: add mutual collection-array mapping.
                            IEnumerable evals = op.GetValue(fobj, null) as IEnumerable;
                            var topType = top.PropertyType.GetElementType();
                            List<object> nvals = new List<object>();
                            foreach(var val in evals)
                            {
                                var nval = Activator.CreateInstance(topType);
                                nval = Map(val, nval);
                                nvals.Add(nval);
                            }
                            if(top.PropertyType.IsArray)
                            {
                                top.SetValue(tobj, nvals.Count > 0 ? nvals.ToArray() : null, null);
                            }
                            else
                            {
                                top.SetValue(tobj, nvals, null);
                            }
                        }
                        // 2. if is a complex type, then map it
                        else
                        {
                            object val = op.GetValue(fobj, null);
                            object nval = Activator.CreateInstance(top.PropertyType);
                            nval = Map(val, nval);
                            top.SetValue(tobj, nval, null);
                        }
                    }
                    else
                    {
                        // 3. else if is a value type, set the value
                        top.SetValue(tobj, op.GetValue(fobj, null), null);
                    }
                }
            }
            return tobj;
        }

        public T MapTo(U obj)
        {
            T nobj = new T();
            return Map(obj, nobj) as T;
        }

        public U MapFrom(T obj)
        {
            U nobj = new U();
            return Map(obj, nobj) as U;
        }
    }
}
