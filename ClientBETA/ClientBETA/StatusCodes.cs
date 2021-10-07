using System;

namespace ClientBETA
{
    class StatusCodes
    {
        static string rsp0 = "Wrong Account";
        static string rsp1 = "Logged In";
        static string rsp2 = "Username Taken";
        static string rsp3 = "Account created";
        static string rsp_ukn = "Unknown Error";
        public static string response_codes(string code)
        {
            if (String.Equals(code, "0", StringComparison.InvariantCultureIgnoreCase))
            {
                return rsp0;
            }
            else if (String.Equals(code, "1", StringComparison.InvariantCultureIgnoreCase))
            {
                return rsp1;
            }
            else if (String.Equals(code, "2", StringComparison.InvariantCultureIgnoreCase))
            {
                return rsp2;
            }
            else if (String.Equals(code, "3", StringComparison.InvariantCultureIgnoreCase))
            {
                return rsp3;
            }
            else
            {
                return rsp_ukn;
            }
        }
    }
}
