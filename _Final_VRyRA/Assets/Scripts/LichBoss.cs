using System.Collections;
using UnityEngine;
using UnityEngine.Events;

// Datos de una fase. Se llenan en el Inspector: agregar o ajustar fases no requiere tocar código.
[System.Serializable]
public class LichPhase
{
    public GameObject orbitGroup;
    public float orbitSpeed = 30f;   // grados por segundo
    public float shootInterval = 4f; // segundos entre ataques
}

// El jefe final. Máquina de estados (patrón State con enum):
// Waiting -> Shielded (bolitas vivas, dispara) -> Stunned (vulnerable al verde)
//         -> Transition -> siguiente fase ... -> Defeated
public class LichBoss : MonoBehaviour, IEnergyReceiver
{
    private enum LichState { Waiting, Shielded, Stunned, Transition, Defeated }

    [Header("Fases")]
    [SerializeField] private LichPhase[] phases;
    [SerializeField] private int hitsPerPhase = 3;
    [SerializeField] private float phaseTransitionDelay = 1.5f;

    [Header("Ataque")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private BulletPooling projectilePool;
    [SerializeField] private float telegraphTime = 1f;
    [SerializeField] private float aimHeight = 1f;
    [SerializeField] private float turnSpeed = 3f;
    [SerializeField] private float restartDelay = 2f;

    [Header("Eventos")]
    public UnityEvent onTelegraph;   // aviso antes de disparar (el bastón brilla)
    public UnityEvent onShoot;
    public UnityEvent onStunned;     // se rompió la última bolita
    public UnityEvent onRecovered;   // empieza una fase con bolitas nuevas
    public UnityEvent onHit;
    public UnityEvent onRejected;
    public UnityEvent onDefeated;

    private LichState state = LichState.Waiting;
    private int phaseIndex;
    private int orbsRemaining;
    private int hitsTaken;
    private Coroutine shootRoutine;

    private void Awake()
    {
        // Conecta por código el onActivated de cada bolita (así no hay que hacerlo a mano 9 veces)
        foreach (LichPhase phase in phases)
        {
            foreach (Rune orb in phase.orbitGroup.GetComponentsInChildren<Rune>(true))
            {
                Rune capturedOrb = orb;
                orb.onActivated.AddListener(() => HandleOrbDestroyed(capturedOrb));
            }

            phase.orbitGroup.SetActive(false);
        }
    }

    private void OnEnable()
    {
        PlayerHealth.OnPlayerDied += HandlePlayerDied;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerDied -= HandlePlayerDied;
    }

    private void Update()
    {
        if (state == LichState.Waiting || state == LichState.Defeated) return;

        FacePlayer();

        LichPhase phase = phases[phaseIndex];
        phase.orbitGroup.transform.Rotate(0f, phase.orbitSpeed * Time.deltaTime, 0f);
    }

    // Conectar a un PlayerTrigger en la entrada de la arena
    public void StartFight()
    {
        if (state != LichState.Waiting) return;
        StartPhase(0);
    }

    private void StartPhase(int index)
    {
        phaseIndex = index;
        hitsTaken = 0;

        LichPhase phase = phases[phaseIndex];
        phase.orbitGroup.SetActive(true);

        Rune[] orbs = phase.orbitGroup.GetComponentsInChildren<Rune>(true);
        foreach (Rune orb in orbs)
        {
            orb.gameObject.SetActive(true);
            orb.ResetRune();
        }
        orbsRemaining = orbs.Length;

        state = LichState.Shielded;
        onRecovered.Invoke();

        shootRoutine = StartCoroutine(ShootLoop());
    }

    private IEnumerator ShootLoop()
    {
        while (state == LichState.Shielded)
        {
            yield return new WaitForSeconds(phases[phaseIndex].shootInterval);
            if (state != LichState.Shielded) yield break;

            onTelegraph.Invoke();
            yield return new WaitForSeconds(telegraphTime);
            if (state != LichState.Shielded) yield break;

            Shoot();
        }
    }

    // Apunta a donde está el jugador EN ESTE MOMENTO; el proyectil ya no lo sigue
    private void Shoot()
    {
        Vector3 target = player.position + Vector3.up * aimHeight;
        shootPoint.LookAt(target);
        projectilePool.GetObject(shootPoint);
        onShoot.Invoke();
    }

    private void HandleOrbDestroyed(Rune orb)
    {
        orb.gameObject.SetActive(false);

        if (state != LichState.Shielded) return;

        orbsRemaining--;
        if (orbsRemaining <= 0) BecomeStunned();
    }

    private void BecomeStunned()
    {
        state = LichState.Stunned;
        if (shootRoutine != null) StopCoroutine(shootRoutine);
        onStunned.Invoke();
    }

    public void ReceiveEnergy(EnergyType energy)
    {
        if (state == LichState.Stunned && energy == EnergyType.Green)
        {
            hitsTaken++;
            onHit.Invoke();

            if (hitsTaken >= hitsPerPhase) CompletePhase();
        }
        else if (state == LichState.Shielded || state == LichState.Stunned)
        {
            // Bolitas vivas, o rosa al Lich: "todavía no"
            onRejected.Invoke();
        }
    }

    private void CompletePhase()
    {
        phases[phaseIndex].orbitGroup.SetActive(false);
        phaseIndex++;

        if (phaseIndex >= phases.Length)
        {
            state = LichState.Defeated;
            onDefeated.Invoke();
        }
        else
        {
            state = LichState.Transition;
            StartCoroutine(StartPhaseAfterDelay(phaseTransitionDelay));
        }
    }

    // Si el jugador muere, se repite la fase ACTUAL (las fases ganadas se conservan)
    private void HandlePlayerDied()
    {
        if (state == LichState.Waiting || state == LichState.Defeated) return;

        StopAllCoroutines();
        state = LichState.Transition;
        StartCoroutine(StartPhaseAfterDelay(restartDelay));
    }

    private IEnumerator StartPhaseAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartPhase(phaseIndex);
    }

    private void FacePlayer()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;
        if (direction.sqrMagnitude < 0.01f) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }
}
