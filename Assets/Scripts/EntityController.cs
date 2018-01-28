using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour {

	public float speed;

	//cough tracking
	public int powerOfLastCough;
	public string winningPlayerTag;

	protected Rigidbody2D _rigidBody;
	protected Animator _animator;

	virtual protected void Awake() {

		_rigidBody = GetComponent<Rigidbody2D>();
		_animator = GetComponent<Animator>();
	}

	virtual public void Move(Vector2 direction, float speedModifier = 1.0f) {

		_animator.SetBool("walking", true);

		if(direction.x > 0) transform.localScale = new Vector2(-1, 1);
		else transform.localScale = new Vector2(1, 1);

		_rigidBody.AddForce(direction * speed * speedModifier * Time.deltaTime * 60);
	}

	virtual public void Sneeze() {
 
        _animator.SetTrigger("Sneeze");
		// return GameObject.Instantiate(projectile, shootPoint.position, transform.rotation);
	}

    virtual public void Cough()
    {

        _animator.SetTrigger("Cough");
        // return GameObject.Instantiate(projectile, shootPoint.position, transform.rotation);
    }

    virtual protected void Die() {
		_animator.SetTrigger("Die");
	}

	virtual public void OnDieAnimEnd() {
		Destroy(this.gameObject);
	}

    void OnParticleCollision(GameObject coll)
    {
        Debug.Log("Entity hit by: " + coll.gameObject.tag);
    }
}
