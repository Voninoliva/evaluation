using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using btp.Models.Data;
using btp.Models.includes;
using Microsoft.EntityFrameworkCore;

namespace btp.Models.Csv
{
    public partial class MaisonTravaux
    {
        public MaisonTravaux() { }
        public MaisonTravaux(string[] valeurs)
        {
            try
            {
                this.TypeMaison = valeurs[0].Trim();
                this.Description = valeurs[1].Trim();
                this.Surface = double.Parse(valeurs[2].Trim());
                this.CodeTravaux = valeurs[3].Trim();
                this.TypeTravaux = valeurs[4].Trim();
                this.Unite = valeurs[5].Trim();
                this.Pu = Double.Parse(valeurs[6].Trim());
                this.Quantite = Double.Parse(valeurs[7].Trim());
                this.DureeTravaux = Double.Parse(valeurs[8].Trim());
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async static Task<List<string[]>> GetFromCsv(IFormFile file)
        {
            try
            {
                List<string[]> listes = new List<string[]>();
                if (file != null && file.Length > 0)
                {
                    // Accès au nom du fichier
                    string fileName = Path.GetFileName(file.FileName);
                    // Accès au contenu du fichier
                    using (var stream = new MemoryStream())
                    {
                        await file.CopyToAsync(stream);
                        listes = SessionHelper.GetDataFromCsvFile(fileName, ",");
                    }
                }
                return listes;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        // TODO : le misy misy paramètre
        public static void InsertUnite(BtpContext context)
        {
            try
            {
                string request = "insert into unites(nom_unite) select distinct unite from maison_travaux";
                context.Database.ExecuteSqlRaw(request);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
        // ity le amboarina ra hoe la duree le ligne 1 iny
        public static void InsertTypeMaisons(BtpContext context)
        {
            try
            {
                string request = "insert into type_maisons(nom_maison,descri,surface,duree) ";
                request += " select distinct type_maison,description,surface,duree_travaux from maison_travaux";
                context.Database.ExecuteSqlRaw(request);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
        public static void InsertTravaux(BtpContext context)
        {
            try
            {
                // string request = "insert into travaux(numero,designation,idunite,pu) ";
                // request += " select distinct code_travaux,type_travaux,idunite,pu from v_maisons_unites";
                string request = "insert into travaux(numero,designation,idunite,pu) SELECT DISTINCT ON (code_travaux)code_travaux,type_travaux,idunite,pu FROM  v_maisons_unites ORDER BY code_travaux DESC;";
                context.Database.ExecuteSqlRaw(request);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
      
        public static void InsertTravauxDesMaisons(BtpContext context)
        {
            try
            {
                string request = "insert into travaux_des_maisons (idtypemaison,idtravaux,idunite,quantite,pu,total,duree,designation)";
                request += " select distinct idtypemaison ,idtravaux,idunite,quantite,pu,total,duree_travaux,designation ";
                // string req = " select distinct  on(numero,designation)idtypemaison ,idtravaux,idunite,quantite,pu,total,duree_travaux,designation ";
                // request+=req;
                request += " from  v_maisons_unites_travaux";
                
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
                // le chauqe ligne de tehirizina ny erreur
                foreach (string[] une_ligne in listes)
                {
                    try
                    {
                        MaisonTravaux m = new MaisonTravaux(une_ligne);
                        context.MaisonTravauxes.Add(m);
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
                        MaisonTravaux.InsertUnite(context);
                        MaisonTravaux.InsertTypeMaisons(context);
                        MaisonTravaux.InsertTravaux(context);
                        MaisonTravaux.InsertTravauxDesMaisons(context);
                    }
                    catch (Exception e)
                    {
                        secondexception = true;
                        errors += "maison insertion base : " + e.Message + "\n";
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