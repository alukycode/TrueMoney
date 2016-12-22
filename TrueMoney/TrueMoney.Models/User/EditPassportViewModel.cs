using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TrueMoney.Models.Basic;

namespace TrueMoney.Models.User
{
    public class EditPassportViewModel
    {
        public PassportModel Passport { get; set; }

        public HttpPostedFileBase Photo { get; set; }

        public string PhotoFilename { get; set; }
    }
}
