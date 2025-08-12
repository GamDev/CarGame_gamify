using UnityEngine;
namespace Gamify.SnakeGame.Data
{
    public class JsonDataDownloader
    {
        public void DownloadJSON(string filename, string jsonContent)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
        string jsCode = $@"
            var blob = new Blob(['{jsonContent}'], {{type: 'application/json'}});
            var url = URL.createObjectURL(blob);
            var a = document.createElement('a');
            a.href = url;
            a.download = '{filename}';
            document.body.appendChild(a);
            a.click();
            document.body.removeChild(a);
            URL.revokeObjectURL(url);
        ";
        Application.ExternalEval(jsCode);
#else
            Debug.Log($"Would download file: {filename}\nContent:\n{jsonContent}");
#endif
        }
    }
}
