using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CafeLib.Identity.Sqlite.Controllers
{
    public class ControllerBase : Controller
    {
        /// <summary>
        /// ApiControllerBase constructor.
        /// </summary>
        /// <param name="serviceProvider">service provider</param>
        protected ControllerBase(IServiceProvider serviceProvider)
        {
        }
    }
}
