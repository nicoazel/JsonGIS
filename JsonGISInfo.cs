using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace JsonGIS
{
    public class JsonGISInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "JsonGIS";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return null;
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("0e3f5e80-f345-4335-99dd-fac7e4893776");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "";
            }
        }
    }
}
