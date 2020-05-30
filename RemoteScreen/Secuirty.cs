using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.AccessControl;
using System.IO;

namespace RemoteScreen
{
    class Secuirty
    {
        public static void AddFileSecurity(string fileName, string account, FileSystemRights rights,
            AccessControlType controlType)
        {
            FileSecurity fSecurity = File.GetAccessControl(fileName);
            fSecurity.AddAccessRule(new FileSystemAccessRule(account, rights, controlType));
            File.SetAccessControl(fileName, fSecurity);
        }

        public static void RemoveFileSecurity(string fileName, string account, FileSystemRights rights,
            AccessControlType controlType)
        {
            FileSecurity fSecurity = File.GetAccessControl(fileName);
            fSecurity.RemoveAccessRule(new FileSystemAccessRule(account, rights, controlType));
            File.SetAccessControl(fileName, fSecurity);
        }
    }
}
