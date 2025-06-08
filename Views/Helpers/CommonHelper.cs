using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CalendarWinUI3.Views.Helpers
{
    public static class CommonHelper
    {
        public static TEnum GetEnum<TEnum>(string text) where TEnum : struct
        {
            if (!typeof(TEnum).GetTypeInfo().IsEnum)
            {
                throw new InvalidOperationException("Generic parameter 'TEnum' must be an enum.");
            }

            if(text ==null)
                return default(TEnum);

            return (TEnum)Enum.Parse(typeof(TEnum), text);
        }
    }
}
