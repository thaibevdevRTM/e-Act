﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace eActConsoleServiceWeekly.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.3.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=10.6.241.71; Initial Catalog=DBEAct_DEV;User ID=rtmDev;password=Thaib" +
            "evRTM@DB;Max Pool Size=200;")]
        public string strConn {
            get {
                return ((string)(this["strConn"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("devrtm@thaibev.com")]
        public string strMailUser {
            get {
                return ((string)(this["strMailUser"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Thaibev@dev20i9")]
        public string strMailPassword {
            get {
                return ((string)(this["strMailPassword"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"
          แจ้งเอกสารจากระบบอัตโนมัติ
          <br/><br/>
          รายงานการสั่งซื้อสินค้า crystal กรุณาตรวจสอบข้อมูลใน file แนบค่ะ
          <br/><br/>
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
    }
}