using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace JsonGIS
{
    public class JsonGISComponent : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public JsonGISComponent()
          : base("BlackBear", "GeoBear",
              "exports a polyline as a Esri Json gis file.",
              "Extra", "GeoBear")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            // Use the pManager object to register your input parameters.
            // You can often supply default values when creating parameters.
            // All parameters must have the correct access type. If you want 
            // to import lists or trees of values, modify the ParamAccess flag.
            pManager.AddPointParameter("linePoints", "lP", "points that compose the polyline organized in a tree", GH_ParamAccess.tree);
            pManager.AddTextParameter("fields", "f", "list of Fields for each geometry. This should not be a datatree but a simple list", GH_ParamAccess.list);
            pManager.AddGenericParameter("attributes", "a", "attributes for each geometry. this should be a dataTree matching the linePoints input, and fields indicies", GH_ParamAccess.tree);
            
            //pManager.AddPlaneParameter("Plane", "P", "Base plane for spiral", GH_ParamAccess.item, Plane.WorldXY);
            //pManager.AddNumberParameter("Inner Radius", "R0", "Inner radius for spiral", GH_ParamAccess.item, 1.0);
            //pManager.AddNumberParameter("Outer Radius", "R1", "Outer radius for spiral", GH_ParamAccess.item, 10.0);
            //pManager.AddIntegerParameter("Turns", "T", "Number of turns between radii", GH_ParamAccess.item, 10);

            // If you want to change properties of certain parameters, 
            // you can use the pManager instance to access them by index:
            //pManager[0].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            // Use the pManager object to register your output parameters.
            // Output parameters do not have default values, but they too must have the correct access type.
            //pManager.AddCurveParameter("Spiral", "S", "Spiral curve", GH_ParamAccess.item);
            pManager.AddTextParameter("geoJSON", "gJSON", "compact geoJson discription of the geometry and data. this can be written to a json file with the WriteGeojson Component", GH_ParamAccess.item);
            pManager.AddTextParameter("readable", "rj", "readable geoJson with indents, for human legablity and review of results", GH_ParamAccess.item);


            // Sometimes you want to hide a specific parameter from the Rhino preview.
            // You can use the HideParameter() method as a quick way:
            //pManager.HideParameter(0);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // First, we need to retrieve all data from the input parameters.
            // We'll start by declaring variables and assigning them starting values.
                    //Plane plane = Plane.WorldXY;
                    //double radius0 = 0.0;
                    //double radius1 = 0.0;
                    //int turns = 0;
            DataTree<Point3d> polylinePoints = new DataTree<Point3d>();
            List<string> fields = new List<string>();
            DataTree<Object> attributes = new DataTree<Object>();

            // Then we need to access the input parameters individually. 
            // When data cannot be extracted from a parameter, we should abort this method.
                    //if (!DA.GetData(0, ref plane)) return;
                    //if (!DA.GetData(1, ref radius0)) return;
                    //if (!DA.GetData(2, ref radius1)) return;
                    //if (!DA.GetData(3, ref turns)) return;
            if (!DA.GetData(0, ref polylinePoints)) return;
            if (!DA.GetData(1, ref fields)) return;
            if (!DA.GetData(2, ref attributes)) return;

            // We should now validate the data and warn the user if invalid data is supplied.
                    //if (radius0 < 0.0){
                    //    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Inner radius must be bigger than or equal to zero");
                    //    return;}
                    //if (radius1 <= radius0){
                    //    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Outer radius must be bigger than the inner radius");
                    //    return;}
                    //if (turns <= 0){
                    //    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Spiral turn count must be bigger than or equal to one");
                    //    return;}

            //Create working variables, not outputs or inputs but middle men
            Dictionary<int, Object> geoDict = new Dictionary<int, Object>();

            // We're set to create the spiral now. To keep the size of the SolveInstance() method small, 
            // The actual functionality will be in a different method:
            //Curve spiral = CreateSpiral(plane, radius0, radius1, turns);



            //Basic esriJSON headder info
            Dictionary<string, Object> geoDict = new Dictionary<string, Object>();
            geoDict.Add("displayFieldName", " ");

            Dictionary<string, string> fieldAliasDic = new Dictionary<string, string>();

            foreach (string field in Fields)
            {
                fieldAliasDic.Add(field, field);
            }
            geoDict.Add("fieldAliases", fieldAliasDic);

            geoDict.Add("geometryType", "esriGeometryPolyline");
            Dictionary<string, int> sr = new Dictionary<string, int>() { { "wkid", 102729 }, { "latestWkid", 2272 } };
            geoDict.Add("spatialReference", sr);


            List<Dictionary<string, string>> fieldsList = new List<Dictionary<string, string>>();

            foreach (var item in Fields.Select((Value, Index) => new { Value, Index }))
            {
                Dictionary<string, string> fieldTypeDict = new Dictionary<string, string>();


                fieldTypeDict.Add("name", item.Value.ToString());

                var typeItem = Values.Branch(Values.Paths[0])[item.Index];

                if (typeItem is int)
                {
                    fieldTypeDict.Add("type", "esriFieldTypeInteger");
                }
                else if (typeItem is long
                      || typeItem is ulong
                      || typeItem is float
                      || typeItem is double
                      || typeItem is decimal)
                {
                    fieldTypeDict.Add("type", "esriFieldTypeDouble");
                }
                else
                {
                    fieldTypeDict.Add("type", "esriFieldTypeString");
                }
                if (item.Value.ToString().Length > 7)
                {
                    fieldTypeDict.Add("alias", item.Value.ToString().Substring(0, 7));
                }
                else
                {
                    fieldTypeDict.Add("alias", item.Value.ToString());
                }

                fieldsList.Add(fieldTypeDict);
            }
            geoDict.Add("fields", fieldsList);
            //Produces conver dictionary to json text

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(geoDict, Newtonsoft.Json.Formatting.Indented);

            Print(json);

            // Finally assign the spiral to the output parameter.
            DA.SetData(0, spiral);
        }

        private Curve CreateSpiral(Plane plane, double r0, double r1, Int32 turns)
        {
            Line l0 = new Line(plane.Origin + r0 * plane.XAxis, plane.Origin + r1 * plane.XAxis);
            Line l1 = new Line(plane.Origin - r0 * plane.XAxis, plane.Origin - r1 * plane.XAxis);

            Point3d[] p0;
            Point3d[] p1;

            l0.ToNurbsCurve().DivideByCount(turns, true, out p0);
            l1.ToNurbsCurve().DivideByCount(turns, true, out p1);

            PolyCurve spiral = new PolyCurve();

            for (int i = 0; i < p0.Length - 1; i++)
            {
                Arc arc0 = new Arc(p0[i], plane.YAxis, p1[i + 1]);
                Arc arc1 = new Arc(p1[i + 1], -plane.YAxis, p0[i + 1]);

                spiral.Append(arc0);
                spiral.Append(arc1);
            }

            return spiral;
        }

        /// <summary>
        /// The Exposure property controls where in the panel a component icon 
        /// will appear. There are seven possible locations (primary to septenary), 
        /// each of which can be combined with the GH_Exposure.obscure flag, which 
        /// ensures the component will only be visible on panel dropdowns.
        /// </summary>
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.primary; }
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("799efa88-5c17-492c-a16f-89893ba2c4d8"); }
        }
    }
}
