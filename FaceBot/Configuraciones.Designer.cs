﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FaceBot {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.7.0.0")]
    internal sealed partial class Configuraciones : global::System.Configuration.ApplicationSettingsBase {
        
        private static Configuraciones defaultInstance = ((Configuraciones)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Configuraciones())));
        
        public static Configuraciones Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("500")]
        public double FacesDetectorInterval {
            get {
                return ((double)(this["FacesDetectorInterval"]));
            }
            set {
                this["FacesDetectorInterval"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("2000")]
        public double FacesRecognizerInterval {
            get {
                return ((double)(this["FacesRecognizerInterval"]));
            }
            set {
                this["FacesRecognizerInterval"] = value;
            }
        }
    }
}
