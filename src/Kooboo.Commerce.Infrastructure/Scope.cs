using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce
{
    public static class Scope
    {
        /// <summary>
        /// Gets the instance of the specified type within the current scope.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Current<T>()
            where T : class
        {
            return Scope<T>.Current;
        }

        /// <summary>
        /// Begin a scope for the specified instance to let the instance accessible within the scope in the same thread.
        /// </summary>
        public static Scope<T> Begin<T>(T instance)
            where T : class
        {
            return new Scope<T>(instance);
        }
    }
}
