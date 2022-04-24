using System;
using System.IO;
using System.Text.RegularExpressions;
using DotnetActionsToolkit;

namespace Appegy.GitHub.UnityVersionAction
{
    public class Program
    {
        private static readonly Core _core = new Core();

        private static void Main(string[] args)
        {
            try
            {
                var input = GetInputs();
                var version = FindUnityVersion(input.ProjectPath);

                _core.Info($"{nameof(version.Version)}: {version.Version}");
                _core.Info($"{nameof(version.Changeset)}: {version.Changeset}");
                _core.SetOutput("version", version.Version);
                _core.SetOutput("changeset", version.Changeset);

                if (!string.IsNullOrEmpty(input.VersionEnv))
                {
                    _core.Info($"Setting environment variable {input.VersionEnv} to {version.Version}");
                    _core.ExportVariable(input.VersionEnv, version.Version);
                }

                if (!string.IsNullOrEmpty(input.ChangesetEnv))
                {
                    _core.Info($"Setting environment variable {input.ChangesetEnv} to {version.Changeset}");
                    _core.ExportVariable(input.ChangesetEnv, version.Changeset);
                }
            }
            catch (Exception ex)
            {
                _core.SetFailed(ex.Message);
            }
        }

        private static ActionInputs GetInputs()
        {
            return new ActionInputs
            {
                ProjectPath = _core.GetInput("project_path", true),
                VersionEnv = _core.GetInput("version_env", false),
                ChangesetEnv = _core.GetInput("changeset_env", false),
            };
        }

        private static UnityVersion FindUnityVersion(string projectPath)
        {
            var path = Path.Combine(projectPath, "ProjectSettings", "ProjectVersion.txt");
            if (!File.Exists(path))
            {
                Console.Error.WriteLine($"Could not find {path}");
                return null;
            }

            var file = File.ReadAllText(path);
            var match = Regex.Match(file, @"m_EditorVersionWithRevision: (.+) \((.+)\)");
            if (match.Success)
            {
                return new UnityVersion(match.Groups[1].Value, match.Groups[2].Value);
            }

            match = Regex.Match(file, @"m_EditorVersion: (.+) \((.+)\)");
            if (match.Success)
            {
                return new UnityVersion(match.Groups[1].Value, string.Empty);
            }

            throw new ArgumentException($"Could not find version in {path}");
        }
    }
}