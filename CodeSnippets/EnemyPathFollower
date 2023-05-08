using System;
using System.Collections.Generic;
using GameInformation;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyPathFollower : MonoBehaviour
{
	private Enemy _enemy;
	private Direction _direction;
	private Vector3 _target;
	private Queue<Vector3> _path;

	private void Start()
	{
		_enemy = GetComponent<Enemy>();
		_path = new Queue<Vector3>();
		
		GameObject path = GameManager.instance.spawner.path;
		
		for (int i = 0; i < path.transform.childCount; i++)
			_path.Enqueue(path.transform.GetChild(i).transform.position);
		
		if (_path.Count == 0) Debug.LogError("No path specified!");
		else NextTarget();
	}

	private enum Direction
	{
		Up = 0,
		Down = 180,
		Left = 90,
		Right = -90,
	}

	private void SetDirection(Direction pDirection)
	{
		_direction = pDirection;
		transform.rotation = Quaternion.Euler(0, 0, (int) _direction);
	}

	private void NextTarget()
	{
		if (_path.Count > 0)
		{
			_target = _path.Dequeue();
			
			//Changes direction based on which axis is closer to the target
			Vector3 thisPosition = transform.position;
			float xDiff = _target.x - thisPosition.x;
			float yDiff = _target.y - thisPosition.y;
			
			if (Math.Abs(xDiff) > Math.Abs(yDiff)) SetDirection(xDiff > 0 ? Direction.Right : Direction.Left);
			else SetDirection(yDiff > 0 ? Direction.Up : Direction.Down);
		}
		else
		{
			GameManager.instance.RemoveALife();
			Destroy(transform.parent.gameObject);	
		}
	}

	private void FixedUpdate()
	{
		Vector3 directionVec = Quaternion.Euler(0, 0, (int) _direction) * new Vector3(0,1);
		Vector3 velocity = directionVec * ((_enemy.speed * 0.7f) * UnityEngine.Time.fixedDeltaTime);

		transform.parent.position += velocity;

		//If arrived swap to next target
		if (HasArrivedAtTarget()) NextTarget();
	}

	private bool HasArrivedAtTarget()
	{
		Vector3 position = transform.position;
		switch (_direction)
		{
			case Direction.Up:
				if (_target.y <= position.y) return true;
				break;
			case Direction.Down:
				if (_target.y >= position.y) return true;
				break;
			case Direction.Left:
				if (_target.x >= position.x) return true;
				break;
			case Direction.Right:
				if (_target.x <= position.x) return true;
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}

		return false;
	}

}
