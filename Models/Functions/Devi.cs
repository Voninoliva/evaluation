using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
// using AspNetCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace btp.Models.Data
{
    public partial class Devi
    {
        [NotMapped]
        public double? PaiementCenseEffectuees;
        [NotMapped]
        public double? PaiementFinalisees;
        [NotMapped]
        public double? PourcentagePayes;
        // ho an le vo i inserer
        public DateOnly GetDateFin(){
           DateOnly nouvelleDate =(DateOnly)this.DateDebutTravaux;
           nouvelleDate=nouvelleDate.AddDays((int)this.Duree);
            return nouvelleDate;
        }
        public static double GetPrixtravauxPourUnDevis(BtpContext context, int id)
        {
            return (double)context.Devis
                       .FromSqlRaw("SELECT SUM(pu * quantite) as taux_finition FROM travaux_des_maisons WHERE idtypemaison = {0} GROUP BY idtypemaison", id)
                       .Select(p => p.TauxFinition).FirstOrDefault();
        }
        // ho an ilay vo hi inserer
        public static double GetDureeTotal(BtpContext context, int id)
        {
            return (double)context.Devis
                                .FromSqlRaw("select sum(duree) as duree from travaux_des_maisons where idtypemaison=" + id)
                                 .Select(p => p.Duree).FirstOrDefault();
        }

        //    ho an le efa inserer
        public static double GetVraiPrixTotalDevis(BtpContext context, Devi devi)
        {
            string sql = "select sum(pu*quantite) as taux_finition from detail_devis where iddevis={0} group by iddevis";
            return (double)context.Devis.FromSqlRaw(sql, devi.Iddevis).Select(p => p.TauxFinition).FirstOrDefault();
        }
        public static double GetVraiPrixTotalDevisExport(BtpContext context, int devi)
        {
            string sql = "select sum(pu*quantite) as taux_finition from detail_devis where iddevis={0} group by iddevis";
            return (double)context.Devis.FromSqlRaw(sql, devi).Select(p => p.TauxFinition).FirstOrDefault();
        }
        // ho an ilay efa inserer
        public static double getVraiDureeTotalDevis(BtpContext context, Devi d)
        {
            string sql = "select sum(duree) as duree from detail_devis where iddevis=" + d.Iddevis;
            return (double)context.Devis
                               .FromSqlRaw(sql)
                                .Select(p => p.Duree).FirstOrDefault();
        }
        public void SetDateDebutTravaux(DateOnly d)
        {
            if (d.CompareTo(DateOnly.FromDateTime(DateTime.Now)) < 0)
            {
                throw new Exception("la date de construction est invalide");
            }
            this.DateDebutTravaux = d;
        }
        public static double CalculMontantTotal(double travaux, double taux)
        {
            double e = (travaux * taux) / 100;
            return travaux + e;
        }
        // TODO:tokony mbola hisy duree
        public static Devi GetWithoutException(BtpContext context, int idtypemaison, int idfinition, DateOnly date, int idclient)
        {
            try
            {
                Devi devi = new Devi();
                devi.SetDateDebutTravaux(date);
                devi.DateDebutTravaux = date;
                devi.Idtypemaison = idtypemaison;
                devi.MontantTotalTravaux = Devi.GetPrixtravauxPourUnDevis(context, (int)devi.Idtypemaison);
                devi.Idclient = idclient;
                devi.Idtypefinition = idfinition;
                devi.Duree = Devi.GetDureeTotal(context, idtypemaison);
                TypeFinition finition = (TypeFinition)context.TypeFinitions
                                        .Where(t => t.Idtypefinition == devi.Idtypefinition)
                                        .FirstOrDefault();
                devi.TauxFinition = finition.AugmentationPourcentage;
                devi.MontantTotal = Devi.CalculMontantTotal((double)devi.MontantTotalTravaux, (double)devi.TauxFinition);
                // navigations
                devi.IdclientNavigation = context.Clients.Where(c => c.Idclient == idclient).FirstOrDefault();
                devi.IdtypemaisonNavigation = context.TypeMaisons.Where(c => c.Idtypemaison == devi.Idtypemaison).FirstOrDefault();
                devi.IdtypefinitionNavigation = finition;
                return devi;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
        public static void InsertUnDetailDevis(BtpContext dbcontext, Devi devi)
        {
            try
            {
                List<TravauxDesMaison> travaux = dbcontext.TravauxDesMaisons
                                            .Where(t => t.Idtypemaison == devi.Idtypemaison)
                                            .ToList();
                foreach (TravauxDesMaison t in travaux)
                {
                    DetailDevi detail = new DetailDevi();
                    detail.Iddevis = devi.Iddevis;
                    detail.Idtravaux = t.Idtravaux;
                    detail.Idunite = t.Idunite;
                    detail.Quantite = t.Quantite;
                    detail.Pu = t.Pu;
                    detail.Duree = t.Duree;
                    Travaux tr = dbcontext.Travauxes.Where(p => p.Idtravaux == t.Idtravaux).FirstOrDefault();
                    detail.Designation = tr.Designation;
                    detail.Total = detail.Pu * detail.Quantite;
                    dbcontext.DetailDevis.Add(detail);
                    dbcontext.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        // TODO : le durée,tokony efa vita**
        public static void Insert(BtpContext dbcontext, Devi d)
        {
            using (var transaction = dbcontext.Database.BeginTransaction())
            {
                try
                {
                    Devi a_inserer = new Devi();
                    a_inserer.Idclient = d.Idclient;
                    a_inserer.Idtypemaison = d.Idtypemaison;
                    a_inserer.Idtypefinition = d.Idtypefinition;
                    a_inserer.TauxFinition = d.TauxFinition;
                    a_inserer.MontantTotalTravaux = d.MontantTotalTravaux;
                    a_inserer.DateDebutTravaux = d.DateDebutTravaux;
                    a_inserer.MontantTotal = d.MontantTotal;
                    a_inserer.Duree = d.Duree;
                    a_inserer.RefDevis="D00"+d.Idtypemaison+""+d.Lieu;
                    dbcontext.Devis.Add(a_inserer);
                    dbcontext.SaveChanges();
                    Devi devi = dbcontext.Devis.
                            Where(p => p.Iddevis == a_inserer.Iddevis).FirstOrDefault();
                    // le tehirizina le devis an le olona efa vita
                    List<TravauxDesMaison> travaux = dbcontext.TravauxDesMaisons
                                            .Where(t => t.Idtypemaison == devi.Idtypemaison)
                                            .ToList();

                    foreach (TravauxDesMaison t in travaux)
                    {
                        DetailDevi detail = new DetailDevi();
                        detail.Iddevis = devi.Iddevis;
                        detail.Idtravaux = t.Idtravaux;
                        detail.Idunite = t.Idunite;
                        detail.Quantite = t.Quantite;
                        detail.Pu = t.Pu;
                        detail.Duree = t.Duree;
                        Travaux tr = dbcontext.Travauxes.Where(p => p.Idtravaux == t.Idtravaux).FirstOrDefault();
                        detail.Designation = tr.Designation;
                        detail.Total = detail.Pu * detail.Quantite;
                        dbcontext.DetailDevis.Add(detail);
                        dbcontext.SaveChanges();
                    }
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                }
            }

        }
        //    ity ze datepaiement null
        public double GetResteAPayer(BtpContext context)
        {
            string request = "select coalesce(sum(montant),0) as montant from paiement_devis where iddevis=" + this.Iddevis + "  and date_paiement is not null";

            double montant = (double)context.PaiementDevis
                            .FromSqlRaw(request)
                            .Select(p => p.Montant).FirstOrDefault();
            return montant;
        }

        // ity le oe paiement efa nanoncena fa mbola tsy payé
        public double GetResteCenseAPayer(BtpContext context)
        {
            string request = "select coalesce(sum(montant),0) as montant from paiement_devis where iddevis=" + this.Iddevis + "  and date_prevue is not null";
            double montant = (double)context.PaiementDevis
                            .FromSqlRaw(request)
                            .Select(p => p.Montant).FirstOrDefault();
            return montant;
        }
        public static List<Devi> GetAllPagined(BtpContext context, int pageslide, int number)
        {
            List<Devi> liste = context.Devis
                                .Include(d => d.IdtypemaisonNavigation)
                                .Include(d => d.IdtypefinitionNavigation)
                                .Include(d => d.IdclientNavigation)
                                .Skip((pageslide - 1) * number)
                                .Take(number)
                                .ToList();
            for (int i = 0; i < liste.Count; i++)
            {
                liste[i].PaiementFinalisees = liste[i].GetResteAPayer(context);
                liste[i].PaiementCenseEffectuees = liste[i].GetResteCenseAPayer(context);
                liste[i].PourcentagePayes = (liste[i].PaiementCenseEffectuees*100)/liste[i].MontantTotal;
            }
            return liste;
        }
        public static double GetTotalPages(BtpContext context, int number)
        {
            double d = context.Devis
                                .ToList().Count;
            return (int)Math.Ceiling((double)d / number);
        }
        public static List<DetailDevi> GetAllForDetails(BtpContext context, int iddevis)
        {
            string request = "select * from detail_devis where iddevis=" + iddevis;
            return context.DetailDevis
                                        .FromSqlRaw(request)
                                        .Include(d => d.IdtravauxNavigation)
                                        .ThenInclude(d => d.IduniteNavigation)
                                        .ToList();
        }

        public static double GetTotal(BtpContext context, int iddevis)
        {
            string request = "select * from detail_devis where iddevis=" + iddevis;
            List<DetailDevi> d = context.DetailDevis
                                          .FromSqlRaw(request)
                                          .Include(d => d.IdtravauxNavigation)
                                          .ThenInclude(d => d.IduniteNavigation)
                                          .ToList();
            double rep = 0;
            foreach (DetailDevi de in d)
            {
                rep += (double)de.Total;
            }
            return rep;
        }
        // order by mois ity
        public static double[] GetMonhts(){
                 double [] rep = new double[12];
                 for(int i=1;i<=12;i++){
                    rep[i-1]=i;
                 }
                 return rep;
        }
        public static double GetMontantTotalForDashboard(BtpContext context){
            string request = "select sum(montant_total) as montant_total from devis";
            return (double)context.Devis.FromSqlRaw(request)
                                .Select(d => d.MontantTotal)
                                .FirstOrDefault();
        }
        public static double GetMontantTotalPaiement(BtpContext context){
            string request = "select sum(montant) as montant_total from paiement_devis";
            return (double)context.Devis.FromSqlRaw(request)
                                .Select(d => d.MontantTotal)
                                .FirstOrDefault();
        }
        public static int[] GetHistogrammeByYear(BtpContext context,int year){
            string request = "select COALESCE(montant_total,0) as montant_total from mois left join ( select extract(month from devis.date_insertion) as m,sum(montant_total) as montant_total from devis ";
            request+=" where extract(year from devis.date_insertion)="+year;
            request+=" group by extract(month from devis.date_insertion) ) as s on mois.month=s.m order by mois.month ";
              List<double?> d=context.Devis.FromSqlRaw(request)
                                .Select(d => d.MontantTotal).ToList();
            List<int> dNonnull = new List<int>();
            foreach(double d0 in d){
                dNonnull.Add((int)d0);
            }
            return dNonnull.ToArray();
        }
    }
}