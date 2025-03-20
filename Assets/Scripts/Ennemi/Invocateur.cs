using UnityEngine;

public class Invocateur : Ennemi
{
    [SerializeField] public Assaillant minionPrefab; // Préfabriqué du sbire (de type Assaillant)

    protected override void Start()
    {
        // Appeler la méthode Start de la classe parente
        base.Start();
        
    }

    protected override void Update()
    {
        if (player)
        {
            // Calculer la distance entre l'invocateur et le joueur
            float distance = Vector3.Distance(transform.position, player.position);

             // Calculer la direction vers le joueur
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0, directionToPlayer.z));

            // Effectuer la rotation vers le joueur
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            
            // Déplacement vers le joueur
            MoveTowardsPlayer(distance);

            // Si le joueur est à portée, invoquer un sbire
            if (distance <= shootRange && Time.time > lastShootTime + shootInterval)
            {
                SpawnMinion();
                lastShootTime = Time.time;
            }
        }

        // Réduire le temps avant d'être une cible
        timeBeforeBeingCible -= Time.deltaTime;
    }



    protected override void MoveTowardsPlayer(float distanceToPlayer)
    {
        if (distanceToPlayer > shootRange)
        {
            // Calculer la direction vers le joueur
            Vector3 direction = (player.position - transform.position).normalized;

            // Déplacer l'ennemi dans cette direction
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }

    public void SpawnMinion()
    {
        Assaillant minion = Instantiate(minionPrefab, transform.position, Quaternion.identity);
        // Lui donner une référence au joueur
        minion.setPlayer(player);
    }
}