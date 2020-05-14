using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Gatekeeper.Storage
{
    public class GtkConstants
    {
        public const string LOXY_CATEGORY = "Gatekeeper";
        public const string DM_GUEST_ROLE_NAME = "DM_GUEST";
        
        //public class Validation
        //{
        //    public const int Code_Length_Min = 1;
        //    public const int Code_Length_Max = 25;

        //    public const int Description_Length_Min = 1;
        //    public const int Description_Length_Max = 255;

        //    public const int DisplayName_Length_Min = 3;
        //    public const int DisplayName_Length_Max = 30;
        //}
        
        public class MenuCodes
        {
            public const string User = "dm_user";
            public const string Admin = "dm_admin";
            public const string AppNavigation = "dm_app";
        }

        public class CredentialIds
        {
            public const string Admin = "admin";
        }
    }
}