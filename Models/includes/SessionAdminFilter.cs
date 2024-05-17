using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace btp.Models.includes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class SessionAdminFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var idUser = context.HttpContext.Session.GetInt32("idadmin");
        if (!idUser.HasValue)
        {
            context.Result = new RedirectToActionResult("Admin", "Home", null); 
        }else{
              context.HttpContext.Session.SetString("layout","admin");
        }
      
    }
    public void OnActionExecuted(ActionExecutedContext context)
    {
        
    }
}


