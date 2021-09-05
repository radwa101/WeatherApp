using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration.Install;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.DirectoryServices;
using Microsoft.Web.Administration;

namespace CustomInstaller
{
    [RunInstaller(true)]
    public class SetupAction : Installer
    {
        public override void Install(System.Collections.IDictionary stateSaver)
        { 
            base.Install(stateSaver);

            RegisterScriptMaps();
        }

        void RegisterScriptMaps()
        {
            try
            {
                CreateAppPool("CiaransApp");
                CreateIISWebsite("CiaransApp", "Test123");
            }
            catch (Exception e)
            {
                File.AppendAllText(@"C:\TDM\Log.txt", e.Message + " ");
            }
        }

        public void CreateAppPool(string appPool)
        {
            using (ServerManager serverManager = new ServerManager())
            {
                ApplicationPool newPool = serverManager.ApplicationPools.Add(appPool);
                newPool.ManagedRuntimeVersion = "v4.0";
                newPool.Enable32BitAppOnWin64 = true;
                newPool.ManagedPipelineMode = ManagedPipelineMode.Integrated;
                serverManager.CommitChanges();
            }
        }

        private static void CreateIISWebsite(string webSite, string appPool)
        {
            ServerManager iisManager = new ServerManager();
            iisManager.Sites.Add(webSite, "http", "*:80:", @"C:\TDM\");
            iisManager.Sites[webSite].ApplicationDefaults.ApplicationPoolName = appPool;

            foreach (var item in iisManager.Sites["Test123"].Applications)
            {
                item.ApplicationPoolName = appPool;
            }
            iisManager.CommitChanges();
        }
    }
}

