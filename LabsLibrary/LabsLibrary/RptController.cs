using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabsLibrary
{
    public class RptController
    {
        public System.Data.DataTable GetDataTableFromXml(string Xml){
            return APC.Utility.Functions.Util.XmlToDataTable(Xml);
        }
    }
}
