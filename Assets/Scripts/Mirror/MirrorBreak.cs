using UnityEngine;

namespace Mirror
{
    public class MirrorBreak : MonoBehaviour
    {
        [SerializeField] private GameObject _completeMirror;
        private MirrorPiece[]_mirrorPices;

        private void Awake()
        {
            _mirrorPices = GetComponentsInChildren<MirrorPiece>();
        }

        public void Break()
        {
            _completeMirror.gameObject.SetActive(false);
            foreach (var piece in _mirrorPices)
            {
                piece.PlayAnimation();
            }
        }
    }
}
