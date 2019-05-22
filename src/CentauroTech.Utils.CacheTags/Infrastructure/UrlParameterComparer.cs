using System.Collections.Generic;

namespace CentauroTech.Utils.CacheTags
{
    
    public class UrlParameterComparer : IEqualityComparer<string>
    {
        public bool Equals(string x, string y)
        {
            return x.ToUpper().Equals(y.ToUpper());
        }

        public int GetHashCode(string obj)
        {
            return obj.GetHashCode();
        }
 }
}