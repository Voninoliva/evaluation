using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using btp.Models;
using Rotativa.AspNetCore;
using btp.Models.Data;
using btp.Models.includes;

namespace btp.Controllers;

public class HomeController : Controller
{
    private readonly BtpContext _context;

    public HomeController(BtpContext c)
    {
        _context=c;
    }
    public IActionResult Index()
    {
        return View();
    }
    public IActionResult Connexion(string numero){
        try{
            Client client = Client.LogIn(_context,numero);
            HttpContext.Session.SetInt32("idclient",client.Idclient);
            return RedirectToAction("Index","Client");
        }catch(Exception e){
            TempData["login"]=e.Message;
            return RedirectToAction("Index","Home");
        }

    }
    public IActionResult ConnexionAdmin(string email,string mdp){
        try{
                Btp b =Btp.GetConnection(_context,email,mdp);
                HttpContext.Session.SetInt32("idadmin",b.Idbtp);
                return RedirectToAction("Index","Admin");
        }
        catch(Exception e){
            TempData["login"]=e.Message;
            return RedirectToAction("Admin","Home");
        }
    }
    public IActionResult Deconnection(){
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }
    public IActionResult Admin(){
        return View();
    }
    public IActionResult Export()
    {
        return new ViewAsPdf("~/Views/Home/Export.cshtml")
        {
            FileName = "stock.pdf"
        };
        // return View();
    }
    public IActionResult Reset(){
            SessionHelper.ResetDataBase(_context);
            HttpContext.Session.Clear();
       return RedirectToAction("Admin","Home");
    }
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
