using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Salesforce.Migrations.Assistant.Library.Domain
{
    static public class Extensions
    {
        static public void CallPrivate(this object objInstance, string methodName, object[] methodParameters)
        {
            MethodInfo privMethod = objInstance.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            privMethod.Invoke(objInstance, new object[] { methodParameters });
        }
    }
}
