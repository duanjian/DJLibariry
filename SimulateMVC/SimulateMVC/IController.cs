using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SimulateMVC
{
    interface IController
    {
        void Execute(HttpContext context);
    }
}
