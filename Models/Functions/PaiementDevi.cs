using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace btp.Models.Data
{
    public partial class PaiementDevi
    {
        public double setMontant(double m){
            if(m<0){
                throw new Exception("montant négatif");
            }
            this.Montant=m;
            return m;
        }
        public static void Insert(BtpContext context,DateOnly date,int iddevis,double montant){
                try{
                    Devi d = context.Devis.Where(d => d.Iddevis == iddevis).FirstOrDefault();
                    if(d.GetResteCenseAPayer(context)+montant>d.MontantTotal){
                        throw new Exception("mihoatra le vola ! ");
                    }
                    PaiementDevi p = new PaiementDevi();
                    p.Iddevis=iddevis;
                    p.DatePrevue=date;
                    p.setMontant(montant);
                    context.PaiementDevis.Add(p);
                    context.SaveChanges();
                }catch(Exception e){
                        throw new Exception(e.Message);
                }
                
        }
    }
}