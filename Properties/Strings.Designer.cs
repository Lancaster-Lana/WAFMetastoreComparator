﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WAFMetastoreComparator.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Strings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Strings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("WAFMetastoreComparator.Properties.Strings", typeof(Strings).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to One of the Metastores doesn&apos;t contains the table {0}. Comparison failed !.
        /// </summary>
        internal static string absentTable {
            get {
                return ResourceManager.GetString("absentTable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Comparison completed !.
        /// </summary>
        internal static string comparisonFinish {
            get {
                return ResourceManager.GetString("comparisonFinish", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to  WAIT ! Comparison of the Metastores is started !.
        /// </summary>
        internal static string comparisonStarted {
            get {
                return ResourceManager.GetString("comparisonStarted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please, select essensial Metastore(s).xml file .
        /// </summary>
        internal static string customizationFilePathFailed {
            get {
                return ResourceManager.GetString("customizationFilePathFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Field {0} has incorrect format ! Please, check it and try again. .
        /// </summary>
        internal static string fieldIncorrectFormat {
            get {
                return ResourceManager.GetString("fieldIncorrectFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There are errors in formXML!.
        /// </summary>
        internal static string formXMLIncorrect {
            get {
                return ResourceManager.GetString("formXMLIncorrect", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Start loading......
        /// </summary>
        internal static string loadingBegin {
            get {
                return ResourceManager.GetString("loadingBegin", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Loading of the Metastore files done!.
        /// </summary>
        internal static string loadingSuccess {
            get {
                return ResourceManager.GetString("loadingSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Select a TABLE.
        /// </summary>
        internal static string selectTable {
            get {
                return ResourceManager.GetString("selectTable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Warning.
        /// </summary>
        internal static string warning {
            get {
                return ResourceManager.GetString("warning", resourceCulture);
            }
        }
    }
}
