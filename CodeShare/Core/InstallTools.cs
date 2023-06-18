using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Windows;

namespace CodeShare.Core
{
    public static class InstallTools
    {
        public static string? ExecutableFile => System.Environment.ProcessPath;
        public static string? ExecutableFolder => Path.GetDirectoryName(ExecutableFile);

        public static void Install()
        {
            System.Threading.Thread.Sleep(2000);

            if (!SetPermissions())
            {
                MessageBox.Show("Error while setting app permissions.");
            }

            Application.Current.Shutdown();
        }

        public static bool SetPermissions()
        {
            try
            {
                if (ExecutableFolder == null) throw new DirectoryNotFoundException();

                var directoryInfo = new DirectoryInfo(ExecutableFolder);
                var directorySecurity = directoryInfo.GetAccessControl();

                var users = new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null);
                directorySecurity.AddAccessRule(new FileSystemAccessRule
                    (
                        users,
                        FileSystemRights.Modify | FileSystemRights.Synchronize,
                        InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
                        PropagationFlags.None,
                        AccessControlType.Allow
                    )
                );

                directoryInfo.SetAccessControl(directorySecurity);

            }
            catch
            {
                return false;
            }
            
            return true;
        }
    }
}
