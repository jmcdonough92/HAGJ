using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{

    private bool _facingRight = true;
    private bool _isSwimming;
    private bool _isWaiting;

    [SerializeField]
    private float _speed = 2;

    private Animator _swimAC;
    private float _waitTime;
    private Vector2 _lastGoal;
    private Vector2 _goal = Vector2.zero;

    private Vector3 _lastPosition = Vector3.zero;
    // Start is called before the first frame update
    void Awake()
    {
        _swimAC = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isWaiting && !_isSwimming)
        {
            GenerateWait();
            GenerateGoal();
        }
    }

    void GenerateWait()
    {
        _waitTime = Random.Range(0.0f, 4.0f);
        _isWaiting = true;
        StartCoroutine("Wait");
    }

    void GenerateGoal()
    {
        _lastGoal = _goal;
        _goal = new Vector2(Random.Range(-2.4f, 5.5f), Random.Range(-0.4f, 3.0f));
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(_waitTime);
        _isWaiting = false;
        if (_goal.x - _lastGoal.x > 0)
        {
            transform.eulerAngles = Vector3.zero;
        }
        else
        {
            transform.eulerAngles = Vector3.up * 180;
        }
        _swimAC.SetFloat("SwimSpeed", 1.5f);
        StartCoroutine("Move");
        _isSwimming = true;
    }

    private IEnumerator Move()
    {
        while (Vector3.Distance(transform.position,_goal) > 0.01f)
        {
            _lastPosition = transform.position;
            transform.position = Vector3.Lerp(transform.position, _goal, _speed * Time.deltaTime);
            
            yield return null;
        }
        _isSwimming = false;
        _swimAC.SetFloat("SwimSpeed", 1);
    }
}
