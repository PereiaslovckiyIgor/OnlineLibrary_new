using System.Linq;
using System.Security.Claims;

namespace LibOnline.Models.General
{
    public class Utils
    {
        public int GetUserIdByUserName(string UserName) {
                int idUser;
                using (ApplicationContext db = new ApplicationContext())
                    idUser = db.Users.First(u => u.Login == UserName).Id;
                return idUser;
            }//GetUserIdByUserName
    }//Utils
}
