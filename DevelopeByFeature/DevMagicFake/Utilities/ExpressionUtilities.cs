// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExpressionUtilities.cs" company="http://mohamedradwan.wordpress.com">
//   © 2011 M.Radwan. All rights reserved
// </copyright>
// <summary>
//   The expression utilities.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region

using System;
using System.Linq.Expressions;
using System.Reflection;

#endregion

namespace M.Radwan.DevMagicFake.Utilities
{
    /// <summary>
    /// The expression utilities.
    /// </summary>
    internal class ExpressionUtilities
    {
        // No need I just create it maybe I will need it
        #region Public Methods

        public static object GetParamterFromMethod(MethodCallExpression methodCallExpression, string methodName, int paramIndex)
        {
            if (MemberOf(methodCallExpression).Name == methodName)
            {
                var argument = methodCallExpression.Arguments[paramIndex];
                var returnValue = GetValueFromExpression(argument);
                return returnValue;
            }

            if (methodCallExpression.Arguments.Count == 0)
            {
                return null;
            }

            foreach (var expression in methodCallExpression.Arguments)
            {
                var method = TryParseAsMethod(expression);
                if (method == null)
                    return null;
                if (method.Name == methodName)
                {
                    return GetParamterFromMethod((MethodCallExpression)expression, methodName, paramIndex);
                }
            }

            return GetParamterFromMethod(methodCallExpression, methodName, paramIndex);
        }

        /// <summary>
        /// The get value from expression.
        /// </summary>
        /// <param name="expression">
        /// The expression.
        /// </param>
        /// <returns>
        /// The get value from expression.
        /// </returns>
        public static object GetValueFromExpression(MemberExpression expression)
        {
            Expression conversion = Expression.Convert(expression, typeof(object));
            var getterLambda = Expression.Lambda<Func<object>>(conversion);

            var getter = getterLambda.Compile();
            return getter();
        }

        // No need I just create it maybe I will need it
        /// <summary>
        /// The get value from expression.
        /// </summary>
        /// <param name="expression">
        /// The expression.
        /// </param>
        /// <returns>
        /// The get value from expression.
        /// </returns>
        public static object GetValueFromExpression(ConstantExpression expression)
        {
            Expression conversion = Expression.Convert(expression, typeof(object));
            var getterLambda = Expression.Lambda<Func<object>>(conversion);

            var getter = getterLambda.Compile();
            return getter();
        }

        // No need I just create it maybe I will need it

        /// <summary>
        /// The get value from expression.
        /// </summary>
        /// <param name="expression">
        /// The expression.
        /// </param>
        /// <returns>
        /// The get value from expression.
        /// </returns>
        public static object GetValueFromExpression(Expression expression)
        {
            Expression conversion = Expression.Convert(expression, typeof(object));
            var getterLambda = Expression.Lambda<Func<object>>(conversion);

            var getter = getterLambda.Compile();
            return getter();
        }

        /// <summary>
        /// The is method called.
        /// </summary>
        /// <param name="methodCallExpression">
        /// The method call expression.
        /// </param>
        /// <param name="methodName">
        /// The method name.
        /// </param>
        /// <returns>
        /// The is method called.
        /// </returns>
        public static bool IsMethodCalled(MethodCallExpression methodCallExpression, string methodName)
        {
            if (MemberOf(methodCallExpression).Name == methodName)
            {
                return true;
            }

            foreach (var expression in methodCallExpression.Arguments)
            {
                var method = TryParseAsMethod(expression);
                if (method == null)
                {
                    return false;
                }

                if (method.Name == methodName)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// The member of.
        /// </summary>
        /// <param name="body">
        /// The body.
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="ArgumentException">
        /// </exception>
        public static MemberInfo MemberOf(Expression body)
        {
            {
                var member = body as MemberExpression;
                if (member != null) return member.Member;
            }
            {
                var method = body as MethodCallExpression;
                if (method != null) return method.Method;

            }

            throw new ArgumentException(
                "'" + body + "': not a member access");
        }

        /// <summary>
        /// The try parse as member.
        /// </summary>
        /// <param name="body">
        /// The body.
        /// </param>
        /// <returns>
        /// </returns>
        public static MemberInfo TryParseAsMember(Expression body)
        {
            {
                var member = body as MemberExpression;
                if (member != null)
                {
                    return member.Member;
                }

                return null;
            }
        }

        /// <summary>
        /// The try parse as method.
        /// </summary>
        /// <param name="body">
        /// The body.
        /// </param>
        /// <returns>
        /// </returns>
        public static MemberInfo TryParseAsMethod(Expression body)
        {
            {
                var method = body as MethodCallExpression;
                if (method != null)
                {
                    return method.Method;
                }

                return null;
            }
        }

        #endregion
    }
}
