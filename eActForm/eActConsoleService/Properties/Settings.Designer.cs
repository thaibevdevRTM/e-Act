﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace eActConsoleService.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.3.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("{0}\\logs\\logs_{1}.txt")]
        public string logsFileName {
            get {
                return ((string)(this["logsFileName"]));
            }
            set {
                this["logsFileName"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("tanapong.w@thaibev.com")]
        public string strDefaultEmail {
            get {
                return ((string)(this["strDefaultEmail"]));
            }
            set {
                this["strDefaultEmail"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("true")]
        public string isDevelop {
            get {
                return ((string)(this["isDevelop"]));
            }
            set {
                this["isDevelop"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("[e-ActivityForm] แจ้งเอกสาร รออนุมัติ จากระบบอัตโนมัติ")]
        public string strSubject {
            get {
                return ((string)(this["strSubject"]));
            }
            set {
                this["strSubject"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("devrtm@thaibev.com")]
        public string strMailUser {
            get {
                return ((string)(this["strMailUser"]));
            }
            set {
                this["strMailUser"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Thaibev@dev20i9")]
        public string strMailPassword {
            get {
                return ((string)(this["strMailPassword"]));
            }
            set {
                this["strMailPassword"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("https:\\\\eact.thaibev.com/?s=approve")]
        public string strUrlApprove {
            get {
                return ((string)(this["strUrlApprove"]));
            }
            set {
                this["strUrlApprove"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("thaibevdevrtm@gmail.com")]
        public string strMailCC {
            get {
                return ((string)(this["strMailCC"]));
            }
            set {
                this["strMailCC"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("parnupong.k@thaibev.com,tanapong.w@thaibev.com")]
        public string emailForDevelopSite {
            get {
                return ((string)(this["emailForDevelopSite"]));
            }
            set {
                this["emailForDevelopSite"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=10.5.56.33; Initial Catalog=DBEAct;User ID=rtmDev;password=ThaibevRTM" +
            "@DB;Max Pool Size=200;")]
        public string strConn {
            get {
                return ((string)(this["strConn"]));
            }
            set {
                this["strConn"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"
          เรียน {0}
          <br/><br/>
          แจ้งเอกสารจากระบบอัตโนมัติ
          <br/><br/>
          ท่านมีเอกสาร <b>รออนุมัติ</b> จำนวน <b>{1}</b> รายการ
          <br/>
          ท่านสามารถตรวจสอบรายละเอียดเพิ่มเติม และ Approve รายการได้ตามลิ้งค์นี้ : <a href=""{2} "" > Click </a> <br><br>
          <br/>จึงเรียนมาเพื่อทราบ
          <br/>ส่วนงานพัฒนาระบบงานสนับสนุนการขาย-RTM
          <br/>อีเมล์นี้ถูกสร้างจากระบบอัตโนมัติ ไม่ต้องตอบกลับ
          <br/>หากท่านต้องการทราบรายละเอียดเพิ่มเติม กรุณาติดต่อ ส่วนงานพัฒนาระบบงานสนับสนุน
          <br/>เบอร์โทรศัพท์ติดต่อ : 02-785-5555 ext. 5744,4580,4607
          <br/>Hot Line: 063-1970586
        ")]
        public string strBody {
            get {
                return ((string)(this["strBody"]));
            }
            set {
                this["strBody"] = value;
            }
        }
    }
}
