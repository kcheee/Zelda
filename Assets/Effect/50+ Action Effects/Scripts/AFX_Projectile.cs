namespace TheDeveloper.AFX{
    using UnityEngine;

    [RequireComponent(typeof(ParticleSystem))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class AFX_Projectile : MonoBehaviour
    {
        public float speed = 5.0f;
        public GameObject muzzle;
        public GameObject impact;

        private ParticleSystem ps;
        private void Start()
        {
            var instance = Instantiate(muzzle, transform.position, Quaternion.LookRotation(transform.forward));
            Destroy(instance, instance.GetComponent<ParticleSystem>().main.duration * 1.2f);
            Activate();
        }

        public void Activate()
        {
            ps = this.GetComponent<ParticleSystem>();
            ps.Play();
            this.GetComponent<Rigidbody>().velocity = transform.forward * speed;
            Invoke(nameof(HitDetected), ps.main.duration * 1.5f);
        }

        private bool _hit = false;
        private void OnTriggerEnter(Collider collider) { HitDetected(); }

        public void HitDetected()
        {
            if (_hit) return;
            _hit = true;

            this.GetComponent<Rigidbody>().isKinematic = true;

            foreach (var child in this.GetComponentsInChildren<ParticleSystem>())
            {
                child.Stop();
                if (child.emission.burstCount > 0)
                    child.Clear();
            }

            Destroy(this);
            Destroy(this.GetComponent<Rigidbody>());
            Destroy(this.GetComponent<Collider>());
            Destroy(this.gameObject, ps.main.duration * 1.2f);

            SpawnImpact();
        }

        private void SpawnImpact()
        {
            if (impact == null) return;

            GameObject instance = Instantiate(impact, transform.position, Quaternion.LookRotation(-transform.forward));
            if (instance.TryGetComponent<ParticleSystem>(out ps))
                Destroy(instance, ps.main.duration * 1.2f);
        }
    }
}