using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class WeaponScript : MonoBehaviour
{
    [Header("Weapon Settings")]
    public float fireRate = 0.1f;
    public int magazineSize = 30;
    public int totalAmmo = 90;
    public float spread = 0.1f;
    public float reloadTime = 2f;
    public int damage = 10;

    public int DAMAGE;

    private float nextFireTime;
    private int currentAmmo;
    private bool isReloading;

    [SerializeField] GameObject ShootingVFX;
    [SerializeField] GameObject Tracer;
    [SerializeField] Transform TracerOutPosition;
    [SerializeField] WeaponRecoil recoil;
    private void Start()
    {
        currentAmmo = magazineSize;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && Time.time >= nextFireTime && !isReloading)
        {
            Fire();
        }

        if (Input.GetKeyDown(KeyCode.R) && !isReloading)
        {
            StartCoroutine(Reload());
        }
    }

    private void Fire()
    {
        if (currentAmmo > 0)
        {
            nextFireTime = Time.time + fireRate;
            currentAmmo--;

            Vector3 spreadVector = Random.insideUnitSphere * spread;
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            ray.direction += spreadVector;

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // ������ ��������� �� ������ ����� ����)))
                StartCoroutine(TracerRenderer(TracerOutPosition.position, hit.point));
                Instantiate(ShootingVFX, TracerOutPosition.position, TracerOutPosition.rotation);

                if(hit.transform.TryGetComponent<IDamagable>(out IDamagable damage))
                {
                    damage.GetDamage(DAMAGE);
                }
                
                Debug.Log("Hit: " + hit.transform.name + " for " + damage + " damage");
            }

           
            recoil.Recoil();
        }
        else if (!isReloading)
        {
            StartCoroutine(Reload());
        }
    }

    public IEnumerator TracerRenderer(Vector3 start, Vector3 target)
    {
        float duration = 0.1f; // ������������ ������ ��������
        float elapsedTime = 0f;

        GameObject tracer = Instantiate(Tracer, TracerOutPosition.position, Quaternion.LookRotation(target - start));

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration; // ��������������� �����

            // ���������� Lerp ��� �������� �����������
            tracer.transform.position = Vector3.Lerp(start, target, t);

            yield return null; // ���� ���������� �����
        }

        // ��������, ��� ������� ������ �������� �����
        tracer.transform.position = target;

        // �����������: ���������� ������� ����� ��������� ��������
        yield return new WaitForSeconds(1f);

        Destroy(tracer);
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        yield return new WaitForSeconds(reloadTime);

        int ammoToReload = Mathf.Min(magazineSize - currentAmmo, totalAmmo);
        currentAmmo += ammoToReload;
        totalAmmo -= ammoToReload;

        isReloading = false;
        Debug.Log("Reload complete. Current ammo: " + currentAmmo + ", Total ammo: " + totalAmmo);
    }
}
