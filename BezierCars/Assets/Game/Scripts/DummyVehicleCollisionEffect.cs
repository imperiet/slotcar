using NaughtyAttributes;
using UnityEngine;
namespace Thoreas.Vehicles
{
    public class DummyVehicleCollisionEffect : MonoBehaviour
    {

        [SerializeField] [ShowAssetPreview] GameObject collisionEffectPrefab;

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.impulse.magnitude == 0) return;

            var collisionEffect = Instantiate(collisionEffectPrefab, collision.GetContact(collision.contactCount-1).point, Quaternion.identity);

            DG.Tweening.DOVirtual.DelayedCall(.5f, () => { Destroy(collisionEffect); });
        }
    }
}
