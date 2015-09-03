using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibGit2Sharp;

namespace Salesforce.Migrations.Assistant.Library
{
    static public class StructureManagement
    {
        public static bool BuildFileSystemStructure(string workspace)
        {
            try
            {
                // ReSharper disable once UseStringInterpolation
                string artifactsLocation = String.Format("{0}\\artifacts", workspace);

                if (!Directory.Exists(artifactsLocation))
                {
                    var dirinfo = Directory.CreateDirectory(artifactsLocation);

                    // ReSharper disable once UseStringInterpolation
                    Directory.CreateDirectory(string.Format("{0}\\{1}", dirinfo.FullName, ConfigurationManager.AppSettings["manifestLocation"]));

                    // ReSharper disable once UseStringInterpolation
                    Directory.CreateDirectory(String.Format("{0}\\{1}", dirinfo.FullName, ConfigurationManager.AppSettings["srcLocation"]));
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static string ManifestRelativeLocation
        {
            get
            {
                return String.Format("artifacts\\{0}", ConfigurationManager.AppSettings["manifestLocation"]);
            }
        }


        public static string SourceRelativeLocation
        {
            get
            {
                return String.Format("artifacts\\{0}", ConfigurationManager.AppSettings["srcLocation"]);
            }  
        } 
    }
}
