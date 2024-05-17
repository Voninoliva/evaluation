using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using btp.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace btp.Models.Csv
{
    public partial class PaiementCsv
    {
        // le resaka references paiement sy devis mbola mila amboarina de le formuaire ana insert devi koa
        // le formattage an le numero
        public PaiementCsv() { }
        public PaiementCsv(string[] valeurs)
        {
            try
            {
                this.RefDevis = valeurs[0].Trim();
                this.RefPaiement = valeurs[1].Trim();
                this.DatePaiement = DateOnly.Parse(valeurs[2].Trim());
                this.Montant = Double.Parse(valeurs[3].Trim());
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
        public static void InsertPaiement(BtpContext context){
            try{
                string request = "insert into paiement_devis(iddevis,date_prevue,montant,ref_paiement) ";
                request+=" select  distinct iddevis,date_paiement,montant,ref_paiement from  v_paiement_devis ";
                request +=" where ref_paiement not in (select distinct ref_paiement from paiement_devis) ";
                context.Database.ExecuteSqlRaw(request);
                context.SaveChanges();
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
                        PaiementCsv p = new PaiementCsv(une_ligne);
                        context.PaiementCsvs.Add(p);
                        context.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        firstException = true;
                        errors += "ligne " + i + " : " + e.Message + " \n";
                    }
                    i++;
                }
                if(firstException==false){
                    try{

                            PaiementCsv.InsertPaiement(context);
                    } catch (Exception e)
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
                }else{
                    transaction.Rollback();
                    throw new Exception(errors);
                }
            }
        }

    }
}