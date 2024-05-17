using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace btp.Models.Data
{
    public partial class TypeMaison
    {
        [NotMapped]
        public double? Prix;
        [NotMapped]
        public string formattedPrix;
        public static List<TypeMaison> GetPagined(BtpContext context, int pageslide, int number)
        {
            List<TypeMaison> l = context.TypeMaisons
                            // .Include(t => t.TravauxDesMaisons)
                            .Skip((pageslide - 1) * number)
                            .Take(number)
                            .ToList();
            for (int i = 0; i < l.Count; i++)
            {
                l[i].Prix = Devi.GetPrixtravauxPourUnDevis(context, l[i].Idtypemaison);
                if(l[i].Prix!=null){
                    double e = (double)l[i].Prix;
                    // le 0.00 le .00 no miteny ny chiffre apres la virgule
                    l[i].formattedPrix = e.ToString("#,##0.00",CultureInfo.CurrentCulture);
                }
            }
            return l;
        }
        public static int GetTotalPages(BtpContext context, int number)
        {
            int i = context.TypeMaisons
                    // .Include(t => t.TravauxDesMaisons)
                    .ToList().Count;
            i = (int)Math.Ceiling((double)i / number);
            return i;
        }

    }
}