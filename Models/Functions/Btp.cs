using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace btp.Models.Data
{
    public partial class Btp
    {
        public static Btp GetConnection(BtpContext c,string email,string mdp){
                Btp? btp = c.Btps
                            .Where(btp=>btp.Email==email)
                            .Where(btp=>btp.Mdp==mdp)
                            .FirstOrDefault();
                            if(btp==null){
                                throw new Exception("login incorrect");
                            }
                            return btp;
        }
    }
}