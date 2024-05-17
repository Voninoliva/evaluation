using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace btp.Models.Data
{
    public class ExportableDevis
    {
        public TypeTravaux typeTravaux;
        public List<DetailDevi> details;
        public double total;
        public ExportableDevis(){}
        public ExportableDevis(BtpContext context,TypeTravaux t,int iddevis){
            this.typeTravaux=t;
            string request = "select detail_devis.* from detail_devis join travaux on detail_devis.idtravaux=travaux.idtravaux";
            request+=" where travaux.idtypetravaux="+t.Idtypetravaux+" and iddevis="+iddevis;
            List<DetailDevi> d = context.DetailDevis
                                        .FromSqlRaw(request)
                                        .Include(d=>d.IdtravauxNavigation)
                                        .ThenInclude(d=>d.IduniteNavigation)
                                        .ToList();
            this.details=d;
                double prix = 0;
                foreach(DetailDevi de in d){
                    prix +=(double)(de.Pu*de.Quantite);
                }
                total=prix;
        }
        public static List<ExportableDevis> GetByIdDevis(BtpContext context,int iddevis){
            string request = "select distinct t.* from travaux tr  join type_travaux t on tr.idtypetravaux=t.idtypetravaux join detail_devis d on tr.idtravaux=d.idtravaux";
            request+=" where d.iddevis="+iddevis;
            List<TypeTravaux> ts = context.TypeTravauxes
                                            .FromSqlRaw(request).ToList();
            List<ExportableDevis> ex = new List<ExportableDevis>();                         
            foreach(TypeTravaux t in ts){
                    ex.Add(new ExportableDevis(context,t,iddevis));
            }
            return ex;
    }
}
}