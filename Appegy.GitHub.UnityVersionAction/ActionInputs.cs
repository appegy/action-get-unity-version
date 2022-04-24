namespace Appegy.GitHub.UnityVersionAction
{
    public class ActionInputs
    {
        public string ProjectPath { get; set; } = null!;
        public string VersionEnv { get; set; } = "";
        public string ChangesetEnv { get; set; } = "";
    }
}