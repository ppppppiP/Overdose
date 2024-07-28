using System.Collections;
using UnityEngine;

public class Shotgun : MonoBehaviour
{
    [Header("Shotgun Settings")]
    public int pelletCount = 10; // Количество рейкастов (пуль)
    public float damage = 10f; // Урон каждой пули
    public float spread = 0.1f; // Кучность (разброс) пуль
    public float range = 100f; // Дальность стрельбы
    public float fireRate = 1f; // Задержка между выстрелами
    public int magazineSize = 5; // Количество патронов в магазине
    public int totalAmmo = 20; // Общее количество патронов
    public float reloadTime = 2f; // Время перезарядки

    [Header("References")]
    public Camera fpsCamera; // Камера игрока
    public ParticleSystem muzzleFlash; // Эффект вспышки выстрела
    public GameObject impactEffect; // Эффект попадания

    private float nextTimeToFire = 0f; // Время следующего выстрела
    private int currentAmmo; // Текущие патроны в магазине
    private bool isReloading = false; // Флаг перезарядки

    [SerializeField] GameObject ShootingVFX;
    [SerializeField] GameObject ShootingSmokeVFX;
    [SerializeField] GameObject Bullet;
    [SerializeField] GameObject Tracer;
    [SerializeField] Transform TracerOutPosition;
    [SerializeField] Transform BulletOutPosition;
    [SerializeField] Transform SmokeOutPosition;
    [SerializeField] Animator anim;
    [SerializeField] WeaponRecoil recoil;
    void Start()
    {
        currentAmmo = magazineSize; // Инициализация патронов в магазине
    }

    void Update()
    {
        if (isReloading)
            return;

        if (currentAmmo <= 0 && totalAmmo > 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && Time.time >= nextTimeToFire && currentAmmo > 0)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
            Instantiate(ShootingVFX, TracerOutPosition.position, TracerOutPosition.rotation);
            recoil.Recoil();
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        // Здесь вы можете добавить анимацию перезарядки
        yield return new WaitForSeconds(reloadTime);

        int ammoToReload = magazineSize - currentAmmo;
        if (totalAmmo >= ammoToReload)
        {
            currentAmmo += ammoToReload;
            totalAmmo -= ammoToReload;
        }
        else
        {
            currentAmmo += totalAmmo;
            totalAmmo = 0;
        }

        isReloading = false;
    }

    void Shoot()
    {
        currentAmmo--;
        anim.CrossFade("Fire", 0f);
        Instantiate(Bullet, BulletOutPosition.position, BulletOutPosition.rotation);
        Instantiate(ShootingSmokeVFX, SmokeOutPosition.position, SmokeOutPosition.rotation);

        // Воспроизведение эффекта вспышки выстрела
        muzzleFlash.Play();

        for (int i = 0; i < pelletCount; i++)
        {
            // Создание рандомного направления с учетом разброса
            Vector3 shootDirection = fpsCamera.transform.forward;
            shootDirection.x += Random.Range(-spread, spread);
            shootDirection.y += Random.Range(-spread, spread);
            shootDirection.z += Random.Range(-spread, spread);

            // Выполнение рейкаста
            RaycastHit hit;
            if (Physics.Raycast(fpsCamera.transform.position, shootDirection, out hit, range))
            {
                // Получение цели
                StartCoroutine(TracerRenderer(TracerOutPosition.position, hit.point));
                

                if (hit.transform.TryGetComponent<IDamagable>(out IDamagable damage))
                {
                    damage.GetDamage(this.damage);
                }


                // Воспроизведение эффекта попадания
                //GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                //Destroy(impactGO, 2f);
            }
        }
    }

    public IEnumerator TracerRenderer(Vector3 start, Vector3 target)
    {
        float duration = 0.1f; // Длительность полета трейсера
        float elapsedTime = 0f;

        GameObject tracer = Instantiate(Tracer, TracerOutPosition.position, Quaternion.LookRotation(target - start));

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration; // Нормализованное время

            // Используем Lerp для плавного перемещения
            tracer.transform.position = Vector3.Lerp(start, target, t);

            yield return null; // Ждем следующего кадра
        }

        // Убедимся, что трейсер достиг конечной точки
        tracer.transform.position = target;

        // Опционально: уничтожаем трейсер после небольшой задержки
        yield return new WaitForSeconds(1f);

        Destroy(tracer);
    }
}
