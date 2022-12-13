using System.Text;

namespace Services.Services.BlobStorages;

public static class BlobStorageConnectionStringParser
{
    // TODO: refactor it
    public static Dictionary<string, string> Parse(string connectionString)
    {
        var result = new Dictionary<string, string>();

        var parts = connectionString.Split(';');
        foreach (var part in parts)
        {
            var keyBuilder = new StringBuilder();
            var valueBuilder = new StringBuilder();
            bool isKeyBeingBuild = true;
            foreach (var character in part)
            {
                if (isKeyBeingBuild)
                {
                    if (character == '=')
                    {
                        isKeyBeingBuild = false;
                    }
                    else
                    {
                        keyBuilder.Append(character);
                    }
                }
                else
                {
                    valueBuilder.Append(character);
                }
            }
            result.Add(keyBuilder.ToString(), valueBuilder.ToString());
        }
        
        return result;
    }
}
