using System;
using System.Collections.Generic;

using BrainCorp.Passwd.Common.Exceptions;

namespace BrainCorp.Passwd.Common.Utilities
{
    public static class ParameterChecker
    {
        /// <summary>
        /// Similar to the SQL ISNULL function, except it will
        /// return NULL, and you cannot choose the output. 
        /// This method should be used for logging
        /// purposes mostly.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string IsNull(object obj)
        {
            if (null == obj)
            {
                return NULL;
            }

            return obj.ToString();
        }

        /// <summary>
        /// Returns whether obj1.Equals(obj2) except it will
        /// add some logic to check if obj1 is null vs obj2.
        /// </summary>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <returns></returns>
        public static bool EqualsIncludeNull(object obj1, object obj2)
        {
            if( null == obj1 && null == obj2)
            {
                return true;
            }

            if( null == obj1 ||
                null == obj2)
            {
                return false;
            }

            return obj1.Equals(obj2);
        }


        /// <summary>
        /// Returns whether obj1.CompareTo(obj2) except it will
        /// add some logic to check if obj1 is null vs obj2.
        /// </summary>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <returns></returns>
        public static int CompareToIncludeNull(IComparable obj1, IComparable obj2)
        {
            if (null == obj1 && null == obj2)
            {
                return 0;
            }

            if (null == obj1 ||
                null == obj2)
            {
                return -1;
            }

            return obj1.CompareTo(obj2);
        }

        /// <summary>
        /// Returns whether obj1.CompareTo(obj2) except it will
        /// add some logic to check if obj1 is null vs obj2.
        /// </summary>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <returns></returns>
        public static int CompareToIncludeNull<E>(E obj1, E obj2)
            where E : IComparable<E>
        {
            if (null == obj1 && null == obj2)
            {
                return 0;
            }

            if (null == obj1 ||
                null == obj2)
            {
                return -1;
            }

            return obj1.CompareTo(obj2);
        }

        /// <summary>
        /// NOTE: This method does depend on Equals being implemented
        /// on the objects within the List. If this method is called
        /// on somethig with an IList[IEnumerable[string]]] you may get funky results as
        /// Equals is not implemented on the List object to check ever.
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="values1"></param>
        /// <param name="values2"></param>
        /// <returns></returns>
        public static bool EqualsIncludeNullList<E>(IList<E> values1 , IList<E> values2)
        {
            if (null == values1 && null == values2)
            {
                return true;
            }

            if (null == values1 ||
                null == values2)
            {
                return false;
            }

            if(values1.Count != values2.Count)
            {
                return false;
            }

            for(int i = 0; i < values1.Count; i++)
            {
                bool found = true;

                for( int j = 0; j < values2.Count; j++)
                {
                    if( true == EqualsIncludeNull(values1[i], values2[j]))
                    {
                        found = true;
                        break;
                    }
                }

                if (false == found)
                {
                    return found;
                }
            }

            return true;
        }

        /// <summary>
        /// If the object is NULL, it will return an
        /// empty string, otherwise it returns
        /// obj.ToString().
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string IsNullEmpty(object obj)
        {
            if (null == obj)
            {
                return string.Empty;
            }

            return obj.ToString();
        }

        /// <summary>
        /// Throws a HardFailureException with a message
        /// if the object is NULL.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="obj"></param>
        /// <param name="message"></param>
        public static void NullCheck(string name, object obj, string message = "")
        {
            if (null == obj)
            {
                if (string.IsNullOrEmpty(message))
                {
                    throw new HardFailureException(
                        "Object " + name + " is null.");
                }
                else
                {
                    throw new HardFailureException(
                        name + ": " + message);
                }

            }
        }

        /// <summary>
        /// Throws a HardFailureException if the object is NULL or
        /// whitespace.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="obj"></param>
        /// <param name="message"></param>
        public static void WhitespaceCheck(string name, string obj, string message = "")
        {
            if (true == String.IsNullOrWhiteSpace(obj))
            {
                if (string.IsNullOrEmpty(message))
                {
                    throw new HardFailureException(
                        "String " + name + " is null or whitespace.");
                }
                else
                {
                    throw new HardFailureException(
                        name + ": " + message);
                }
            }
        }

        /// <summary>
        /// Throws a HardFailureException if the index is less than 0.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="message"></param>
        public static void IndexCheck(int index, string message = "")
        {
            if (index < ZERO)
            {
                if (string.IsNullOrEmpty(message))
                {
                    throw new HardFailureException(
                       "Index cannot be < than 0. ");
                }
                else
                {
                    throw new HardFailureException(message);
                }
            }
        }

        private const string NULL = "NULL";
        private const int ZERO = 0;
    }
}
