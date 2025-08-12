using TMPro;
using UnityEngine;
using Zenject;

namespace Gamify.SnakeGame.LeaderBoard
{
    public class PlayerScoreView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _noTxt;
        [SerializeField] private TMP_Text _fullNameTxt;
        [SerializeField] private TMP_Text _scoreTxt;

        public void SetData(string no, string fullName, string score)
        {
            _noTxt.text = no;
            _fullNameTxt.text = fullName;
            _scoreTxt.text = score;
            ResetScale();
        }
        public void ResetScale()
        {
            transform.localScale = Vector3.one;
        }
        public void SetParent(Transform parent)
        {
            transform.SetParent(parent);
        }
        public class Factory : PlaceholderFactory<PlayerScoreView>
        {

        }
    }
}
