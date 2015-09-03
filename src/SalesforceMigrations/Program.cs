﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using CLAP;
using CLAP.Validation;
using Salesforce.Migrations.Assistant.Library;

namespace SalesforceMigrations
{
    public class Program
    {
        public static void Main(string[] args)
        {
            foreach (var s in args)
            {
                Console.WriteLine("Arg: {0}", s);
            }

            try
            {
                Parser.Run<CommadParser>(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
            }
            
        }
    }

    internal class CommadParser
    {
        [Verb(IsDefault = true)]
        public static void BuildPackage(
                        [DirectoryExists][Required] string workingdirectory,
                        [DirectoryExists][Required] string buildOutputDirectory,
                        [Required] string gitUrl,
                        [Required][DefaultValue("origin/develop")] string gitBranch,
                        [Required] string gitCommit)
        {
            SalesForceMigrationsAssistantBuild smab = new SalesForceMigrationsAssistantBuild( workingdirectory,
                                                                                              buildOutputDirectory,
                                                                                              gitUrl,
                                                                                              gitBranch,
                                                                                              gitCommit);

            XmlDocument document = smab.BuildPackageFile();

            if (document != null)
            {
                if (StructureManagement.BuildFileSystemStructure(buildOutputDirectory))
                {
                    // ReSharper disable once UseStringInterpolation
                    string formattedfilename = string.Format("{0}\\{1}", buildOutputDirectory,
                        StructureManagement.ManifestRelativeLocation);

                    // ReSharper disable once UseStringInterpolation
                    document.Save(string.Format("{0}\\package.xml",formattedfilename));

                    // ReSharper disable once UseStringInterpolation
                    System.IO.File.WriteAllText(string.Format("{0}\\change.log", formattedfilename), smab.GetChangeDetails);
                    

                    Console.WriteLine("XML Output {0}", document.InnerXml);
                }
                
            }
        }
    }
}
