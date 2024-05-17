using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using btp.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace btp.Models.Csv
{
    public partial class DevisCsv
    {
        public DevisCsv() { }
        public DevisCsv(string[] valeurs)
        {
            try
            {
                this.Client = valeurs[0].Trim();
                this.RefDevis = valeurs[1].Trim();
                this.TypeMaison = valeurs[2].Trim();
                this.Finition = valeurs[3].Trim();
                string taux = valeurs[4].Trim().Replace("%", "");
                this.TauxFinition = Double.Parse(taux);
                this.DateDevis = DateOnly.Parse(valeurs[5].Trim());
                this.DateDebut = DateOnly.Parse(valeurs[6].Trim());
                this.Lieu = valeurs[7].Trim();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }


        }
        public static void InsertClient(BtpContext context)
        {
            try
            {
                string request = " insert into clients(numero) select distinct client from devis_csv";
                context.Database.ExecuteSqlRaw(request);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static void InsertFinitions(BtpContext context)
        {
            try
            {
                string request = "insert into type_finitions(nom_finition,augmentation_pourcentage) ";
                request += " select distinct finition,taux_finition from devis_csv ";
                context.Database.ExecuteSqlRaw(request);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        // le insert detail mila kitihana rehefa ovaina asina duree ity
        public static void InsertDevis(BtpContext context)
        {
            try
            {
                string request = "insert into devis(idclient,idtypemaison,idtypefinition,taux_finition,date_insertion,date_debut_travaux,ref_devis,lieu,duree) ";
                request += " select distinct idclient,idtypemaison,idtypefinition,augmentation_pourcentage,date_devis,date_debut,ref_devis,lieu,duree";
                request += " from v_pour_devis ";
                context.Database.ExecuteSqlRaw(request);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        // boucle anaty boucles
        public static void InsertDetailDevis(BtpContext context)
        {
            try
            {
                List<Devi> devises = context.Devis.ToList();

                foreach (Devi d in devises)
                {
                    Devi intermediaire =d;
                    Devi.InsertUnDetailDevis(context, d);
                    intermediaire.MontantTotalTravaux=Devi.GetVraiPrixTotalDevis(context,d);
                    // intermediaire.Duree = Devi.getVraiDureeTotalDevis(context,d);
                    intermediaire.MontantTotal = Devi.CalculMontantTotal((double)intermediaire.MontantTotalTravaux, (double)d.TauxFinition);
                    context.Devis.Update(intermediaire);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static void Insert(BtpContext context, List<string[]> listes)
        {
            string errors = "";
            int i = 1;
            bool firstException = false;
            bool secondexception = false;
            using (var transaction = context.Database.BeginTransaction())
            {
                foreach (string[] une_ligne in listes)
                {
                    try
                    {
                        DevisCsv devis = new DevisCsv(une_ligne);
                        context.DevisCsvs.Add(devis);
                        context.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        firstException = true;
                        errors += "ligne " + i + " : " + e.Message + " \n";
                    }
                    i++;
                }
                if (firstException == false)
                {
                    try
                    {
                        DevisCsv.InsertClient(context);
                        DevisCsv.InsertFinitions(context);
                        DevisCsv.InsertDevis(context);
                        DevisCsv.InsertDetailDevis(context);
                    }
                    catch (Exception e)
                    {
                        secondexception = true;
                        errors += "insertion base : " + e.Message + "\n";
                    }
                    finally
                    {
                        if (secondexception == true)
                        {
                            transaction.Rollback();
                            throw new Exception(errors);
                        }
                        else
                        {
                            transaction.Commit();
                        }
                    }
                }
                else
                {
                    transaction.Rollback();
                    throw new Exception(errors);
                }
            }
        }
    }
}