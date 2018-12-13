using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccaProduction.Models
{
    public class IndividualButtonPartial
    {

        public String ButtonType { get; set; }
        public String Action { get; set; }
        public string Glyph { get; set; }
        public string Text { get; set; }


        public int? KandidatId { get; set; }

        public string ActionParameters
        {
            get
            {
                var param = new StringBuilder(@"/");
                if (KandidatId!= 0 && KandidatId!=null)
                {
                    param.Append(String.Format("{0}", KandidatId));
                }
                

                return param.ToString().Substring(0, param.Length);
            }
        }

    }
}
