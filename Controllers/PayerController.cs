using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using btp.Models.Data;
using btp.Models.includes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace btp.Controllers;
[TypeFilter(typeof(SessionClientFilter))]
public class PayerController : Controller
{
    private readonly BtpContext _context;

    public PayerController(BtpContext b)
    {
        _context = b;
    }

    public IActionResult Index(int id)
    {
        Devi d = _context.Devis.Where(d => d.Iddevis == id).FirstOrDefault();
        var retour = new
        {
            Devi = d,
            TotalPaye = d.GetResteAPayer(_context),
            CenseAPaye = d.GetResteCenseAPayer(_context)
        };
        return View(retour);
    }
    // TODO : mbola tsy vita ny mampiseho exception
    // public IActionResult AjouterPaiement(DateOnly date, double montant, int iddevis)
    // {
    //     // ato le tokony hisy verification zany
    //     try{
    //          PaiementDevi.Insert(_context, date, iddevis, montant);
        
    //     }catch(Exception e){
    //         TempData["montant"]=montant.ToString();
    //         TempData["date"]=date.ToString();
    //         TempData["log"]=e.Message;
    //     }
    //    return RedirectToAction("Index", "Payer", new { id = iddevis });
    // }
   
    public async Task<IActionResult>  AjouterPaiement(int iddevis, DateOnly date, double montant)
    {
        //  
        try{
            Console.WriteLine("ATOOO  AjouterPaiement");
            PaiementDevi.Insert(_context, date, iddevis, montant);
            //  "Payer/Index?id="+iddevis
            // return RedirectToAction("Index", "Payer", new { id = iddevis });
            return Json(new {url =Url.Action("Index","Client",new{id=iddevis})});
        }catch(Exception e){
            return Json(new{errors = e.Message} );
        }
        
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}
