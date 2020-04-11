using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class SignUpMV
    {
        
        public Nullable<int> SchoolId { get; set; }
        public string PlayerName { get; set; }
        public Nullable<int> PlayerSex { get; set; }
        public int PlayerBirthday { get; set; }
        public string PlayerIdCard { get; set; }
        public string PlayerPrize { get; set; }
        public string PlayerDeclaration { get; set; }
        public int ParentId { get; set; }
        public string ParentName { get; set; }
        public string ParentEmail { get; set; }
        public string ParentPhone { get; set; }
        public int PictureId { get; set; }        
        public int PictureTypeId { get; set; }
        
        public string PerformanceName2 { get; set; }

        public string PerformanceName3 { get; set; }

    }
}
