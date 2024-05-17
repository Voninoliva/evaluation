using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace btp.Models.Data
{
    public partial class Travaux
    {
        public static List<Travaux> getAllPagined(BtpContext context, int pageslide, int number)
        {
            return context.Travauxes
                            .Include(d => d.IduniteNavigation)
                            .Skip((pageslide - 1) * number)
                                .Take(number)
                                .ToList();
        }
        public static void Update(BtpContext context,int id,double pu){
            string request = "select * from travaux_des_maisons where idtravaux = "+id;
            Travaux t = context.Travauxes.Where(d=>d.Idtravaux==id).FirstOrDefault();
            t.Pu=pu;
            context.Travauxes.Update(t);
            context.SaveChanges();
            List<TravauxDesMaison> tm = context.TravauxDesMaisons
                                        .FromSqlRaw(request).ToList();
            for(int i=0;i<tm.Count;i++){
                tm[i].Pu=t.Pu;
                tm[i].Total=tm[i].Pu*tm[i].Quantite;
                context.TravauxDesMaisons.Update(tm[i]);
                context.SaveChanges();
            }
        }
    }
}