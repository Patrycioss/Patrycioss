[CreateAssetMenu(fileName = "Wave", menuName = "Wave/Wave", order = 0)]
public class Wave : ScriptableObject
{
	[SerializeField] private List<Sequence> _waveSequence = new();
	
	private Queue<Sequence> _sequenceQueue;
	private Queue<Enemy> _enemyQueue;

	private Action<Enemy> _enemyReceiver;
	private Action _waveFinishCallback;

	private GameObject _timerObject;
	private SimpleTimer _timer;

	public float GetDurationSeconds() => _waveSequence.Sum(pSequence => pSequence.duration);

	public void StartWave(Action<Enemy> pEnemyReceiver, Action pWaveFinishCallback)
	{
		_timer = GameManager.instance.timer;

		_enemyReceiver = pEnemyReceiver;
		_waveFinishCallback = pWaveFinishCallback;

		_sequenceQueue = new Queue<Sequence>(_waveSequence);
		NextSequence();
	}

	private void NextSequence()
	{
		if (_sequenceQueue.Count != 0)
		{
			Sequence sequence = _sequenceQueue.Dequeue();
			_enemyQueue = new Queue<Enemy>(sequence.GetEnemies());
			_timer.StartTimer(sequence.duration, NextSequence);

			_spawnInterval = sequence.spawnInterval;
			NextEnemy();
		}
		else
		{
			Destroy(_timerObject);
			_waveFinishCallback();
		}
	}

	
	private float _spawnInterval;
	private void NextEnemy()
	{
		if (_enemyQueue.Count == 0) return;
		_enemyReceiver(_enemyQueue.Dequeue());
		_timer.StartTimer(_spawnInterval, NextEnemy);
	}
}

[Serializable]
public class Sequence
{
	[Tooltip("Duration of the sequence in seconds")]
	[SerializeField] private float _duration;
	
	[Tooltip("Interval between spawns in seconds")] 
	[SerializeField] private float _spawnInterval = 1;
	
	[SerializeField] private List<EnemyCountPair> _enemies;
		
	public float duration => _duration;
	public float spawnInterval => _spawnInterval;
	
	public List<Enemy> GetEnemies()
	{
		List<Enemy> sequence = new List<Enemy>();

		foreach (EnemyCountPair enemyCountPair in _enemies)
			for (int i = 0; i < enemyCountPair._count; i++)
				sequence.Add(GameManager.instance.enemies[(int)enemyCountPair._enemy].GetComponentInChildren<Enemy>());

		return sequence;
	}

	[Serializable]
	private class EnemyCountPair
	{
		public EnumToEnemy _enemy;
		public int _count;
		
		[Serializable]
		public enum EnumToEnemy
		{
			Normal,
			Fast,
			Tank,
		}
	}
}
