using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NoovinData.Models.Domin;
namespace NoovinData.Models.Repository
{
    public class Rep_Register
    {
        DB db = new DB();


        public IEnumerable<State> Get_State()
        {
            var state = db.States.OrderBy(a => a.State_Name).ToList();
            return state.AsEnumerable();
        }
    }
}