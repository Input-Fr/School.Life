using UnityEngine;

namespace Professors
{
    public class Sounds : MonoBehaviour
    {
        public static void MakeSound(AudioSource source, Sound sound)
        {
            Collider[] colliders = Physics.OverlapSphere(sound.Position, sound.HearingRange);

            foreach (Collider collider in colliders)
            {
                if (!collider.transform.parent.TryGetComponent(out IHear hearer)) continue;

                SoundType soundType =
                    Vector3.Distance(collider.transform.position, sound.Position) < sound.HearingRange / 2
                        ? SoundType.Alerting
                        : SoundType.Interesting;
                
                sound.SetType(soundType);
                hearer.RespondToSound(sound);
                return;
            }
        }
    }
}
