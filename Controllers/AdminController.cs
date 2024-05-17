using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using btp.Models.Csv;
using btp.Models.Data;
using btp.Models.includes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
namespace btp.Controllers
{
    [TypeFilter(typeof(SessionAdminFilter))]
    public class AdminController : Controller
    {
        private readonly BtpContext _context;

        public AdminController(BtpContext c)
        {
            _context = c;
        }

        public IActionResult Index(int? pageslide)
        {
            if (pageslide == null)
            {
                pageslide = 1;
            }
            int number = 5;
            var reponse = new
            {
                Data = Devi.GetAllPagined(_context, (int)pageslide, number),
                TotalPage = Devi.GetTotalPages(_context, number),
                CurrentPage = pageslide
            };
            return View(reponse);
        }
        public IActionResult Details(int id)
        {
            List<DetailDevi> devis = Devi.GetAllForDetails(_context, id);
            double totals = 0;
            Devi da = _context.Devis.Where(d=>d.Iddevis==id).Include(d=>d.IdtypefinitionNavigation).FirstOrDefault();
            foreach (DetailDevi d in devis)
            {
                totals += (double)d.Total;
            }
            var reponse = new
            {
                Devi = devis,
                Total = totals,
                Data = da,
                MontantTotal = Devi.CalculMontantTotal(Devi.GetTotal(_context,id), (double)da.IdtypefinitionNavigation.AugmentationPourcentage)
            };
            return View(reponse);
        }

        // le resaka import
        public IActionResult ImportM()
        {
            return View();
        }
        public async Task<IActionResult> ExecuteImportM(IFormFile maison, IFormFile devis)
        {
            try
            {
                // le header efa tsy ao anatin le donnees intsony
                string delimiter = ",";
                List<string[]> maisons_et_travaux = new List<string[]>();
                List<string[]> devises = new List<string[]>();
                if (maison != null && maison.Length > 0)
                {
                    string fileName = Path.GetFileName(maison.FileName);
                    using (var stream = new MemoryStream())
                    {
                        await maison.CopyToAsync(stream);
                        maisons_et_travaux = SessionHelper.GetDataFromCsvFile(fileName, delimiter);
                    }
                }
                MaisonTravaux.Insert(_context, maisons_et_travaux);
                if (devis != null && devis.Length > 0)
                {
                    string fileName = Path.GetFileName(devis.FileName);
                    using (var stream = new MemoryStream())
                    {
                        await devis.CopyToAsync(stream);
                        devises = SessionHelper.GetDataFromCsvFile(fileName, delimiter);
                    }
                }
                DevisCsv.Insert(_context, devises);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                TempData["Erreur"] = e.Message;
            }
            return RedirectToAction("ImportM", "Admin");
        }
        public IActionResult ImportP()
        {
            return View();
        }
        public async Task<IActionResult> ExecuteImportP(IFormFile paiement)
        {
            try
            {
                List<string[]> l =new List<string[]>();
                if (paiement != null && paiement.Length > 0)
                {
                    string fileName = Path.GetFileName(paiement.FileName);
                    using (var stream = new MemoryStream())
                    {
                        await paiement.CopyToAsync(stream);
                      l=SessionHelper.GetDataFromCsvFile(fileName, ",");
                    }
                    PaiementCsv.Insert(_context,l);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                TempData["Erreur"] = e.Message;
            }
            return RedirectToAction("ImportP", "Admin");
        }


        public IActionResult Dashboard(int? annee){
            if(annee==null){
                var retour = new{
                    DevisTotal = Devi.GetMontantTotalForDashboard(_context),
                    DevisPayer = Devi.GetMontantTotalPaiement(_context),
                     Mois = Devi.GetMonhts(),
                    Data = Devi.GetHistogrammeByYear(_context, (int)2024),
                    Annee = 2024,
                };
                return View(retour);
            }else{
                var retour = new{
                    Mois = Devi.GetMonhts(),
                    Data = Devi.GetHistogrammeByYear(_context, (int)annee),
                    Annee = annee,
                    DevisTotal = Devi.GetMontantTotalForDashboard(_context),
                    DevisPayer = Devi.GetMontantTotalPaiement(_context)
                };
                Console.WriteLine("REOTUR   = "+retour.Annee);
                return View(retour);
            }
        }
        public IActionResult Filtrer(int annee){
                
            return View("Dashboard",new{annee=annee});
        }
        // le resaka liste sy update
        public IActionResult ListTravaux(int?pageslide){
            if(pageslide==null){
                pageslide=1;
            }
            var retour = new{
                Data = Travaux.getAllPagined(_context, (int)pageslide,5),
                TotalPage = SessionHelper.GetTotalPages<Travaux>(_context,5),
                CurrentPage = pageslide
            };
            return View(retour);
        }
        public IActionResult ListFinitions(int? pageslide){
            if(pageslide==null){
                pageslide=1;
            }
            var retour = new{
                Data = TypeFinition.getAllPagined(_context, (int)pageslide,5),
                TotalPage = SessionHelper.GetTotalPages<TypeFinition>(_context,5),
                 CurrentPage = pageslide
            };
            return View(retour);
        }
        public IActionResult UpdateTravaux(int id){
                Travaux t = _context.Travauxes.Where(d=>d.Idtravaux==id).FirstOrDefault();
                return View(t);
        }
        public IActionResult UpdateFinition(int id){
            TypeFinition f = _context.TypeFinitions.Where(d=>d.Idtypefinition==id).FirstOrDefault();
            return View(f);
        }
        public IActionResult ExecuteUF(int id,double pourcentage){
            TypeFinition f = _context.TypeFinitions.Where(d=>d.Idtypefinition==id).FirstOrDefault();
            f.AugmentationPourcentage=pourcentage;
            _context.TypeFinitions.Update(f);
            _context.SaveChanges();
            return RedirectToAction("ListFinitions","Admin");
        }
        public IActionResult ExecuteUT(int id,double pu){
           Travaux.Update(_context,id,pu);
           return RedirectToAction("ListTravaux","Admin");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}