using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace SetupCustomActions
{
    [RunInstaller(true)]
    public partial class CustomInstaller : Installer
    {
        private string logFilePath;

        public CustomInstaller()
        {
            try
            {
                string logDir = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                    "VotreApplication");

                Directory.CreateDirectory(logDir);
                logFilePath = Path.Combine(logDir, "install.log");
            }
            catch
            {
                logFilePath = Path.Combine(Path.GetTempPath(), "VotreApplication_install.log");
            }
        }

        public override void Install(IDictionary stateSaver)
        {
            base.Install(stateSaver);

            try
            {
                // Obtenir le dossier d'installation
                string installDir = GetInstallDirectory();

                LogMessage("=======================================================");
                LogMessage("Début de l'installation personnalisée");
                LogMessage($"Dossier d'installation : {installDir}");
                LogMessage("=======================================================");

                // Étape 1 : Installer SQL Server Express
                LogMessage("");
                LogMessage("ÉTAPE 1 : Installation de SQL Server Express");
                LogMessage("-------------------------------------------------------");

                if (!InstallSQLServer(installDir))
                {
                    throw new InstallException("L'installation de SQL Server Express a échoué. Consultez le journal : " + logFilePath);
                }

                LogMessage("✓ SQL Server Express installé avec succès");

                // Attendre que SQL Server soit prêt
                LogMessage("Attente du démarrage de SQL Server...");
                Thread.Sleep(10000); // 10 secondes

                // Étape 2 : Restaurer la base de données
                LogMessage("");
                LogMessage("ÉTAPE 2 : Restauration de la base de données");
                LogMessage("-------------------------------------------------------");

                if (!RestoreDatabase(installDir))
                {
                    throw new InstallException("La restauration de la base de données a échoué. Consultez le journal : " + logFilePath);
                }

                LogMessage("✓ Base de données restaurée avec succès");
                LogMessage("");
                LogMessage("=======================================================");
                LogMessage("Installation personnalisée terminée avec succès !");
                LogMessage("=======================================================");

                stateSaver.Add("InstallSuccess", true);
            }
            catch (Exception ex)
            {
                LogMessage("");
                LogMessage("=======================================================");
                LogMessage("ERREUR FATALE !");
                LogMessage("=======================================================");
                LogMessage($"Exception : {ex.GetType().Name}");
                LogMessage($"Message : {ex.Message}");
                LogMessage($"StackTrace : {ex.StackTrace}");

                throw new InstallException(
                    $"Erreur lors de l'installation personnalisée.\n\n" +
                    $"Détails : {ex.Message}\n\n" +
                    $"Consultez le journal : {logFilePath}",
                    ex);
            }
        }

        private string GetInstallDirectory()
        {
            // Méthode 1 : Depuis l'emplacement de cette DLL
            string dllPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string dir = Path.GetDirectoryName(dllPath);

            if (!string.IsNullOrEmpty(dir) && Directory.Exists(dir))
            {
                LogMessage($"Dossier trouvé via DLL location : {dir}");
                return dir;
            }

            // Méthode 2 : Depuis les paramètres Context
            if (Context.Parameters.ContainsKey("assemblypath"))
            {
                string assemblyPath = Context.Parameters["assemblypath"];
                if (!string.IsNullOrEmpty(assemblyPath))
                {
                    dir = Path.GetDirectoryName(assemblyPath);
                    if (!string.IsNullOrEmpty(dir) && Directory.Exists(dir))
                    {
                        LogMessage($"Dossier trouvé via assemblypath : {dir}");
                        return dir;
                    }
                }
            }

            // Méthode 3 : Depuis targetdir
            if (Context.Parameters.ContainsKey("targetdir"))
            {
                dir = Context.Parameters["targetdir"];
                if (!string.IsNullOrEmpty(dir) && Directory.Exists(dir))
                {
                    LogMessage($"Dossier trouvé via targetdir : {dir}");
                    return dir.TrimEnd('\\');
                }
            }

            throw new InstallException("Impossible de déterminer le dossier d'installation");
        }

        private bool InstallSQLServer(string installPath)
        {
            try
            {
                string scriptPath = Path.Combine(installPath, "InstallSQLExpress.ps1");

                if (!File.Exists(scriptPath))
                {
                    LogMessage($"✗ Script non trouvé : {scriptPath}");

                    // Lister les fichiers présents
                    LogMessage("Fichiers présents dans le dossier :");
                    foreach (string file in Directory.GetFiles(installPath))
                    {
                        LogMessage($"  - {Path.GetFileName(file)}");
                    }

                    return false;
                }

                LogMessage($"Script trouvé : {scriptPath}");
                return RunPowerShellScript(scriptPath, "Installation SQL Server");
            }
            catch (Exception ex)
            {
                LogMessage($"✗ Exception lors de l'installation SQL : {ex.Message}");
                LogMessage($"StackTrace : {ex.StackTrace}");
                return false;
            }
        }

        private bool RestoreDatabase(string installPath)
        {
            try
            {
                string scriptPath = Path.Combine(installPath, "RestoreDatabase.ps1");

                if (!File.Exists(scriptPath))
                {
                    LogMessage($"✗ Script non trouvé : {scriptPath}");
                    return false;
                }

                LogMessage($"Script trouvé : {scriptPath}");
                return RunPowerShellScript(scriptPath, "Restauration base de données");
            }
            catch (Exception ex)
            {
                LogMessage($"✗ Exception lors de la restauration : {ex.Message}");
                LogMessage($"StackTrace : {ex.StackTrace}");
                return false;
            }
        }

        private bool RunPowerShellScript(string scriptPath, string actionName)
        {
            try
            {
                LogMessage($"Lancement de PowerShell pour : {actionName}");
                LogMessage($"Commande : powershell.exe -NoProfile -ExecutionPolicy Bypass -File \"{scriptPath}\"");

                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Arguments = $"-NoProfile -ExecutionPolicy Bypass -File \"{scriptPath}\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    WorkingDirectory = Path.GetDirectoryName(scriptPath)
                };

                using (Process process = Process.Start(psi))
                {
                    // Lire la sortie
                    string output = process.StandardOutput.ReadToEnd();
                    string errors = process.StandardError.ReadToEnd();

                    process.WaitForExit();

                    LogMessage($"Code de sortie : {process.ExitCode}");

                    if (!string.IsNullOrWhiteSpace(output))
                    {
                        LogMessage("--- Sortie standard ---");
                        LogMessage(output);
                    }

                    if (!string.IsNullOrWhiteSpace(errors))
                    {
                        LogMessage("--- Erreurs ---");
                        LogMessage(errors);
                    }

                    bool success = process.ExitCode == 0;

                    if (success)
                    {
                        LogMessage($"✓ {actionName} terminé avec succès");
                    }
                    else
                    {
                        LogMessage($"✗ {actionName} a échoué (code {process.ExitCode})");
                    }

                    return success;
                }
            }
            catch (Exception ex)
            {
                LogMessage($"✗ Exception lors de l'exécution du script : {ex.Message}");
                LogMessage($"StackTrace : {ex.StackTrace}");
                return false;
            }
        }

        private void LogMessage(string message)
        {
            try
            {
                string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}\n";

                // Écrire dans le fichier
                File.AppendAllText(logFilePath, logEntry);

                // Aussi dans le journal Windows Installer
                if (Context != null)
                {
                    Context.LogMessage(message);
                }
            }
            catch
            {
                // Ignorer les erreurs de log
            }
        }

        public override void Commit(IDictionary savedState)
        {
            base.Commit(savedState);

            if (savedState.Contains("InstallSuccess") && (bool)savedState["InstallSuccess"])
            {
                LogMessage("Installation validée (Commit)");
            }
        }

        public override void Rollback(IDictionary savedState)
        {
            base.Rollback(savedState);
            LogMessage("Installation annulée (Rollback)");
        }

        public override void Uninstall(IDictionary savedState)
        {
            base.Uninstall(savedState);
            LogMessage("Désinstallation");
        }
    }
}