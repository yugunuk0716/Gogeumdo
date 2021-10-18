using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pool;
using System;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;

    Pool<Box> boxPool; //박스의 풀
    Pool<FeverBox> feverBoxPool; //피버 박스의 풀
    public GameObject boxPrefab; //박스의 프리팹
    public GameObject feverBoxPrefab; //피버 박스의 프리팹
    public Transform spawnPoint; //박스의 소환지점

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("다수의 풀매니저가 실행중");
            return;
        }
        instance = this;
    }

    private void Start()
    {
        boxPool = new Pool<Box>(new PrefabFactory<Box>(boxPrefab), 15); //15개만큼 미리 만들고
        feverBoxPool = new Pool<FeverBox>(new PrefabFactory<FeverBox>(feverBoxPrefab), 5); //5개만큼 미리 만들고
        boxPool.members.ForEach(b => b.gameObject.SetActive(false)); //전부 꺼두기
        feverBoxPool.members.ForEach(x => x.gameObject.SetActive(false));

        StartCoroutine(SpawnBox()); //코루틴 시작
    }

    public void BoxSpawn()
    {
        Box box = boxPool.Allocate(); //박스의 풀에 박스가있다면 가져오고 없다면 새로 만들기

        EventHandler handler = null;
        handler = (s, e) =>
        {
            //GameManager.instance.boxCount--;
            boxPool.Release(box); //박스의 초기화
            box.Death -= handler; //했으면 빼주기
        };
        box.Death += handler; //생성된 박스의 Death에 추가해줌
        //GameManager.instance.boxCount++;

        //생성한 후 포지션 변경이 필요할경우 여기서 해줘야함.
        box.gameObject.transform.position = spawnPoint.position; //박스의 포지션을 스폰포인트로 해주고
        box.gameObject.SetActive(true); //액티브를 켜줌
    }
    public void FeverBoxSpawn()
    {
        FeverBox box = feverBoxPool.Allocate();

        EventHandler handler = null;

        handler = (s, e) =>
        {
            //피버 실행
            StartCoroutine(FeverManager.instance.Fever());
            feverBoxPool.Release(box);
            box.Death -= handler;
        };
        box.Death += handler; //생성된 박스의 Death에 추가해줌
        //GameManager.instance.boxCount++;

        //생성한 후 포지션 변경이 필요할경우 여기서 해줘야함.
        box.gameObject.transform.position = spawnPoint.position; //박스의 포지션을 스폰포인트로 해주고
        box.gameObject.SetActive(true); //액티브를 켜줌
    }
    

    IEnumerator SpawnBox()
    {
        while (!GameManager.instance.isGameover) //게임오버가 아닐때까지 
        {
            BoxSpawn();
            yield return new WaitForSeconds(4f); //나중에 조절해준다.
        }
    }
}
