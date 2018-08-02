
using System.Collections.Generic;

namespace BrainCorp.Passwd.Common.Extensions
{
    public static class CollectionExtensions
    {
        public static bool ContainsAny<E>(this ICollection<E> collection, ICollection<E> collectionToCheck)
        {
            if( null == collection || 
                null == collectionToCheck ||
                collection.Count == 0 ||
                collectionToCheck.Count == 0)
            {
                return false;
            }

            foreach(E obj in collectionToCheck)
            {
                if(collection.Contains(obj))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
