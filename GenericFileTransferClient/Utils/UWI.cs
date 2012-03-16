using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GenericFileTransferClient
{
    public static class UWI
    {
        public static Dictionary<string, string> ParseUWIAlberta(string uwi)
        {
            Dictionary<string, string> tempList = new Dictionary<string, string>();
            string tempUwi = uwi.ToUpper();

            Regex wellIdent = new Regex(@"\d{2,3}/");
            Regex wellLegalSub = new Regex(@"^\d{2}-");
            //same as legal sub but for clarity
            //will probably change it to be same though
            Regex wellSection = new Regex(@"^\d{2}-");
            Regex wellTownship = new Regex(@"^\d{3}-");
            Regex wellRange = new Regex(@"^\d{2}");

            Regex wellW4 = new Regex(@"^W\d/\d{2}");

            if (wellIdent.IsMatch(tempUwi))
            {
                tempList.Add("wellIdent", "1" + wellIdent.Match(tempUwi).Value);
                tempUwi = wellIdent.Replace(tempUwi, "");

                if (wellLegalSub.IsMatch(tempUwi))
                {
                    tempList.Add("wellLegalSub", wellLegalSub.Match(tempUwi).Value.Replace("-", ""));
                    tempUwi = wellLegalSub.Replace(tempUwi, "");

                    if (wellSection.IsMatch(tempUwi))
                    {
                        tempList.Add("wellSection", wellSection.Match(tempUwi).Value.Replace("-", ""));
                        tempUwi = wellSection.Replace(tempUwi, "");

                        if (wellTownship.IsMatch(tempUwi))
                        {
                            tempList.Add("wellTownShip", wellTownship.Match(tempUwi).Value.Replace("-", ""));
                            tempUwi = wellTownship.Replace(tempUwi, "");

                            if (wellRange.IsMatch(tempUwi))
                            {
                                tempList.Add("wellRange", wellRange.Match(tempUwi).Value);
                                tempUwi = wellRange.Replace(tempUwi, "");

                                if (wellW4.IsMatch(tempUwi))
                                {
                                    tempList.Add("wellMeridian", wellW4.Match(tempUwi).Value);
                                    tempUwi = wellW4.Replace(tempUwi, "");

                                }
                            }
                        }
                    }
                }
            }

            if (tempList.Count == 6)
            {
                return tempList;
            }
            else
            {
                return new Dictionary<string,string>();
            }

        }
    }
}
