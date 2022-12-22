using UnityEngine;
using UnityEngine.Events;

namespace Characters.Protagonist
{
    public class NearbyEnemies : MonoBehaviour
    {
        [SerializeField] private LayerMask _enemyLayer;
        [SerializeField] private float _dangerRange;
        [SerializeField] private UnityEvent _onNearEnemies;
        private bool _didTriggerEvent = false;
        private bool _canCheckForNearbyEnemies = true;

        public LayerMask EnemiesLayer => _enemyLayer;
#if UNITY_EDITOR
        [Header("Debug")] [SerializeField] private bool _drawGizmos;
#endif

        private void Update()
        {
            if (Amount() > 0 && !_didTriggerEvent)
            {
                _onNearEnemies.Invoke();
                _didTriggerEvent = true;
                return;
            }

            if (Amount() <= 0 && _didTriggerEvent)
            {
                _didTriggerEvent = false;
            }
            
        }
        
        public int Amount()
        {
            if (!_canCheckForNearbyEnemies) return 0;
            Collider[] enemies = new Collider[100];
            return Physics.OverlapSphereNonAlloc(transform.position, _dangerRange, enemies, _enemyLayer);
        }

        public void SetActive(bool result)
        {
            _canCheckForNearbyEnemies = result;
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!_drawGizmos) return;
            Gizmos.color = Amount() > 0 ? Color.red : Color.green;
            Gizmos.DrawWireSphere(transform.position, _dangerRange);
        }
#endif
    }
}