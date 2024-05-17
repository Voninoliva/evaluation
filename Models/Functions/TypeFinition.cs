using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace btp.Models.Data
{
    public partial class TypeFinition
    {
        public static List<TypeFinition> getAllPagined(BtpContext context, int pageslide, int number)
        {
            return context.TypeFinitions
                            .Skip((pageslide - 1) * number)
                                .Take(number)
                                .ToList();
        }
    }
}