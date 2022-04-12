using UnityEngine;

public class DestroyableBoardObject : BoardObject, IDamageable
{
    [SerializeField] private int startingHealth;
    private int currentHealth;
    public void TakeDamage(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth - amount, 0, startingHealth);

        if (currentHealth <= 0)
        {
            DestroyBoardObject();
        }
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, startingHealth);
    }

    private void DestroyBoardObject()
    {
        Tile.Data.content.Remove(this);
        Destroy(this.gameObject);
    }
}