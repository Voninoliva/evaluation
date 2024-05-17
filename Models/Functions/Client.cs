using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace btp.Models.Data
{
    public partial class Client
    {
        public static bool ISNumberValidFormat(string numero){
            // oe miatomboka amin'ny 0 ireo de 7 chiffres ny apres 
            bool regerx = Regex.IsMatch(numero,@"^(033|034`|038)\d\d{7}$");
            return regerx;
        }
        public static Client LogIn(BtpContext context, string numero)
        {
            Client? client = context.Clients
                                    .Where(c => c.Numero == numero)
                                    .FirstOrDefault();
            if (client == null)
            {
                Client d = new Client();
                d.Numero=numero;
                context.Clients.Add(d);
                context.SaveChanges();
                client=d;
            }
            return client;
        }
        public static List<Devi> GetPagined(BtpContext context,int idclient,int pageslide,int number){

            List<Devi> devis = context.Devis
                                .Where(d=>d.Idclient==idclient)
                                .Include(d=>d.IdtypemaisonNavigation)
                                .Include(d=>d.IdtypefinitionNavigation)
                                .Skip((pageslide - 1) * number)
                                .Take(number)
                                .ToList();
                                return devis;
        }
        public static double GetTotalPages(BtpContext context,int idclient,int number){
                           double d =  context.Devis
                                .Where(d=>d.Idclient==idclient)
                                .ToList().Count;
                                return (int)Math.Ceiling((double)d / number);
        }
    }
}