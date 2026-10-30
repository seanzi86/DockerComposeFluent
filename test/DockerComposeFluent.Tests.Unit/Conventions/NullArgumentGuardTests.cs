using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using DockerComposeFluent.Builders;

namespace DockerComposeFluent.Tests.Unit.Conventions
{
    // Every public With* method on a builder must reject a null reference argument with an ArgumentException
    // (ArgumentNullException included) at the call, never later from Build() or ToYaml().
    public class NullArgumentGuardTests
    {
        private static IEnumerable<MethodInfo> BuilderMethods()
        {
            return typeof(ServiceBuilder).Assembly.GetExportedTypes()
                .Where(type => type.IsClass && type.Name.EndsWith("Builder", StringComparison.Ordinal))
                .SelectMany(type => type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                .Where(method => method.Name.StartsWith("With", StringComparison.Ordinal));
        }

        private static object? ValidValue(Type type)
        {
            if (type == typeof(string)) return "a";
            if (type == typeof(int)) return 1;
            if (type == typeof(long)) return 1L;
            if (type == typeof(double)) return 0.5;
            if (type == typeof(bool)) return true;
            if (type == typeof(TimeSpan)) return TimeSpan.FromSeconds(1);
            if (type.IsEnum) return Enum.GetValues(type).GetValue(0);
            if (type == typeof(IEnumerable<string>)) return new[] { "a" };
            if (type == typeof(IEnumerable<KeyValuePair<string, string>>)) return new[] { new KeyValuePair<string, string>("a", "b") };
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Action<>))
            {
                ParameterExpression parameter = Expression.Parameter(type.GetGenericArguments()[0]);
                return Expression.Lambda(type, Expression.Empty(), parameter).Compile();
            }

            if (type.IsClass && type.GetConstructor(Type.EmptyTypes) != null) return Activator.CreateInstance(type);
            return null;
        }

        private static bool CanBuildValid(Type type)
        {
            return type.IsValueType ? type == typeof(int) || type == typeof(long) || type == typeof(double) || type == typeof(bool) || type == typeof(TimeSpan) || type.IsEnum
                : ValidValue(type) != null;
        }

        public static IEnumerable<object[]> Cases()
        {
            NullabilityInfoContext context = new NullabilityInfoContext();
            foreach (MethodInfo method in BuilderMethods())
            {
                ParameterInfo[] parameters = method.GetParameters();
                for (int i = 0; i < parameters.Length; i++)
                {
                    Type type = parameters[i].ParameterType;
                    if (type.IsValueType || type == typeof(object)) continue;
                    if (context.Create(parameters[i]).WriteState == NullabilityState.Nullable) continue;
                    if (!parameters.Where((_, index) => index != i).All(p => CanBuildValid(p.ParameterType))) continue;

                    yield return new object[] { new MethodCase(method, i) };
                }
            }
        }

        [Fact]
        public void TheSweepFindsMethodsToCheck()
        {
            Assert.True(Cases().Count() > 300);
        }

        [Theory]
        [MemberData(nameof(Cases))]
        public void NullArgument_IsRejectedAtTheCall(MethodCase testCase)
        {
            MethodInfo method = testCase.Method;
            object builder = Activator.CreateInstance(method.DeclaringType!)!;
            object?[] arguments = method.GetParameters()
                .Select((p, index) => index == testCase.NullIndex ? null : ValidValue(p.ParameterType))
                .ToArray();

            Exception? thrown = null;
            try
            {
                method.Invoke(builder, arguments);
            }
            catch (TargetInvocationException ex)
            {
                thrown = ex.InnerException;
            }

            Assert.IsAssignableFrom<ArgumentException>(thrown);
        }

        public sealed class MethodCase
        {
            public MethodCase(MethodInfo method, int nullIndex)
            {
                Method = method;
                NullIndex = nullIndex;
            }

            public MethodInfo Method { get; }
            public int NullIndex { get; }

            public override string ToString()
            {
                return Method.DeclaringType!.Name + "." + Method + " arg " + NullIndex;
            }
        }
    }
}
