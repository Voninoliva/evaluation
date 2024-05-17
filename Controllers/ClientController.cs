using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using btp.Models.Data;
using btp.Models.includes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rotativa.AspNetCore;
namespace btp.Controllers
{
    [TypeFilter(typeof(SessionClientFilter))]
    public class ClientController : Controller
    {
        private readonly BtpContext _context;

        public ClientController(BtpContext _context)
        {
            this._context = _context;
        }
        public IActionResult Index(int? pageslide)
        {
                if(pageslide==null){
                    pageslide=1;
                }
                int a_afficher = 3;
                var retour = new{
                    Data = TypeMaison.GetPagined(_context, (int)pageslide,a_afficher),
                    TotalPage = TypeMaison.GetTotalPages(_context,a_afficher),
                    CurrentPage = (int)pageslide
                };
            return View(retour);
        }
        public IActionResult ConsultationDevis(int idtypemaison,int idfinition,DateOnly datedebuttravaux){
            try{
                int idclient = (int)HttpContext.Session.GetInt32("idclient");
            Devi d = Devi.GetWithoutException(_context,idtypemaison,idfinition,datedebuttravaux,idclient);
            var retour = new{
                Devi = d,
                String = JsonConvert.SerializeObject(d)
            };
            return  View(retour);
            }catch(Exception e){
                TempData["data"] = e.Message;
                TempData["idfinition"]=idfinition;
               Int32.Parse(TempData["idfinition"].ToString());
                TempData["date"] = datedebuttravaux.ToString();
                return RedirectToAction("Finitions","Client",new{id=idtypemaison});
            }   
        }
        public IActionResult DevisRequest(){
            return RedirectToAction("Index","Client");
        }
        public IActionResult Finitions(string id){
            Console.WriteLine("IDDDD = "+id); 

            int selected =Int32.Parse(id); 
            var retour = new{
                Finitions = _context.TypeFinitions.ToList(),
                Id = selected
            };
            return View(retour);
        }
        public IActionResult Confirmer(string data ){
           Devi consultation = JsonConvert.DeserializeObject<Devi>(data);
            Devi.Insert(_context,consultation);
            return RedirectToAction("All","Client");
        }
        public IActionResult All(int? pageslide){
            if(pageslide==null){
                pageslide=1;
            }
            int idclient = (int)HttpContext.Session.GetInt32("idclient");
            int number = 5;
            var retour = new{
                Data = Client.GetPagined(_context,idclient, (int)pageslide,number),
                CurrentPage = pageslide,
                TotalPage = Client.GetTotalPages(_context,idclient,number)
            };
            return View(retour);
        }
        // le get total ity no ovaina rehefa tokony 
        public IActionResult ExporterPdf(int id){
            List<DetailDevi> devis =Devi.GetAllForDetails(_context,id);
            string request = "select * from paiement_devis where iddevis = "+id;
            Devi d = _context.Devis.Where(d=>d.Iddevis==id).Include(d=>d.IdtypefinitionNavigation)
                                                                .Include(d=>d.IdclientNavigation)
                                                                .Include(d=>d.IdtypemaisonNavigation)
                                                                
                                                            .FirstOrDefault();
            List<PaiementDevi> paiements = _context.PaiementDevis
                                            .FromSqlRaw(request).ToList();
            double sommePayes = 0;
            foreach(PaiementDevi ds in paiements){
                sommePayes +=(double)ds.Montant;
            }
            var retour = new{
                MontantPayes = sommePayes,
                Paiements = paiements,
                Devi = devis,
                Total = Devi.GetTotal(_context,id),
                Data = d,
                MontantTotal = Devi.CalculMontantTotal(Devi.GetTotal(_context,id), (double)d.IdtypefinitionNavigation.AugmentationPourcentage)
            };
            return new ViewAsPdf("~/Views/Client/ExporterPdf.cshtml",retour){
                FileName = "devis_"+id+".pdf"
            };
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}