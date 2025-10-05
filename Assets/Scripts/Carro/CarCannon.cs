using UnityEngine;
using UnityEngine.UI;

public class CarCannon : MonoBehaviour
{
    public Camera fireCamera;
    public EnterVehicle enterVehicle;
    public KeyCode fireKey = KeyCode.Space;
    public int shots = 10;
    public float cooldown = 0.35f;
    public float maxRange = 200f;
    public LayerMask hitMask = ~0;

    public GameObject explosionPrefab;
    public float explosionRadius = 7f;
    public float explosionForce = 1200f;
    public float upwardsModifier = 0.5f;
    public int damage = 999;

    public AudioSource sfx;
    public AudioClip shotSfx;

    float nextShotTime;

    public GameObject carHudRoot;
    public Text shotsText;

    void Update()
    {
        bool inCar = enterVehicle && enterVehicle.IsInVehicle;

        if (carHudRoot && carHudRoot.activeSelf != inCar)
            carHudRoot.SetActive(inCar);

        if (!inCar || fireCamera == null || !fireCamera.isActiveAndEnabled)
            return;

        if (shotsText) shotsText.text = $"{shots}/10";

        if (shots > 0 && Time.time >= nextShotTime && Input.GetKeyDown(fireKey))
            Fire();
    }

    void Fire()
    {
        nextShotTime = Time.time + cooldown;
        shots--;

        Ray ray = fireCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Vector3 hitPoint;
        Vector3 hitNormal;

        if (Physics.Raycast(ray, out RaycastHit hit, maxRange, hitMask, QueryTriggerInteraction.Ignore))
        {
            hitPoint = hit.point;
            hitNormal = hit.normal;
        }
        else
        {
            hitPoint = fireCamera.transform.position + fireCamera.transform.forward * maxRange;
            hitNormal = -fireCamera.transform.forward;
        }

        if (explosionPrefab)
        {
            var rot = Quaternion.LookRotation(hitNormal);
            Instantiate(explosionPrefab, hitPoint + hitNormal * 0.05f, rot);
        }

        Collider[] cols = Physics.OverlapSphere(hitPoint, explosionRadius, hitMask, QueryTriggerInteraction.Ignore);
        foreach (var c in cols)
        {
            var rb = c.attachedRigidbody;
            if (rb) rb.AddExplosionForce(explosionForce, hitPoint, explosionRadius, upwardsModifier, ForceMode.Impulse);

            var dmgTarget = c.GetComponentInParent<ILevarDano>();
            if (dmgTarget != null)
            {
                try { dmgTarget.LevarDano(damage); } catch { }
            }
        }

        if (sfx && shotSfx) sfx.PlayOneShot(shotSfx);
    }
}
