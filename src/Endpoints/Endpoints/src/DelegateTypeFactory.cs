using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Quandt.Endpoints
{
    // Thanks to svick for the solution to creating a dynamic delegate that has param resolution for DI
    // https://stackoverflow.com/a/9507589
    internal class DelegateTypeFactory
    {
        private readonly ModuleBuilder m_module;

        public DelegateTypeFactory()
        {
            var assembly = AssemblyBuilder.DefineDynamicAssembly(
                new AssemblyName("DelegateTypeFactory"), AssemblyBuilderAccess.RunAndCollect);
            m_module = assembly.DefineDynamicModule("DelegateTypeFactory");
        }

        public Type CreateDelegateType(MethodInfo method)
        {
            if (method.DeclaringType == null) { throw new NullReferenceException("Method DeclaringType cannot be null"); }

            string nameBase = string.Format("{0}{1}", method.DeclaringType.Name, method.Name);
            string name = GetUniqueName(nameBase);

            var typeBuilder = m_module.DefineType(
                name, TypeAttributes.Sealed | TypeAttributes.Public, typeof(MulticastDelegate));

            

            var constructor = typeBuilder.DefineConstructor(
                MethodAttributes.RTSpecialName | MethodAttributes.HideBySig | MethodAttributes.Public,
                CallingConventions.Standard, new[] { typeof(object), typeof(IntPtr) });
            constructor.SetImplementationFlags(MethodImplAttributes.CodeTypeMask);

            var parameters = method.GetParameters();

            var invokeMethod = typeBuilder.DefineMethod(
                "Invoke", MethodAttributes.HideBySig | MethodAttributes.Virtual | MethodAttributes.Public,
                method.ReturnType, parameters.Select(p => p.ParameterType).ToArray());
            invokeMethod.SetImplementationFlags(MethodImplAttributes.CodeTypeMask);

            for (int i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];
                invokeMethod.DefineParameter(i + 1, ParameterAttributes.None, parameter.Name);
            }

            var returnType = typeBuilder.CreateType();

            if (returnType == null) { throw new NullReferenceException("Something went wrong creating delegate type"); }

            return returnType;
        }

        private string GetUniqueName(string nameBase)
        {
            int number = 2;
            string name = nameBase;
            while (m_module.GetType(name) != null)
                name = nameBase + number++;
            return name;
        }
    }
}
