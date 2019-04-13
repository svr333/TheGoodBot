using System.Collections.Generic;
using System.Text;

namespace TheGoodBot.Core.Extensions
{
    public static class ListToStringExt
    {
        public static string ReturnListAsString<T>(this List<T> list)
        {
            var sb = new StringBuilder();

            if (list.Count is 0) { return "N/A"; }

            for (int i = 0; i < list.Count; i++)
            {
                if (i == list.Count - 1) { sb.Append($"`{list[i]}`"); }
                else { sb.Append($"`{list[i]}`, "); }
            }

            return sb.ToString();
        }
    }
}
