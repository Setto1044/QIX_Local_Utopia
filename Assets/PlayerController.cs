using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private	Movement2D	movement2D;

	private void Awake()
	{
		movement2D = GetComponent<Movement2D>();
	}

	private void Update()
	{
		float x = Input.GetAxisRaw("Horizontal");
		float y = Input.GetAxisRaw("Vertical");

		if ( x != 0 || y != 0 )
		{
			movement2D.MoveDirection = new Vector3(x, y, 0);
		}
	}
}

