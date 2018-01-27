using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour {

	public float speed;

	public List<Sneeze> sneezes;

	protected Rigidbody2D _rigidBody;
	protected Animator _animator;

	virtual protected void Awake() {

		_rigidBody = GetComponent<Rigidbody2D>();
		_animator = GetComponent<Animator>();
	}

	virtual public void Move(Vector2 direction) {

		_rigidBody.AddForce(direction * speed * Time.deltaTime * 60);
	}

	virtual protected void Sneeze() {

		_animator.SetTrigger("Sneeze");
		// return GameObject.Instantiate(projectile, shootPoint.position, transform.rotation);
	}

	virtual protected void Die() {
		_animator.SetTrigger("Die");
	}

	virtual public void OnDieAnimEnd() {
		Destroy(this.gameObject);
	}
}
