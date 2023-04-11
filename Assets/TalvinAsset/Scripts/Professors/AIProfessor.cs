using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Professors
{
    public class AIProfessor : MonoBehaviour, IHear
    {
        #region Variables

        [Header("References")]
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private LayerMask obstacleMask;

        [Header("Statistics")]
        [SerializeField] private float walkSpeed;
        [SerializeField] private float chaseSpeed;
        [SerializeField] private float walkViewRadius;
        [SerializeField] private float chaseViewRadius;
        [SerializeField] private float walkViewAngle;
        [SerializeField] private float chaseViewAngle;
        [SerializeField] private float distancePlayerCatch;

        [Header("Wandering parameters")]
        [SerializeField] private float wanderingWaitTimeMin;
        [SerializeField] private float wanderingWaitTimeMax;
        [SerializeField] private float wanderingDistanceMin;
        [SerializeField] private float wanderingDistanceMax;

        [Header("Private parameters")]
        private const string PlayerTag = "Player";
        private GameObject _player;
        private float _viewRadius;
        private float _viewAngle;

        private bool _isPurchasing;
        private bool _isFollowingSong;
        private bool _isInView;
        private bool _wasInView;
        private bool _isCatch;
        private bool _wasCatch;

        private bool _hasDestination;
        private readonly Vector3[] _destinations = 
        {
            new (7, 0, -7),
            new (-7,0, -7),
            new (-7, 0, 7),
            new (7, 0, 7)
        };

        #endregion
    
        private void Start()
        {
            _viewRadius = walkViewRadius;
            _viewAngle = walkViewAngle;
            agent.speed = walkSpeed;
        }

        private void Update()
        {
            if (GetPlayer())
            {
                _isPurchasing = true;
                if (_isCatch) CatchPlayer();
                else MoveTo(_player.transform.position, chaseViewRadius, chaseViewAngle, chaseSpeed);
                return;
            }

            if (_isFollowingSong)
            {
                if (agent.hasPath) return;
                _isFollowingSong = false;
            }
            
            _isPurchasing = false;
            Npc();
        }

        private bool GetPlayer()
        {
            GameObject[] playersGameObject = GameObject.FindGameObjectsWithTag(PlayerTag);
        
            GameObject newPlayer = null;
            bool isCatch = false;
            bool isInView = false;

            _wasInView = _isInView;
            _wasCatch = _isCatch;

            foreach (GameObject playerGameObject in playersGameObject)
            {
                Vector3 position = transform.position + new Vector3(0, 1f, 0);
            
                Vector3 playerBodyPosition = playerGameObject.transform.position + new Vector3(0, 1f, 0);
                Vector3 playerHeadPosition = playerBodyPosition + new Vector3(0, 0.6f, 0);

                if (!(Vector3.Distance(position, playerBodyPosition) < _viewRadius)) continue;
                
                Vector3 directionToPlayer = (playerBodyPosition - position).normalized;
                if (!(Vector3.Angle(transform.forward, directionToPlayer) < _viewAngle / 2)) continue;
                
                float distanceToPlayer = Vector3.Distance(position, playerHeadPosition);
                if (Physics.Raycast(position, directionToPlayer, distanceToPlayer, obstacleMask)) continue;
                
                if (newPlayer != null)
                {
                    if (Vector3.Distance(position, playerBodyPosition) < Vector3.Distance(position, newPlayer.transform.position + new Vector3(0, 1f, 0)))
                    {
                        newPlayer = playerGameObject;
                    }
                }
                else
                {
                    newPlayer = playerGameObject;
                }

                if (Vector3.Distance(position, newPlayer.transform.position + new Vector3(0, 1f, 0)) < distancePlayerCatch)
                {
                    isCatch = true;
                }
                isInView = true;
            }

            _player = newPlayer;
            _isInView = isInView;
            _isCatch = isCatch;
        
            return _isInView;
        }

        private void CatchPlayer()
        {
            if (_isFollowingSong) _isFollowingSong = false;

            if (_wasCatch) return;
            
            _viewRadius = chaseViewRadius;
            _viewAngle = chaseViewAngle;
            agent.speed = chaseSpeed;
            agent.isStopped = true;
        }

        private void MoveTo(Vector3 position, float viewRadius, float viewAngle, float speed)
        {
            if (!_wasInView || (_wasInView && _wasCatch))
            {
                _viewRadius = viewRadius;
                _viewAngle = viewAngle;
                agent.speed = speed;

                agent.isStopped = false;
            }

            agent.SetDestination(position);
        }

        private void Npc()
        {
            if (!agent.hasPath && !_hasDestination)
            {
                _viewRadius = walkViewRadius;
                _viewAngle = walkViewAngle;
                agent.speed = walkSpeed;

                float delay = Random.Range(wanderingWaitTimeMin, wanderingWaitTimeMax);
                StartCoroutine(Random.Range(0, 2) == 0 ? GoToDestination(delay) : StayOnSite(delay));
            }
        }
    
        private IEnumerator GoToDestination(float delay)
        {
            _hasDestination = true;
            yield return new WaitForSeconds(delay);
        
            agent.SetDestination(_destinations[Random.Range(0, _destinations.Length)]);
            _hasDestination = false;
        }
    
        private IEnumerator StayOnSite(float delay)
        {
            _hasDestination = true;
            yield return new WaitForSeconds(delay);

            Vector3 nextDestination;
            NavMeshHit hit;
            do
            {
                nextDestination = transform.position + Random.Range(wanderingDistanceMin, wanderingDistanceMax) *
                    new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1)).normalized;
            } while (!NavMesh.SamplePosition(nextDestination, out hit, wanderingDistanceMax, NavMesh.AllAreas));
        
            agent.SetDestination(hit.position);
            _hasDestination = false;
        }
    
        private Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
        {
            if (!angleIsGlobal)
                angleInDegrees += transform.eulerAngles.y;
            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }

        private void OnDrawGizmos()
        {
            Vector3 origin = transform.position + new Vector3(0, 1f, 0);
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(origin, _viewRadius);

            Gizmos.color = _isCatch ? Color.red : Color.black;
            Gizmos.DrawWireSphere(origin, distancePlayerCatch);
        
            Vector3 viewAngleA = DirFromAngle(-_viewAngle / 2, false);
            Vector3 viewAngleB = DirFromAngle(_viewAngle / 2, false);
        
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(origin, origin + viewAngleA * _viewRadius);
            Gizmos.DrawLine(origin, origin + viewAngleB * _viewRadius);

            if (_isInView)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(origin, _player.transform.position + new Vector3(0, 1f, 0));
            }
        }

        public void RespondToSound(Sound sound)
        {
            if (_isPurchasing) return;

            switch (sound.GetType())
            {
                case SoundType.Interesting:
                    MoveTo(sound.Position, walkViewRadius, walkViewAngle, walkSpeed);
                    break;
                case SoundType.Alerting:
                    MoveTo(sound.Position, chaseViewRadius, chaseViewAngle, chaseSpeed);
                    break;
                case SoundType.Default:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _isFollowingSong = true;
        }
    }
}
