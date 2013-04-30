using System;
using System.Diagnostics;
using System.Management;
using System.Runtime.InteropServices;
using System.IO;
namespace Win7EventsLibrary
{
    public class ProcessRunner
    {
        /// <summary>
        /// ProcessRunner class provides the core functionality of running the selected application
        /// The main invoke method uses two parameters, 1.Target Window 2. Application Path. Both are supplied while calling the method from Listener Service
        /// 
        /// </summary>
        private readonly string _logPath = System.Configuration.ConfigurationManager.AppSettings["LogFilePath"].ToString();
      

        private const int MAXIMUM_ALLOWED = 0x2000000;
        private const int STANDARD_RIGHTS_READ = 0x20000;
        private const int STANDARD_RIGHTS_REQUIRED = 0xf0000;
        private const int TOKEN_ADJUST_DEFAULT = 0x80;
        private const int TOKEN_ADJUST_GROUPS = 0x40;
        private const int TOKEN_ADJUST_PRIVILEGES = 0x20;
        private const int TOKEN_ADJUST_SESSIONID = 0x100;
        private const int TOKEN_ALL_ACCESS = 0xf01ff;
        private const int TOKEN_ASSIGN_PRIMARY = 1;
        private const int TOKEN_DUPLICATE = 2;
        private const int TOKEN_IMPERSONATE = 4;
        private const int TOKEN_QUERY = 8;
        private const int TOKEN_QUERY_SOURCE = 0x10;
        private const int TOKEN_READ = 0x20008;

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr hObject);
        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool CreateProcessAsUser(IntPtr hToken, string lpApplicationName, string lpCommandLine, ref IntPtr lpProcessAttributes, ref IntPtr lpThreadAttributes, bool bInheritHandles, int dwCreationFlags, IntPtr lpEnvironment, string lpCurrentDirectory, ref STARTUPINFO lpStartupInfo, ref PROCESS_INFORMATION lpProcessInformation);

       
        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool DuplicateTokenEx(IntPtr hExistingToken, uint dwDesiredAccess, IntPtr lpTokenAttributes, uint ImpersonationLevel, uint TokenType, ref IntPtr phNewToken);
        

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool OpenProcessToken(IntPtr ProcessHandle, int DesiredAccess, ref IntPtr TokenHandle);
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetTokenInformation(IntPtr TokenHandle, uint TokenInformationClass, ref int TokenInformation, int TokenInformationLength);
        public void TerminateSystemProcess(string processPath)
        {
            string queryString = processPath.Contains(@"\") ? string.Format("SELECT * FROM Win32_Process WHERE ExecutablePath='{0}'", processPath.Replace(@"\", @"\\")) : string.Format("SELECT * FROM Win32_Process WHERE Name='{0}'", processPath);
            ManagementObjectCollection objects = new ManagementObjectSearcher(@"\\.\root\CIMv2", queryString).Get();
            if (objects.Count > 0)
            {
                foreach (ManagementObject obj2 in objects)
                {
                    try
                    {
                        object[] args = new object[1];
                        obj2.InvokeMethod("GetOwner", args);
                        if (args[0].ToString().Equals("SYSTEM", StringComparison.CurrentCultureIgnoreCase))
                        {
                            obj2.InvokeMethod("Terminate", null);
                        }
                    }
                    catch (Exception exception)
                    {
                        Debug.WriteLine(exception.Message);
                    }
                }
            }
        }

        [DllImport("kernel32.dll")]
        private static extern int WTSGetActiveConsoleSessionId();
        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        const UInt32 SWP_NOSIZE = 0x0001;
        const UInt32 SWP_NOMOVE = 0x0002;
        const UInt32 SWP_SHOWWINDOW = 0x0040;

        [StructLayout(LayoutKind.Sequential)]
        internal struct PROCESS_INFORMATION
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public int dwProcessId;
            public int dwThreadId;
        }

        private enum SECURITY_IMPERSONATION_LEVEL
        {
            SecurityAnonymous,
            SecurityIdentification,
            SecurityImpersonation,
            SecurityDelegation
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct STARTUPINFO
        {
            public int cb;
            public string lpReserved;
            public string lpDesktop;
            public string lpTitle;
            public int dwX;
            public int dwY;
            public int dwXSize;
            public int dwYSize;
            public int dwXCountChars;
            public int dwYCountChars;
            public int dwFillAttribute;
            public int dwFlags;
            public short wShowWindow;
            public short cbReserved2;
            public int lpReserved2;
            public int hStdInput;
            public int hStdOutput;
            public int hStdError;
        }

        private enum TOKEN_INFORMATION_CLASS
        {
            MaxTokenInfoClass = 0x1d,
            TokenAccessInformation = 0x16,
            TokenAuditPolicy = 0x10,
            TokenDefaultDacl = 6,
            TokenElevation = 20,
            TokenElevationType = 0x12,
            TokenGroups = 2,
            TokenGroupsAndPrivileges = 13,
            TokenHasRestrictions = 0x15,
            TokenImpersonationLevel = 9,
            TokenIntegrityLevel = 0x19,
            TokenLinkedToken = 0x13,
            TokenLogonSid = 0x1c,
            TokenMandatoryPolicy = 0x1b,
            TokenOrigin = 0x11,
            TokenOwner = 4,
            TokenPrimaryGroup = 5,
            TokenPrivileges = 3,
            TokenRestrictedSids = 11,
            TokenSandBoxInert = 15,
            TokenSessionId = 12,
            TokenSessionReference = 14,
            TokenSource = 7,
            TokenStatistics = 10,
            TokenType = 8,
            TokenUIAccess = 0x1a,
            TokenUser = 1,
            TokenVirtualizationAllowed = 0x17,
            TokenVirtualizationEnabled = 0x18
        }

        private enum TOKEN_TYPE
        {
            TokenImpersonation = 2,
            TokenPrimary = 1
        }
    

        private void WriteToLog(string entry)
        {
            try
            {
                    var sw = new StreamWriter(_logPath, true);
                    sw.WriteLine("{0}\t{1}", DateTime.Now, entry);
                    sw.Close();
               
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

      
        [DllImport("user32.dll", SetLastError = true)]
        static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);
        //-----
        [DllImportAttribute("User32.DLL")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        public void InvokeProcess(string desktop, string processPath)
        {

          
            string processname = Path.GetFileNameWithoutExtension(processPath);
            var pi = new PROCESS_INFORMATION();
            var si = new STARTUPINFO();
            var defaultIntPtr = new IntPtr();
            var hToken = defaultIntPtr;
            var hDuplicateToken = defaultIntPtr;
            int sessionId = WTSGetActiveConsoleSessionId();
            int SW_RESTORE = 9;
            si.lpDesktop = desktop;
            si.cb = Marshal.SizeOf(si);
            
            OpenProcessToken(Process.GetCurrentProcess().Handle, 0x10a, ref hToken);
            DuplicateTokenEx(hToken, MAXIMUM_ALLOWED, defaultIntPtr, 2, 1, ref hDuplicateToken);
            SetTokenInformation(hDuplicateToken, 12, ref sessionId, 4);
            CreateProcessAsUser(hDuplicateToken, processPath, null, ref defaultIntPtr, ref defaultIntPtr,false, 0, defaultIntPtr, null, ref si, ref pi);
            
            
            Process curProcess = Process.GetCurrentProcess();
            Process[] arrayProcess = Process.GetProcesses();
            
            
            
            foreach (Process p1 in arrayProcess)
            {
                if (String.Compare(p1.ProcessName, processname, true) == 0)
                {
                    ShowWindow(p1.Handle, SW_RESTORE);
                    break;
            
                }
            }

            CloseHandle(hToken);
            CloseHandle(hDuplicateToken);
            CloseHandle(pi.hProcess);
            
            CloseHandle(pi.hThread);
            WriteToLog(String.Format("Application Started \t{0} {1}", desktop, processPath));
        }
 
    }
}
