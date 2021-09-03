using UnityEngine;

public class DeathSimulator : MonoBehaviour
{
	[SerializeField] [Range(0, 100)] int health;
	[SerializeField] [Range(0, 100)] int damagePerHit;
	[SerializeField] [Range(0, 100)] int regen;
	[SerializeField] HealthBar healthBar;
	[Space]
	[SerializeField] GameObject gameObjectOnOff;

	public AudioSource whail;

	private void Start()
	{
		healthBar.SetMaxHealth(health);
		whail.Stop();
	}

	private void Update()
	{
		if (health <= 0)
		{
			gameObjectOnOff.SetActive(true);
			Time.timeScale = 0f;

			AudioListener.pause = true;
		}
	}

	private void OnCollisionStay(Collision collision)
	{
		if (collision.collider.CompareTag("Enemy"))
		{
			health -= damagePerHit;
			healthBar.SetHealth(health);
			whail.Play(0);
		}
        else if(health>0 && health<90)
        {
			health += regen;
			healthBar.SetHealth(health);
			whail.Stop();
		}

		

	}


}
